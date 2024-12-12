using System;
using UnityEngine;

namespace Brige_Race_Quiz.Scripts.Player
{
    [Serializable]
    public class InputController
    {
        public Vector3 getInputVector = Vector3.zero;
        public void Update()
        {
#if UNITY_EDITOR 
            Game.isMoving = GetMouseButton(0);
#else
		    Game.isMoving = Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved;
#endif
        }

        private static bool GetMouseButton(int i)
        {
            return Input.GetMouseButton(i);
        }

        public static float GetAxis(string mouseX)
        {
            return Input.GetAxis(mouseX);
        }

        public Vector3 InputVector
        {
            get
            {
                getInputVector.x = GetAxis ("Mouse X");
                getInputVector.y = GetAxis ("Mouse Y");
                return getInputVector;
            }
        }
    }
}