using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace JustAFewPeppers
{
    // Each session owns its enabled maps; the authored asset is never runtime state.
    public sealed class YardInput : IDisposable
    {
        public InputActionAsset Actions { get; }
        public InputAction Move { get; }
        public InputAction Look { get; }
        public InputAction Reset { get; }
        public InputAction Pause { get; }
        readonly InputActionMap gameplay;
        readonly InputActionMap ui;
        readonly InputSystemUIInputModule module;
        readonly List<InputActionReference> references = new List<InputActionReference>();

        public YardInput(InputActionAsset source, InputSystemUIInputModule uiModule)
        {
            Actions = UnityEngine.Object.Instantiate(source);
            gameplay = Actions.FindActionMap("Gameplay", true);
            ui = Actions.FindActionMap("UI", true);
            Move = gameplay.FindAction("Move", true);
            Look = gameplay.FindAction("Look", true);
            Reset = gameplay.FindAction("Reset", true);
            Pause = Actions.FindAction("System/Pause", true);
            module = uiModule;
            module.enabled = false;
            module.actionsAsset = Actions;
            module.move = Reference("Navigate");
            module.submit = Reference("Submit");
            module.cancel = Reference("Cancel");
            module.point = Reference("Point");
            module.leftClick = Reference("Click");
            Pause.actionMap.Enable();
        }

        InputActionReference Reference(string name)
        {
            var reference = InputActionReference.Create(ui.FindAction(name, true));
            references.Add(reference);
            return reference;
        }

        public void SetPaused(bool paused)
        {
            if (paused)
            {
                gameplay.Disable();
                ui.Enable();
                module.enabled = true;
            }
            else
            {
                module.enabled = false;
                ui.Disable();
                gameplay.Enable();
            }
        }

        public void Dispose()
        {
            if (module != null) module.enabled = false;
            Actions.Disable();
            foreach (var reference in references) UnityEngine.Object.Destroy(reference);
            UnityEngine.Object.Destroy(Actions);
        }
    }
}
