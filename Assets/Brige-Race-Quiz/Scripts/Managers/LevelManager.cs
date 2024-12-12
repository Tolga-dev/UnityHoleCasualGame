using System.Collections.Generic;
using Brige_Race_Quiz.OutPacks.Timers.Scripts.Timers;
using Brige_Race_Quiz.Scripts.Managers;
using Brige_Race_Quiz.Scripts.Objects;
using Brige_Race_Quiz.Scripts.Player;
using Brige_Race_Quiz.Scripts.So;
using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Brige_Race_Quiz.Scripts
{
	public class LevelManager : Singleton<LevelManager>
	{
		[SerializeField] Timer timer1;
		public HoleMovement holeMovement;
		public FxManager fxManager;
		public GameSo gameSo;
		public Button gameSceneSaver;
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
			gameSceneSaver.onClick.AddListener(SaveGameSceneByUsingButton);
			RestartLevel();
		}

		// new world generation
		private void GenerateCurrentLevel()
		{
			DestroyCreatedObjects();
			
			var currentLevel = gameSo.GetCurrentLevel();
			var objectInGames = currentLevel.objectsInGame;
			var amount = objectInGames.Count;
			var createdAmountOfPointObject = 0;
			
			for (int i = 0; i < amount; i++)
			{
				var objectInGame = objectInGames[i];
				var createdPrefab = Instantiate(objectInGame.objectPrefab.prefab, objectInGame.objectTransform, Quaternion.identity,objectsParent);
				createdPrefab.transform.localScale = objectInGame.objectScale;
				createdPrefabs.Add(createdPrefab);
				if (IsItPoint(objectInGame.objectPrefab.objectType))
					createdAmountOfPointObject++;
			}
			
			totalObjects = objectsInScene = createdAmountOfPointObject;
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
			holeMovement.ResetPlayer();
			UIManager.Instance.inGameUI.ResetGame();
			Magnet.Instance.RestartMagnet();
			Game.StartGame();
			RestartTime();
		}

		private void RestartTime()
		{
			timer1.SetDuration(gameSo.GetCurrentLevel().timeAmount)
				.OnEnd(RestartLevel)
				.Begin();
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

		public void SaveGameSceneByUsingButton()
		{
			var objectsInGame = new List<ObjectInGame>();
			
			for (int i = 0; i < objectsParent.childCount; i++)
			{
				var currentChild = objectsParent.GetChild(i).gameObject;
				var levelSaveObjectBase = currentChild.GetComponent<LevelSaveObjectBase>();
				var currentPrefab = gameSo.GetCurrentPrefabFromType(levelSaveObjectBase.objectType);
				
				objectsInGame.Add(
					new ObjectInGame() 
						{ objectTransform = currentChild.transform.position, objectScale = currentChild.transform.localScale, 
							
							objectPrefab = new ObjectPrefab()
								{prefab = currentPrefab,objectType = levelSaveObjectBase.objectType}
						});
			}
			
			var objectInGame = gameSo.GetCurrentLevel().objectsInGame;
			objectInGame.Clear();
			objectInGame.AddRange(objectsInGame);
		}

		void OnValidate()
		{
			UpdateLevelColors ();
		}
		
		private bool IsItPoint(ObjectType objectPrefabObjectType) => 
			objectPrefabObjectType is ObjectType.SquarePointObject or ObjectType.CirclePointObject;
	}
}
