using UnityEngine;

namespace Chushkopek.Stage0
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class Stage0Interaction : MonoBehaviour
    {
        public Stage0Session session;
        public Camera view;
        [Range(.4f, 4f)] public float sensitivity = 1.6f;
        public float reach = 2.8f;
        CharacterController controller;
        Vector3 initialPosition;
        Quaternion initialRotation;
        float pitch = 25f;
        int draggingStrip = -1;
        bool waitingForMouseRelease;
        Vector2 peelDirection;
        Stage0Target target;
        string hint = "";
        GUIStyle titleStyle, textStyle, hintStyle, smallStyle;
        public bool IsDragging => draggingStrip >= 0;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            ResetView();
            session.SetPaused(false);
        }

        public void ResetView()
        {
            EndDrag();
            if (!controller) return;
            controller.enabled = false;
            transform.SetPositionAndRotation(initialPosition, initialRotation);
            controller.enabled = true;
            pitch = 25f;
            view.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) { session.SetPaused(!session.Paused); return; }
            if (session.Paused) return;
            if (Input.GetKeyDown(KeyCode.R)) { session.ResetCycle(); return; }
            if (Input.GetMouseButtonDown(1)) { EndDrag(); session.CancelCarry(); return; }
            if (waitingForMouseRelease)
            {
                if (Input.GetMouseButton(0)) return;
                waitingForMouseRelease = false;
            }
            Vector2 mouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            if (draggingStrip >= 0)
            {
                if (!Input.GetMouseButton(0)) { EndDrag(); return; }
                float positiveMovement = Mathf.Max(0f, Vector2.Dot(mouse, peelDirection));
                float resistance = session.State.StripProgress(draggingStrip) < .09f ? .42f : 1f;
                session.Peel(draggingStrip, positiveMovement * .024f * resistance);
                hint = "PULL " + DirectionWord() + "  /  release to rest; your progress stays";
                if (session.State.StripProgress(draggingStrip) >= 1f) EndDrag();
                return;
            }

            transform.Rotate(0f, mouse.x * sensitivity, 0f);
            pitch = Mathf.Clamp(pitch - mouse.y * sensitivity, -55f, 75f);
            view.transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
            float x = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
            float z = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);
            controller.SimpleMove(Vector3.ClampMagnitude(transform.right * x + transform.forward * z, 1f) * 1.35f);

            target = null;
            session.pepper.HoverStrip = -1;
            Ray ray = view.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
            if (Physics.Raycast(ray, out RaycastHit hit, reach, ~0, QueryTriggerInteraction.Collide))
                target = hit.collider.GetComponentInParent<Stage0Target>();
            hint = HintFor(target);
            if (target && target.isPepper && session.State.Stage == PepperStage.Peelable)
            {
                session.pepper.HoverStrip = session.pepper.ChooseStrip(hit.point);
                hint = "HOLD LEFT MOUSE on this broad edge, then pull along the pepper";
            }
            if (!Input.GetMouseButtonDown(0)) return;
            if (!target) { session.Explain(session.State.Location == PepperLocation.Held ? "Aim at a marked work position to place the pepper." : "Aim at the pepper or a work position within reach."); return; }
            if (target.isPepper)
            {
                if (session.State.Stage == PepperStage.Peelable)
                {
                    draggingStrip = session.pepper.ChooseStrip(hit.point);
                    peelDirection = session.pepper.ScreenPeelDirection(view);
                    session.pepper.HoverStrip = draggingStrip;
                    hint = "PULL " + DirectionWord();
                }
                else session.Pick();
            }
            else if (session.State.Location == target.location) session.Pick();
            else session.Place(target.location);
        }

        string DirectionWord()
        {
            return Mathf.Abs(peelDirection.y) >= Mathf.Abs(peelDirection.x)
                ? peelDirection.y >= 0 ? "UP" : "DOWN"
                : peelDirection.x >= 0 ? "RIGHT" : "LEFT";
        }

        string HintFor(Stage0Target selected)
        {
            if (!selected) return session.State.Location == PepperLocation.Held ? "Pepper held — aim at a work position" : "Aim at the pepper";
            if (selected.isPepper || session.State.Location == selected.location)
            {
                if (session.State.Stage == PepperStage.Steaming) return "STEAMING — the cover will open shortly";
                if (session.State.Stage == PepperStage.Peelable) return "Aim at the broad skin on the pepper to peel";
                if (session.State.Stage == PepperStage.Finished) return "PREPARED — R to try another cycle";
                return "LEFT CLICK — lift pepper";
            }
            return "LEFT CLICK — " + selected.label;
        }

        public void EndDrag() { draggingStrip = -1; waitingForMouseRelease = Input.GetMouseButton(0); }

        void OnGUI()
        {
            if (session.State == null) return;
            if (titleStyle == null)
            {
                titleStyle = Style(24, FontStyle.Bold, new Color(.96f, .91f, .78f));
                textStyle = Style(16, FontStyle.Normal, new Color(.93f, .93f, .89f));
                hintStyle = Style(19, FontStyle.Bold, new Color(.98f, .85f, .48f));
                smallStyle = Style(13, FontStyle.Normal, new Color(.7f, .76f, .74f));
            }
            float scale = Mathf.Max(.65f, Screen.height / 900f);
            GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));
            float width = Screen.width / scale, height = Screen.height / scale;
            Panel(new Rect(24, 22, 520, 104));
            GUI.Label(new Rect(42, 33, 480, 35), "CHUSHKOPEK  /  ONE PEPPER", titleStyle);
            GUI.Label(new Rect(42, 73, 480, 28), "ROAST   →   STEAM   →   PEEL   →   FINISH", smallStyle);
            GUI.Label(new Rect(42, 96, 480, 25), Status(), textStyle);
            Panel(new Rect(24, height - 136, width - 48, 112));
            GUI.Label(new Rect(42, height - 124, width - 84, 35), hint, hintStyle);
            GUI.Label(new Rect(42, height - 85, width - 84, 35), Time.unscaledTime < session.NoticeUntil ? session.Notice : NextStep(), textStyle);
            GUI.Label(new Rect(42, height - 48, width - 84, 25), "WASD move  ·  Mouse look  ·  Left click pick/place  ·  Right click return  ·  R reset  ·  Esc pause", smallStyle);
            GUI.color = target || IsDragging ? new Color(1f, .87f, .48f) : Color.white;
            GUI.DrawTexture(new Rect(width / 2f - 2, height / 2f - 2, 4, 4), Texture2D.whiteTexture);
            GUI.color = Color.white;
            if (session.Paused)
            {
                Rect panel = new Rect(width / 2f - 230f, height / 2f - 135f, 460, 270);
                Panel(panel);
                GUI.Label(new Rect(panel.x + 24, panel.y + 18, 410, 35), "PAUSED", titleStyle);
                GUI.Label(new Rect(panel.x + 24, panel.y + 58, 410, 30), "Mouse sensitivity", textStyle);
                sensitivity = GUI.HorizontalSlider(new Rect(panel.x + 24, panel.y + 98, 410, 24), sensitivity, .4f, 4f);
                if (GUI.Button(new Rect(panel.x + 24, panel.y + 140, 198, 42), "Resume")) session.SetPaused(false);
                if (GUI.Button(new Rect(panel.x + 238, panel.y + 140, 198, 42), "Reset pepper")) { session.ResetCycle(); session.SetPaused(false); }
                if (GUI.Button(new Rect(panel.x + 24, panel.y + 198, 412, 38), "Quit")) Application.Quit();
            }
            GUI.matrix = Matrix4x4.identity;
        }

        string Status()
        {
            var p = session.State;
            if (p.Stage == PepperStage.Perfect) return p.IsHeating ? "READY — lift now" : "Perfect roast — heat stopped";
            if (p.Stage == PepperStage.Burnt) return "Burnt — still processable";
            if (p.Stage == PepperStage.Peelable) return "Skin ready — pull two broad strips";
            return p.Stage == PepperStage.Finished ? "Prepared — one transformed pepper" : p.Stage.ToString();
        }
        string NextStep()
        {
            switch (session.State.Stage)
            {
                case PepperStage.Raw: return "Place the pepper in the single roasting socket.";
                case PepperStage.Roasting: return session.State.IsHeating ? "Listen and watch the skin blister." : "Return it to the roaster to finish heating.";
                case PepperStage.Perfect: case PepperStage.Burnt: return "Lift the pepper and use the steaming position.";
                case PepperStage.Steaming: return "The short steam is loosening the skin.";
                case PepperStage.Peelable: return "Hold a broad edge and drag. Let go at any time; progress stays.";
                case PepperStage.Peeled: return "Lift the peeled pepper and place it on the finished tray.";
                default: return "Would you like to do that again? R resets the same pepper.";
            }
        }
        static GUIStyle Style(int size, FontStyle weight, Color color)
        {
            var style = new GUIStyle(GUI.skin.label) { fontSize = size, fontStyle = weight, wordWrap = true };
            style.normal.textColor = color;
            return style;
        }
        static void Panel(Rect rect)
        {
            GUI.color = new Color(.055f, .085f, .078f, .92f);
            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = Color.white;
        }
    }
}
