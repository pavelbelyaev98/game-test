using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static JustAFewPeppers.Editor.FoundationSceneBuilder;

namespace JustAFewPeppers.Editor
{
    // Edits the existing HUD only; never regenerates the yard or input asset.
    public static class ComfortAuthoring
    {
        [MenuItem("Just a few peppers/Apply first playable comfort")]
        public static void Apply()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            if (session == null || session.handling == null || session.handling.finished == null)
                throw new InvalidOperationException("Comfort requires the completed finished-food scene.");
            var hud = session.hud;
            if (hud.guidanceText != null || hud.sensitivitySlider != null)
                throw new InvalidOperationException("Comfort is already authored. Edit the saved scene directly.");

            hud.transform.Find("Scope").GetComponent<Text>().text = "Prepare the harvest for winter";
            hud.controlsText = hud.transform.Find("Controls").GetComponent<Text>();
            hud.controlsText.transform.SetParent(hud.pausePanel.transform, false);
            hud.controlsText.rectTransform.anchoredPosition = new Vector2(0, 179);
            hud.controlsText.rectTransform.sizeDelta = new Vector2(900, 140);
            hud.controlsText.fontSize = 19;
            hud.controlsText.text = "WASD / Arrows  Move     Shift  Sprint     Space  Jump     Mouse  Look\n" +
                "E  Pick up / place gently on ground / tip at intake / collect / hand off\n" +
                "Hold left mouse  Scoop (release to stop)     G  Drop     Z / X  Optional rotation\n" +
                "Esc  Pause / resume     R  Recover carriers and return to gate (keeps food)\n" +
                "F8  Restart food test (clears every load and stored food)";
            hud.guidanceText = Text("Current action guidance", hud.transform, "", new Vector2(0, -390), new Vector2(1320, 80), 21);
            // Keep the existing dark footer behind its new, shorter contents.
            var backing = hud.transform.Find("Controls backing");
            backing.GetComponent<RectTransform>().sizeDelta = new Vector2(1400, 88);
            hud.guidanceText.transform.SetSiblingIndex(backing.GetSiblingIndex() + 1);

            hud.pausePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(940, 710);
            hud.pauseTitle.rectTransform.anchoredPosition = new Vector2(0, 306);
            var intro = hud.pausePanel.transform.Find("Pause help").GetComponent<Text>();
            intro.text = "Scoop peppers, use the processor, then hand off finished food.\nThe yard stays paused until you resume.";
            intro.rectTransform.anchoredPosition = new Vector2(0, 257);
            intro.rectTransform.sizeDelta = new Vector2(900, 52);
            intro.fontSize = 19;
            Position(hud.resumeButton, 63);
            Position(hud.resetButton, -3);
            Position(hud.restartPrototypeButton, -69);
            Position(hud.quitButton, -284);
            hud.sensitivityText = Text("Sensitivity value", hud.pausePanel.transform, "Mouse sensitivity  1.00x", new Vector2(0, -125), new Vector2(700, 30), 21);
            Text("Sensitivity help", hud.pausePanel.transform, "Drag or use Left / Right. Default 1.00x; lasts until you close this test.",
                new Vector2(0, -219), new Vector2(880, 42), 18);
            var sliderObject = new GameObject("Mouse sensitivity", typeof(RectTransform), typeof(Slider));
            sliderObject.transform.SetParent(hud.pausePanel.transform, false);
            var rect = sliderObject.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, -170);
            rect.sizeDelta = new Vector2(560, 42);
            Image("Track", sliderObject.transform, new Vector2(560, 12), new Color(.3f, .39f, .32f));
            var thumb = Image("Handle", sliderObject.transform, new Vector2(24, 38), new Color(.95f, .79f, .43f));
            var slider = sliderObject.GetComponent<Slider>();
            hud.sensitivitySlider = slider;
            slider.targetGraphic = thumb;
            slider.handleRect = thumb.rectTransform;
            slider.direction = Slider.Direction.LeftToRight;
            slider.minValue = .25f;
            slider.maxValue = 2.5f;
            slider.SetValueWithoutNotify(1);
            var colors = slider.colors;
            colors.selectedColor = Color.white;
            colors.highlightedColor = Color.white;
            slider.colors = colors;
            var navigation = new Selectable[] { hud.resumeButton, hud.resetButton, hud.restartPrototypeButton, slider, hud.quitButton };
            for (int i = 0; i < navigation.Length; i++)
                navigation[i].navigation = new Navigation { mode = Navigation.Mode.Explicit,
                    selectOnUp = navigation[(i + navigation.Length - 1) % navigation.Length],
                    selectOnDown = navigation[(i + 1) % navigation.Length] };
            hud.pausePanel.transform.SetAsLastSibling();
            PolishMenu(hud);
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Could not save comfort wiring.");
            AssetDatabase.SaveAssets();
            Debug.Log("FIRST_PLAYABLE_COMFORT_AUTHORED " + ScenePath);
        }

        [MenuItem("Just a few peppers/Tune first playable comfort presentation")]
        public static void TunePresentation()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var session = UnityEngine.Object.FindAnyObjectByType<YardSession>();
            if (session == null || session.hud.sensitivitySlider == null)
                throw new InvalidOperationException("Apply comfort authoring first.");
            PolishMenu(session.hud);
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Could not save comfort presentation.");
            Debug.Log("FIRST_PLAYABLE_COMFORT_PRESENTATION_TUNED " + ScenePath);
        }

        static void PolishMenu(YardHud hud)
        {
            hud.guidanceBackdrop = hud.transform.Find("Controls backing").gameObject;
            hud.pausePanel.GetComponent<Image>().color = new Color(.055f, .10f, .085f, 1);
            // Slider stretches its handle anchors vertically to the 42-pixel track area.
            hud.sensitivitySlider.handleRect.sizeDelta = new Vector2(24, -4);
        }

        static void Position(Button button, float y)
        {
            var rect = button.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, y);
            rect.sizeDelta = new Vector2(630, 54);
            button.GetComponentInChildren<Text>().rectTransform.sizeDelta = new Vector2(620, 52);
        }

        static Image Image(string name, Transform parent, Vector2 size, Color color)
        {
            var go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            var image = go.GetComponent<Image>();
            image.rectTransform.sizeDelta = size;
            image.color = color;
            return image;
        }
    }
}
