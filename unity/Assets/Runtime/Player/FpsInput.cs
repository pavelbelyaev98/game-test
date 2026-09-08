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
        public bool DigPressed;
        public bool JumpPressed;
        public bool JetpackHeld;
        public bool InteractPressed;
        public bool InventoryPressed;
        public bool BackPressed;
        public bool AdminMenuPressed;
        public int AdminLevel;
        public bool RefillPressed;
        public bool ReturnPressed;
        public bool XrayPressed;
    }

    public sealed class FpsInput : IDisposable
    {
        private readonly InputActionMap actions = new InputActionMap("FPS");
        private readonly InputAction move, look, dig, jetpack, interact, inventory, back;
        private readonly InputAction refill, returnToSurface, adminMenu, adminCtrl, adminShift, xray;
        private readonly InputAction[] adminLevels = new InputAction[6];
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
            refill = actions.AddAction("AdminRefill", InputActionType.Button, "<Keyboard>/r");
            returnToSurface = actions.AddAction("AdminReturn", InputActionType.Button, "<Keyboard>/home");
            adminMenu = actions.AddAction("AdminMenu", InputActionType.Button, "<Keyboard>/f10");
            xray = actions.AddAction("AdminXray", InputActionType.Button, "<Keyboard>/x");
            adminCtrl = actions.AddAction("AdminCtrl", InputActionType.Button, "<Keyboard>/ctrl");
            adminShift = actions.AddAction("AdminShift", InputActionType.Button, "<Keyboard>/shift");
            for (int i = 0; i < adminLevels.Length; i++)
            {
                adminLevels[i] = actions.AddAction("AdminLevel" + (i + 1), InputActionType.Button, "<Keyboard>/" + (i + 1));
                adminLevels[i].AddBinding("<Keyboard>/numpad" + (i + 1));
            }
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
            int adminLevel = 0;
            // Require modifiers before the action-key edge. Pressing Ctrl/Shift after
            // an ordinary key is held must never turn that old press into an admin action.
            bool adminChord = FpsPlayer.AdminBuild && adminCtrl.IsPressed() && adminShift.IsPressed();
            for (int i = 0; i < adminLevels.Length; i++)
                if (adminChord && adminLevels[i].WasPressedThisFrame()) adminLevel = i + 1;

            return new FpsInputFrame
            {
                Move = move.ReadValue<Vector2>(),
                Look = look.ReadValue<Vector2>(),
                DigHeld = digArmed && digHeld,
                DigPressed = digArmed && dig.WasPressedThisFrame(),
                JumpPressed = jetpackArmed && jetpack.WasPressedThisFrame(),
                JetpackHeld = jetpackArmed && jetpackHeld,
                InteractPressed = interactArmed && interact.WasPressedThisFrame(),
                InventoryPressed = inventory.WasPressedThisFrame(),
                BackPressed = back.WasPressedThisFrame(),
                AdminMenuPressed = adminChord && adminMenu.WasPressedThisFrame(),
                AdminLevel = adminLevel,
                RefillPressed = adminChord && refill.WasPressedThisFrame(),
                ReturnPressed = adminChord && returnToSurface.WasPressedThisFrame(),
                XrayPressed = adminChord && xray.WasPressedThisFrame()
            };
        }

        public void Dispose() => actions.Dispose();
    }
}
