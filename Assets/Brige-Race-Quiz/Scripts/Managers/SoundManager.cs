using System;
using System.Collections;
using Brige_Race_Quiz.Scripts.So;
using Controllers;
using Core;
using Managers;
using So;
using UnityEngine;

namespace Brige_Race_Quiz.Scripts
{
    public class SoundManager : Singleton<SoundManager>
    {   
        public AudioSource mainGameSound;
        public GameObject gameEffectSoundPlayer;
        public GameSo gameSo;
        
        public void PlayMainGameSound(string newClipName, float fadeDuration = 1.0f)
        {
            var newClip = gameSo.GetSound(newClipName);
            StartCoroutine(FadeToNewMainGameSound(newClip, fadeDuration));
        }

        private IEnumerator FadeToNewMainGameSound(AudioClip newClip, float fadeDuration)
        {
            if (mainGameSound.isPlaying)
            {
                // Fade out the current sound.
                for (float t = 0; t < fadeDuration; t += Time.deltaTime)
                {
                    mainGameSound.volume = Mathf.Lerp(1.0f, 0.0f, t / fadeDuration);
                    yield return null;
                }
                mainGameSound.Stop();
            }

            // Set the new clip and fade it in.
            mainGameSound.clip = newClip;
            mainGameSound.Play();
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                mainGameSound.volume = Mathf.Lerp(0.0f, 1.0f, t / fadeDuration);
                yield return null;
            }
            mainGameSound.volume = 1.0f; // Ensure volume is fully restored.
        }

        public void RunASound(string soundName)
        {
            if (gameSo.gameEffectSound == false)
                return;
            
            var sound = gameSo.GetSound(soundName);
            var createdSound = Instantiate(gameEffectSoundPlayer);
            var source = createdSound.GetComponent<AudioSource>();
            
            if (sound != null && source != null)
            {
                source.PlayOneShot(sound);
                Destroy(createdSound, sound.length);
            }
            else
            {
                Debug.LogWarning($"Sound {soundName} not found or gameEffectSound is not assigned.");
            }
            
        }

    }
}