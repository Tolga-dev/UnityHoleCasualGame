using System;
using Brige_Race_Quiz.Scripts.Managers;
using Brige_Race_Quiz.Scripts.So;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Brige_Race_Quiz.Scripts.UI
{
    [Serializable]
    public class InGameUI : UIBase
    {
        [Header ("Level Progress UI")]
        [SerializeField] TextMeshProUGUI nextLevelText;
        [SerializeField] TextMeshProUGUI currentLevelText;
        [SerializeField] Image progressFillImage;

        [Space]
        [SerializeField] TextMeshProUGUI levelCompletedText;

        [Space]
        [SerializeField] Image fadePanel;

        private SaveManager _saveManager;
        private SoundManager _soundManager;
        public override void Starter(UIManager uiManager)
        {
            base.Starter(uiManager);

            _saveManager = SaveManager.Instance;
            _soundManager = SoundManager.Instance;

            ResetGame();
        }
        
        // update level
        public void UpdateLevelProgress()
        {
            var val = 1f - ((float)LevelManager.Instance.objectsInScene / LevelManager.Instance.totalObjects);
            progressFillImage.DOFillAmount (val, .4f);
            Debug.Log("PlayPointSound");
        }
        
        // reset game
        public void ResetGame()
        {
            progressFillImage.fillAmount = 0f;
            FadeAtStart();
            SetLevelProgressText ();
            Debug.Log("PlayResetGameSound");
        }

        // set levels
        void SetLevelProgressText()
        {
            var level = gameSo.currentLevel;
            currentLevelText.text = level.ToString ();
            nextLevelText.text = (level + 1).ToString ();
        }

        // ui effect
        public void ShowLevelCompletedUI ()
        {
            levelCompletedText.DOFade (1f, .6f).From (0f);
        }
        
        // ui effect
        public void FadeAtStart ()
        {
            fadePanel.DOFade (0f, 1.3f).From (1f);
        }

        public GameSo gameSo => _saveManager.gameSo;
    }
}