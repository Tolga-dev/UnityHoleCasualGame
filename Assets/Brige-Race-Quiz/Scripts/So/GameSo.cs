using System;
using System.Collections.Generic;
using UnityEngine;

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
    }
    
    [Serializable]
    public class Level
    {
        public List<ObjectPrefab> objectPrefabs = new List<ObjectPrefab>();
        public List<Vector3> objectTransforms = new List<Vector3>();
    }

    [Serializable]
    public class ObjectPrefab
    {
        public GameObject prefab;
        public ObjectType objectType;
    }
}