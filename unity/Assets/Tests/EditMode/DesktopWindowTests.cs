using NUnit.Framework;
using UnityEngine;

namespace SomethingDownThere.Tests
{
    public sealed class DesktopWindowTests
    {
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
