using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SomethingDownThere.Tests
{
    public sealed class FpsInputTests : InputTestFixture
    {
        private Keyboard keyboard;
        private Mouse mouse;
        private FpsInput input;

        public override void Setup()
        {
            base.Setup();
            keyboard = InputSystem.AddDevice<Keyboard>();
            mouse = InputSystem.AddDevice<Mouse>();
            input = new FpsInput();
            input.Enable();
            InputSystem.Update();
            input.Read();
        }

        public override void TearDown()
        {
            input.Dispose();
            base.TearDown();
        }

        [Test]
        public void WasdDiagonalIsNormalizedAndMouseDeltaIsPreserved()
        {
            Press(keyboard.wKey);
            Press(keyboard.dKey);
            Set(mouse.delta, new Vector2(5, -3));
            var frame = input.Read();
            Assert.That(frame.Move.magnitude, Is.EqualTo(1f).Within(0.0001f));
            Assert.That(frame.Look, Is.EqualTo(new Vector2(5, -3)));
        }

        [Test]
        public void InteractIsAnEdgeButDigAndJetpackRemainHeld()
        {
            Press(keyboard.eKey, queueEventOnly: true);
            Press(keyboard.spaceKey, queueEventOnly: true);
            Press(mouse.leftButton);
            Assert.That(input.Read().InteractPressed, Is.True);
            InputSystem.Update();
            var held = input.Read();
            Assert.That(held.InteractPressed, Is.False);
            Assert.That(held.DigHeld, Is.True);
            Assert.That(held.JetpackHeld, Is.True);
        }

        [Test]
        public void ResumeRequiresReleaseOfEachHeldWorldAction()
        {
            Press(keyboard.eKey, queueEventOnly: true);
            Press(keyboard.spaceKey, queueEventOnly: true);
            Press(mouse.leftButton);
            input.SuppressHeldActions();
            var blocked = input.Read();
            Assert.That(blocked.InteractPressed || blocked.DigHeld || blocked.JetpackHeld, Is.False);
            Release(mouse.leftButton);
            input.Read();
            Press(mouse.leftButton);
            var partlyReleased = input.Read();
            Assert.That(partlyReleased.DigHeld, Is.True);
            Assert.That(partlyReleased.JetpackHeld || partlyReleased.InteractPressed, Is.False);
            Release(keyboard.eKey);
            Release(keyboard.spaceKey);
            input.Read();
            Press(keyboard.eKey, queueEventOnly: true);
            Press(keyboard.spaceKey);
            var rearmed = input.Read();
            Assert.That(rearmed.InteractPressed && rearmed.JetpackHeld, Is.True);
        }

        [Test]
        public void MenuBindingsRemainAvailableWhileWorldActionsAreSuppressed()
        {
            input.SuppressHeldActions();
            Press(keyboard.tabKey);
            Assert.That(input.Read().InventoryPressed, Is.True);
            Press(keyboard.escapeKey);
            Assert.That(input.Read().BackPressed, Is.True);
        }

        [Test]
        public void MissingDevicesProduceNeutralInput()
        {
            InputSystem.RemoveDevice(keyboard);
            InputSystem.RemoveDevice(mouse);
            InputSystem.Update();
            var frame = input.Read();
            Assert.That(frame.Move, Is.EqualTo(Vector2.zero));
            Assert.That(frame.Look, Is.EqualTo(Vector2.zero));
            Assert.That(frame.DigHeld || frame.JetpackHeld || frame.InteractPressed, Is.False);
        }
    }
}
