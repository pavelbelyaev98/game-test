using System;
using System.IO;
using NUnit.Framework;

namespace SomethingDownThere.Tests
{
    public sealed class CameraPreferencesTests
    {
        private sealed class Store : ICameraPreferencesStore
        {
            public string Contents;
            public int Writes;
            public bool Fail;
            public string Read() => Contents;
            public void Write(string contents)
            {
                Writes++;
                if (Fail) throw new IOException("Unavailable test volume");
                Contents = contents;
            }
        }

        [TestCase(null, 75, true)]
        [TestCase("corrupt", 75, true)]
        [TestCase("version=2\nverticalFov=60\nsteadyCrosshair=false", 75, true)]
        [TestCase("version=1", 75, true)]
        [TestCase("version=1\nverticalFov=82\nsteadyCrosshair=false", 82, false)]
        [TestCase("version=1\nverticalFov=-100\nsteadyCrosshair=false", 55, false)]
        [TestCase("version=1\nverticalFov=1e30\nsteadyCrosshair=invalid", 90, true)]
        [TestCase("version=1\nverticalFov=NaN\nsteadyCrosshair=false", 75, false)]
        [TestCase("version=1\nverticalFov=Infinity", 75, true)]
        [TestCase("version=1\nverticalFov=broken\nsteadyCrosshair=0", 75, true)]
        [TestCase("version=1\r\nverticalFov=66.8\r\nsteadyCrosshair=true\r\n", 67, true)]
        public void StoredFieldsUseSafeIndependentDefaults(string contents, int fov, bool steady)
        {
            var store = new Store { Contents = contents };
            var preferences = new CameraPreferences(store);
            Assert.That(preferences.VerticalFov, Is.EqualTo(fov));
            Assert.That(preferences.SteadyCrosshair, Is.EqualTo(steady));
            preferences.Flush();
            Assert.That(store.Writes, Is.Zero, "Reading or normalizing preferences never rewrites the source.");
        }

        [Test]
        public void DraggingWritesOnlyAtBoundariesAndFailuresKeepRetryableSessionValues()
        {
            var store = new Store();
            var preferences = new CameraPreferences(store);
            for (int value = 55; value <= 90; value++) preferences.SetVerticalFov(value);
            preferences.SetSteadyCrosshair(false);
            Assert.That(store.Writes, Is.Zero);
            store.Fail = true;
            Assert.That(preferences.Flush(), Is.False);
            Assert.That(preferences.VerticalFov, Is.EqualTo(90));
            Assert.That(preferences.WriteFailed && preferences.HasUnsavedChanges, Is.True);
            store.Fail = false;
            Assert.That(preferences.Flush(), Is.True);
            Assert.That(preferences.WriteFailed || preferences.HasUnsavedChanges, Is.False);
            var reloaded = new CameraPreferences(store);
            Assert.That(reloaded.VerticalFov, Is.EqualTo(90));
            Assert.That(reloaded.SteadyCrosshair, Is.False);
            int writes = store.Writes, events = 0;
            preferences.Changed += () => events++;
            for (int i = 0; i < 1000; i++)
            {
                preferences.SetVerticalFov(90);
                preferences.SetSteadyCrosshair(false);
                preferences.Flush();
            }
            Assert.That(store.Writes, Is.EqualTo(writes));
            Assert.That(events, Is.Zero);
            preferences.Reset();
            reloaded = new CameraPreferences(store);
            Assert.That(reloaded.VerticalFov, Is.EqualTo(75));
            Assert.That(reloaded.SteadyCrosshair, Is.True);
        }

        [Test]
        public void AtomicSettingsReplacementPreservesOtherFilesAndRecoversFromLockedDestination()
        {
            string directory = Path.Combine(Path.GetTempPath(), "SDT-camera-tests", Guid.NewGuid().ToString("N"));
            string path = Path.Combine(directory, "camera-v1.ini");
            try
            {
                var store = new CameraPreferencesFile(path);
                var preferences = new CameraPreferences(store);
                preferences.Reset();
                string previous = File.ReadAllText(path);
                string world = Path.Combine(directory, "world.sav");
                File.WriteAllText(world, "unrelated excavation");
                using (var locked = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    preferences.SetVerticalFov(55);
                    Assert.That(preferences.Flush(), Is.False);
                    Assert.That(File.ReadAllText(path), Is.EqualTo(previous));
                }
                Assert.That(preferences.Flush(), Is.True);
                Assert.That(new CameraPreferences(store).VerticalFov, Is.EqualTo(55));
                Assert.That(File.ReadAllText(world), Is.EqualTo("unrelated excavation"));
                Assert.That(File.Exists(path + ".pending"), Is.False);
                File.WriteAllText(path + ".pending", "interrupted replacement");
                Assert.That(new CameraPreferences(store).VerticalFov, Is.EqualTo(55));
                File.WriteAllText(path, new string('x', 5000));
                Assert.That(new CameraPreferences(store).VerticalFov, Is.EqualTo(75));
            }
            finally { if (Directory.Exists(directory)) Directory.Delete(directory, true); }
        }
    }
}
