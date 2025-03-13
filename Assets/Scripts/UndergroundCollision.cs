using Brige_Race_Quiz.Scripts.Managers;
using Brige_Race_Quiz.Scripts.Player;
using DG.Tweening;
using UnityEngine;

namespace Brige_Race_Quiz.Scripts
{
	//Object or Obstacle is at the bottom of the Hole
	public class UndergroundCollision : MonoBehaviour
	{
		public HoleMovement holeMovement;
		void OnTriggerEnter (Collider other)
		{
			if (!Game.isGameover) {
				var s = other.tag;
				
				if (s.Equals ("Object"))
				{
					EatPointObject(other);
				}
				else if (s.Equals ("Obstacle"))
				{
					EatObstacle(other);
				}
			}
		}

		private void EatObstacle(Collider other)
		{
			Game.isGameover = true;
			Destroy (other.gameObject);
			CameraShake();
		}

		private void EatPointObject(Collider other)
		{
			LevelManager.Instance.objectsInScene--;
			UIManager.Instance.inGameUI.UpdateLevelProgress();
			Magnet.Instance.RemoveFromMagnetField (other.attachedRigidbody);
			
			Destroy (other.gameObject);

			if (LevelManager.Instance.objectsInScene == 0) {
				UIManager.Instance.inGameUI.ShowLevelCompletedUI();
				LevelManager.Instance.PlayWinFx();
				Invoke (nameof(NextLevel), 2f);
			}
		}

		private void CameraShake()
		{
			Camera.main.transform
				.DOShakePosition (1f, .2f, 20, 90f)
				.OnComplete (() => {
					//restart level after shaking complet
					LevelManager.Instance.RestartLevel ();
				});
		}
		void NextLevel ()
		{
			LevelManager.Instance.LoadNextLevel ();
		}
	}
}
