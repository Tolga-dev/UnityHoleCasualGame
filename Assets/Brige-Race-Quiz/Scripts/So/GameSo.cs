using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Brige_Race_Quiz.Scripts.So
{
    [CreateAssetMenu(fileName = "GameSo", menuName = "So/GameSo", order = 0)]
    public class GameSo : ScriptableObject
    {
        public List<Level> levels = new List<Level>();
        public List<ObjectPrefab> objectPrefabs = new List<ObjectPrefab>();
        
        public int currentLevel;
        
        // levels
        public Level GetCurrentLevel()
        {
            return levels[currentLevel];
        }
        
        // sounds
        public bool gameEffectSound;
        public AudioClip GetSound(string newClipName)
        {
            return null;
        }
        
        // fx
        public GameObject GetFx(string effectName)
        {
            return null;
        }

        public GameObject GetCurrentPrefabFromType(ObjectType objectType)
        {
            foreach (var objectPrefab in objectPrefabs)
            {
                if (objectPrefab.objectType == objectType)
                    return objectPrefab.prefab;
            }
            return null;
        }
    }
    
    [Serializable]
    public class Level
    {
        public List<ObjectInGame> objectsInGame = new List<ObjectInGame>();
        public int timeAmount;
    }

    [Serializable]
    public class ObjectInGame
    {
        public ObjectPrefab objectPrefab;
        public Vector3 objectTransform;
        public Vector3 objectScale;
    }

    [Serializable]
    public class ObjectPrefab
    {
        public GameObject prefab;
        public ObjectType objectType;
    }
}