﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Brige_Race_Quiz.Scripts;
using DG.Tweening;
using So;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Brige_Race_Quiz.OutPacks.Timers.Scripts.Timers
{
    public class Timer : MonoBehaviour
    {
        public float shakeDuration = 0.5f;
        public float shakeStrength = 20f;
        public int vibrato = 10;
        public float sizeMultiplier = 2f; // Scale multiplier for doubling the size

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
        
        
        private CancellationTokenSource _cancellationTokenSource;
        public void ResetTimer()
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
            StopTimer();
            _onTimerBeginAction?.Invoke();
            _ = UpdateTimerAsync();
        }

        private async Task UpdateTimerAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _cancellationTokenSource.Token;

            while (_remainingDuration > 0)
            {
                if (token.IsCancellationRequested)
                {
                    Debug.Log("Timer canceled.");
                    break;
                }

                if (!IsPaused)
                {
                    _onTimerChangeAction?.Invoke(_remainingDuration);
                    UpdateUI(_remainingDuration);
                    _remainingDuration--;
                    audioSource?.PlayOneShot(soundData.timeClip);
                }

                try
                {
                    await Task.Delay(1000, token); // Use the cancellation token here
                }
                catch (TaskCanceledException)
                {
                    Debug.Log("Task delay canceled.");
                    break;
                }
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
            audioSource?.Stop();
            // Move to the middle of the screen
            var screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
            var originalPos = transform.position;
            var originalScale = transform.localScale;

            if (_remainingDuration > 0)
                return;
            
            Game.StopGame();
            transform.DOMove(screenCenter, 0.5f) // Move to the middle of the screen
                .OnComplete(() =>
                {
                    transform.DOScale(originalScale * sizeMultiplier, 0.3f) // Double the size
                        .OnComplete(() =>
                        {
                            transform.DOShakePosition(shakeDuration, shakeStrength, vibrato) // Shake the position
                                .OnComplete(() =>
                                {
                                    // Return to the original position and scale
                                    transform.DOMove(originalPos, 0.5f);
                                    transform.DOScale(originalScale, 0.3f).OnComplete(() =>
                                    {
                                        if (_remainingDuration <= 0)
                                        {
                                            _onTimerEndAction?.Invoke(); // Call the timer end action
                                        }
                                    });
                                });
                        });
                });
        }
        
        public void StopTimer()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }
    }
}
