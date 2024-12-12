using System.Collections.Generic;
using Brige_Race_Quiz.Scripts.Managers;
using Brige_Race_Quiz.Scripts.So;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Brige_Race_Quiz.Scripts
{
	public class LevelManager : Singleton<LevelManager>
	{
		public FxManager fxManager;
		public GameSo gameSo;
		
		[Space]
		[HideInInspector] public int objectsInScene;
		[HideInInspector] public int totalObjects;

		[SerializeField] Transform objectsParent;

		[Space]
		[Header ("Materials & Sprites")]
		[SerializeField] Material groundMaterial;
		[SerializeField] Material objectMaterial;
		[SerializeField] Material obstacleMaterial;
		[SerializeField] SpriteRenderer groundBorderSprite;
		[SerializeField] SpriteRenderer groundSideSprite;
		[SerializeField] Image progressFillImage;
		[SerializeField] SpriteRenderer bgFadeSprite;

		[Space]
		[Header ("Level Colors-------")]
		[Header ("Ground")]
		[SerializeField] Color groundColor;
		[SerializeField] Color bordersColor;
		[SerializeField] Color sideColor;

		[Header ("Objects & Obstacles")]
		[SerializeField] Color objectColor;
		[SerializeField] Color obstacleColor;

		[Header ("UI (progress)")]
		[SerializeField] Color progressFillColor;

		[Header ("Background")]
		[SerializeField] Color cameraColor;
		[SerializeField] Color fadeColor;

		public List<GameObject> createdPrefabs = new List<GameObject>();
		void Start ()
		{
			GenerateCurrentLevel();
			UpdateLevelColors ();
		}

		// new world generation
		private void GenerateCurrentLevel()
		{
			DestroyCreatedObjects();
			
			var currentLevel = gameSo.GetCurrentLevel();
			var prefabs = currentLevel.objectPrefabs;
			var transforms = currentLevel.objectTransforms;
			var amount = prefabs.Count;
			
			for (int i = 0; i < amount; i++)
			{
				var createdPrefab = Instantiate(prefabs[i].prefab, transforms[i], Quaternion.identity,objectsParent);
				createdPrefab.transform.parent = objectsParent;
				createdPrefabs.Add(createdPrefab);
			}
			totalObjects = objectsInScene = amount;
		}

		// destroy world
		private void DestroyCreatedObjects()
		{
			foreach (var prefab in createdPrefabs)
			{
				Destroy(prefab);
			}
			createdPrefabs.Clear();
		}
		
		public void LoadNextLevel ()
		{
			gameSo.currentLevel++;
			RestartLevel();
		}

		public void RestartLevel()
		{
			GenerateCurrentLevel();
			UIManager.Instance.inGameUI.ResetGame();
			Magnet.Instance.RestartMagnet();
			Game.StartGame();
		}
		public void PlayWinFx ()
		{
			fxManager.CreateEffects("", transform);
		}

		void UpdateLevelColors ()
		{
			groundMaterial.color = groundColor;
			groundSideSprite.color = sideColor;
			groundBorderSprite.color = bordersColor;

			obstacleMaterial.color = obstacleColor;
			objectMaterial.color = objectColor;

			progressFillImage.color = progressFillColor;

			Camera.main.backgroundColor = cameraColor;
			bgFadeSprite.color = fadeColor;
		}

		void OnValidate()
		{
			UpdateLevelColors ();
		}
	}
}
