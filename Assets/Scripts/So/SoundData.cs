using UnityEngine;

namespace So
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "So/SoundData", order = 0)]
    public class SoundData : ScriptableObject
    {
        public AudioClip timeClip;
    }
}