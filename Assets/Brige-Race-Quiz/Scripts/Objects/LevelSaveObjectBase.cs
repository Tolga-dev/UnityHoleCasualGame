using UnityEngine;

public enum ObjectType
{
    PointObject,
    ObjectObject
}
namespace Brige_Race_Quiz.Scripts.Objects
{
    public class LevelSaveObjectBase : MonoBehaviour
    {
        public ObjectType objectType;
    }
}