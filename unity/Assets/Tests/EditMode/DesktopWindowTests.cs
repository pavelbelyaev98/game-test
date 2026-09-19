using NUnit.Framework;
using UnityEngine;

namespace SomethingDownThere.Tests
{
    public sealed class DesktopWindowTests
    {
        [Test]
        public void Supported4k8kAndUltrawideRemainSelectableWithLowerDesktopMode()
        {
            var reported = new[] { new Vector2Int(1920, 1080), new Vector2Int(3440, 1440),
                new Vector2Int(3840, 2160), new Vector2Int(3840, 2160), new Vector2Int(7680, 4320) };
            var options = DesktopWindow.ResolutionOptions(reported, new Vector2Int(1920, 1080), new Vector2Int(1280, 720));
            Assert.That(options, Does.Contain(new Vector2Int(3840, 2160)));
            Assert.That(options, Does.Contain(new Vector2Int(3440, 1440)));
            Assert.That(options, Does.Contain(new Vector2Int(7680, 4320)));
            Assert.That(options, Is.Unique, "Different refresh rates must not duplicate resolution entries.");
        }

        [Test]
        public void MissingReportedModesUseDesktopAndSafeFallbacksWithoutInventing4k()
        {
            var desktop = new Vector2Int(2560, 1440);
            var options = DesktopWindow.ResolutionOptions(System.Array.Empty<Vector2Int>(), desktop, new Vector2Int(1280, 720));
            Assert.That(options, Does.Contain(desktop));
            Assert.That(options, Does.Contain(new Vector2Int(1280, 720)));
            Assert.That(options, Has.No.Member(new Vector2Int(3840, 2160)));
            Assert.That(DesktopWindow.ResolutionOptions(System.Array.Empty<Vector2Int>(), Vector2Int.zero, Vector2Int.zero),
                Is.EqualTo(new[] { new Vector2Int(1280, 720) }));
        }

        [Test]
        public void WindowsPlayerUsesQuietGuardInsteadOfUnityFatalErrorDialog()
        {
            Assert.That(UnityEditor.PlayerSettings.forceSingleInstance, Is.False,
                "Unity's native guard shows a Fatal error dialog; DesktopInstance owns quiet launch handling.");
        }

        [Test]
        public void ApplicationReservationRejectsDuplicatesAndAllowsRelaunchAfterExit()
        {
            string identity = @"Local\SDT-instance-test-" + System.Guid.NewGuid().ToString("N");
            using (var first = DesktopInstance.TryAcquire(identity))
            {
                Assert.That(first, Is.Not.Null);
                using var duplicate = DesktopInstance.TryAcquire(identity);
                Assert.That(duplicate, Is.Null);
            }
            using var relaunched = DesktopInstance.TryAcquire(identity);
            Assert.That(relaunched, Is.Not.Null);
        }

        [TestCase(2560, 1440, 2560, 1440)]
        [TestCase(1920, 1080, 1920, 1080)]
        [TestCase(3840, 2160, 3840, 2160)]
        [TestCase(1280, 800, 1280, 800)]
        [TestCase(3440, 1440, 3440, 1440)]
        [TestCase(960, 540, 960, 540)]
        [TestCase(0, 0, 1280, 720)]
        public void RecommendedDisplayUsesNativeAspectAndBorderless(int width, int height, int expectedWidth, int expectedHeight)
        {
            var result = DesktopWindow.RecommendedDisplay(width, height);
            Assert.That(result.Width, Is.EqualTo(expectedWidth));
            Assert.That(result.Height, Is.EqualTo(expectedHeight));
            Assert.That(result.Mode, Is.Zero);
        }
    }
}
