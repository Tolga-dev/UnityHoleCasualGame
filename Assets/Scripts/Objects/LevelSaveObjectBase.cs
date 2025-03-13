using UnityEngine;

public enum ObjectType
{
    SquarePointObject,
    CirclePointObject,
    
    SquareObstacleType,
    CircleObstacleType
}
namespace Brige_Race_Quiz.Scripts.Objects
{
    public class LevelSaveObjectBase : MonoBehaviour
    {
        public ObjectType objectType;
    }
}