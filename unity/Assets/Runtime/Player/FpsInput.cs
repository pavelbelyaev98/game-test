using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SomethingDownThere
{
    public struct FpsInputFrame
    {
        public Vector2 Move;
        public Vector2 Look;
        public bool DigHeld;
        public bool JumpPressed;
        public bool JetpackHeld;
        public bool InteractPressed;
        public bool InventoryPressed;
        public bool BackPressed;
    }

    public sealed class FpsInput : IDisposable
    {
        private readonly InputActionMap actions = new InputActionMap("FPS");
        private readonly InputAction move, look, dig, jetpack, interact, inventory, back;
        private bool digArmed, jetpackArmed, interactArmed;

        public FpsInput()
        {
            move = actions.AddAction("Move", InputActionType.Value);
            move.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");
            look = actions.AddAction("Look", InputActionType.Value, "<Mouse>/delta");
            dig = actions.AddAction("Dig", InputActionType.Button, "<Mouse>/leftButton");
            jetpack = actions.AddAction("JumpAndJetpack", InputActionType.Button, "<Keyboard>/space");
            interact = actions.AddAction("Interact", InputActionType.Button, "<Keyboard>/e");
            inventory = actions.AddAction("Inventory", InputActionType.Button, "<Keyboard>/tab");
            back = actions.AddAction("Back", InputActionType.Button, "<Keyboard>/escape");
        }

        public void Enable()
        {
            SuppressHeldActions();
            actions.Enable();
        }

        public void Disable() => actions.Disable();

        public void SuppressHeldActions()
        {
            digArmed = jetpackArmed = interactArmed = false;
        }

        public FpsInputFrame Read()
        {
            bool digHeld = dig.IsPressed();
            bool jetpackHeld = jetpack.IsPressed();
            bool interactHeld = interact.IsPressed();
            if (!digHeld) digArmed = true;
            if (!jetpackHeld) jetpackArmed = true;
            if (!interactHeld) interactArmed = true;

            return new FpsInputFrame
            {
                Move = move.ReadValue<Vector2>(),
                Look = look.ReadValue<Vector2>(),
                DigHeld = digArmed && digHeld,
                JumpPressed = jetpackArmed && jetpack.WasPressedThisFrame(),
                JetpackHeld = jetpackArmed && jetpackHeld,
                InteractPressed = interactArmed && interact.WasPressedThisFrame(),
                InventoryPressed = inventory.WasPressedThisFrame(),
                BackPressed = back.WasPressedThisFrame()
            };
        }

        public void Dispose() => actions.Dispose();
    }
}
