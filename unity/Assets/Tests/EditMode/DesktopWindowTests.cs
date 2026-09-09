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

        [TestCase(2560, 1440, 2560, 1392, 1920, 1080)]
        [TestCase(1920, 1080, 1920, 1040, 1440, 810)]
        [TestCase(3840, 2160, 3840, 2100, 1920, 1080)]
        [TestCase(1280, 720, 1280, 680, 960, 540)]
        [TestCase(2560, 1440, 1600, 900, 1472, 828)]
        [TestCase(2560, 1440, 0, 0, 1920, 1080)]
        [TestCase(0, 0, 0, 0, 1280, 720)]
        public void InitialWindowLeavesDesktopSpaceAndFitsAvailableArea(
            int width, int height, int workWidth, int workHeight, int expectedWidth, int expectedHeight)
        {
            Assert.That(DesktopWindow.ChooseInitialSize(width, height, workWidth, workHeight),
                Is.EqualTo(new Vector2Int(expectedWidth, expectedHeight)));
        }
    }
}
