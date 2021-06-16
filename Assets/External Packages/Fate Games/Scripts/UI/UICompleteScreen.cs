using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace FateGames
{
    public class UICompleteScreen : UIElement
    {
        private RaceLevel levelManager = null;
        [HideInInspector] public bool Success = false;
        [SerializeField] private TextMeshProUGUI[] completeTexts = null;
        [SerializeField] private Text continueText = null;
        [SerializeField] private Label[] labels = null;
        [SerializeField] private Color playerColor;

        private void Awake()
        {
            levelManager = (RaceLevel)LevelManager.Instance;
        }
        public void SetScreen(bool success, int level)
        {
            foreach (TextMeshProUGUI completeText in completeTexts)
                completeText.text = GameManager.Instance.LevelName + " " + level + (success ? " WON!" : " LOST!");
            for (int i = 0; i < levelManager.Competitors.Length; i++)
            {
                Competitor competitor = levelManager.Competitors[i];
                Label label = labels[competitor.Rank - 1];
                if (levelManager.Competitors[i].transform == levelManager.Player.transform)
                    label.image.color = playerColor;
                label.text.text = competitor.Name;
            }
            continueText.text = success ? "CONTINUE" : "TRY AGAIN";
        }

        protected override void Animate()
        {
            return;
        }

        // Called by ContinueButton onClick
        public void Continue()
        {
            GameManager.Instance.LoadCurrentLevel();
        }

        [System.Serializable]
        private class Label
        {
            public Image image;
            public TextMeshProUGUI text;
        }
    }
}