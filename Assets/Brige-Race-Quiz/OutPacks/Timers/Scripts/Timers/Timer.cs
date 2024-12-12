using System;
using System.Threading.Tasks;
using So;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Brige_Race_Quiz.OutPacks.Timers.Scripts.Timers
{
    public class Timer : MonoBehaviour
    {
        public SoundData soundData;
        
        [Header("Timer UI references:")]
        [SerializeField] private Image uiFillImage;
        [SerializeField] private Text uiText;
        
        [Header("Sound")]
        public AudioSource audioSource;

        private int Duration { get; set; }
        private bool IsPaused { get; set; }

        private int _remainingDuration;

        private UnityAction _onTimerBeginAction;
        private UnityAction<int> _onTimerChangeAction;
        private UnityAction _onTimerEndAction;
        private UnityAction<bool> _onTimerPauseAction;
        
        private void ResetTimer()
        {
            uiText.text = "00:00";
            uiFillImage.fillAmount = 0f;

            Duration = _remainingDuration = 0;

            _onTimerBeginAction = null;
            _onTimerChangeAction = null;
            _onTimerEndAction = null;
            _onTimerPauseAction = null;

            IsPaused = false;
        }

        public void SetPaused(bool paused)
        {
            IsPaused = paused;
            _onTimerPauseAction?.Invoke(IsPaused);
        }

        public Timer SetDuration(int seconds)
        {
            Duration = _remainingDuration = seconds;
            return this;
        }

        public Timer OnBegin(UnityAction action) => WithAction(ref _onTimerBeginAction, action);
        public Timer OnChange(UnityAction<int> action) => WithAction(ref _onTimerChangeAction, action);
        public Timer OnEnd(UnityAction action) => WithAction(ref _onTimerEndAction, action);
        public Timer OnPause(UnityAction<bool> action) => WithAction(ref _onTimerPauseAction, action);

        private Timer WithAction<T>(ref T actionField, T action) where T : Delegate
        {
            actionField = action;
            return this;
        }

        public void Begin()
        {
            _onTimerBeginAction?.Invoke();
            StopAllCoroutines();
            _ = UpdateTimerAsync();
        }

        private async Task UpdateTimerAsync()
        {
            while (_remainingDuration > 0)
            {
                if (!IsPaused)
                {
                    _onTimerChangeAction?.Invoke(_remainingDuration);
                    UpdateUI(_remainingDuration);
                    _remainingDuration--;
                    audioSource?.PlayOneShot(soundData.timeClip);
                }
                await Task.Delay(1000); // Normal 1 second delay for the first third
            }
            End();
        }

        private void UpdateUI(int seconds)
        {
            uiText.text = $"{seconds / 60:D2}:{seconds % 60:D2}";
            uiFillImage.fillAmount = Mathf.InverseLerp(0, Duration, seconds);
        }

        public void End()
        {
            _onTimerEndAction?.Invoke();
            audioSource?.Stop();
            ResetTimer();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
