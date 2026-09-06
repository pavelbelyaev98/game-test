using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static JustAFewPeppers.Editor.FoundationSceneBuilder;

namespace JustAFewPeppers.Editor
{
    public static class QuietHelpAuthoring
    {
        public static void Apply()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath);
            var hud = UnityEngine.Object.FindAnyObjectByType<YardSession>().hud;
            if (hud.helpPanel != null) throw new InvalidOperationException("Quiet help already authored.");
            var input = InputActionAsset.FromJson(File.ReadAllText(InputPath));
            input.FindActionMap("System", true).AddAction("Help", InputActionType.Button, "<Keyboard>/f1");
            var toss = input.FindActionMap("Gameplay", true).AddAction("Toss", InputActionType.Button, "<Mouse>/rightButton");
            toss.wantsInitialStateCheck = true;
            File.WriteAllText(InputPath, input.ToJson());
            UnityEngine.Object.DestroyImmediate(input);
            AssetDatabase.ImportAsset(InputPath);

            hud.helpPanel = new GameObject("Optional controls reference", typeof(RectTransform), typeof(Image));
            hud.helpPanel.transform.SetParent(hud.transform, false);
            var help = hud.helpPanel.GetComponent<Image>();
            help.color = new Color(.055f, .10f, .085f, 1);
            help.rectTransform.sizeDelta = new Vector2(1120, 640);
            Text("Help title", help.transform, "Controls and test tools", new Vector2(0, 260), new Vector2(1000, 55), 32);
            hud.controlsText.transform.SetParent(help.transform, false);
            hud.controlsText.rectTransform.anchoredPosition = new Vector2(0, 90);
            hud.controlsText.rectTransform.sizeDelta = new Vector2(1040, 270);
            hud.controlsText.fontSize = 21;
            hud.controlsText.text = "WASD / Arrows  Move     Shift  Sprint     Space  Jump     Mouse  Look\n" +
                "E  Pick up / place gently / tip at intake / collect / hand off\n" +
                "Hold left mouse  Scoop (release to stop)\n" +
                "Z / X  Rotate     G  Drop     Right mouse  Toss a loose prop\n" +
                "Esc  Pause / resume     F1  Open / close this reference\n\n" +
                "Test tools: R  Return to gate / recover objects (keeps food)\n" +
                "F8  Restart food test (clears all food, keeps prop arrangements)";
            hud.guidanceText.transform.SetParent(help.transform, false);
            hud.guidanceText.rectTransform.anchoredPosition = new Vector2(0, -135);
            hud.guidanceText.rectTransform.sizeDelta = new Vector2(1030, 130);
            Text("Close reference", help.transform, "F1 / Esc  Back", new Vector2(0, -270), new Vector2(1000, 40), 21);
            hud.guidanceBackdrop.SetActive(false);
            hud.helpPanel.SetActive(false);
            hud.pausePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(720, 560);
            hud.pauseTitle.rectTransform.anchoredPosition = new Vector2(0, 220);
            hud.pausePanel.transform.Find("Pause help").gameObject.SetActive(false);
            hud.pausePanel.transform.Find("Sensitivity help").gameObject.SetActive(false);
            Position(hud.resumeButton.transform, 145);
            Position(hud.resetButton.transform, 79);
            hud.resetButton.GetComponentInChildren<Text>().text = "Return to gate / recover objects";
            Position(hud.restartPrototypeButton.transform, 13);
            Position(hud.sensitivityText.transform, -48);
            Position(hud.sensitivitySlider.transform, -88);
            Position(hud.quitButton.transform, -163);
            Text("Optional help shortcut", hud.pausePanel.transform, "F1  Controls and test tools", new Vector2(0, -235), new Vector2(650, 35), 19);
            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene)) throw new InvalidOperationException("Quiet help save failed.");
            AssetDatabase.SaveAssets();
            Debug.Log("QUIET_HELP_AUTHORED " + ScenePath);
        }

        static void Position(Transform target, float y) => target.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
    }
}
