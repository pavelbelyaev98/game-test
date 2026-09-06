using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JustAFewPeppers
{
    public sealed class YardHud : MonoBehaviour
    {
        public GameObject pausePanel;
        public GameObject reticle;
        public Text targetText;
        public Text pauseTitle;
        public Text noticeText;
        public Button resumeButton;
        public Button resetButton;
        public Button quitButton;
        public Button restartPrototypeButton;
        public EventSystem events;
        float noticeUntil;

        public void Bind(YardSession session)
        {
            resumeButton.onClick.AddListener(session.Resume);
            resetButton.onClick.AddListener(session.ResetToSpawn);
            quitButton.onClick.AddListener(session.Quit);
            if (restartPrototypeButton != null) restartPrototypeButton.onClick.AddListener(session.RestartPrototype);
        }

        public void ShowPause(bool paused, string title)
        {
            pauseTitle.text = title;
            pausePanel.SetActive(paused);
            reticle.SetActive(!paused);
            targetText.gameObject.SetActive(!paused);
            events.SetSelectedGameObject(null);
            if (paused) events.SetSelectedGameObject(resumeButton.gameObject);
        }

        public void ShowTarget(YardTarget target)
        {
            targetText.text = target == null ? "" : target.displayName + "\n" + target.description;
        }

        public void Notice(string text)
        {
            noticeText.text = text;
            noticeUntil = Time.unscaledTime + 3;
        }

        void Update()
        {
            if (Time.unscaledTime > noticeUntil) noticeText.text = "";
        }
    }
}
