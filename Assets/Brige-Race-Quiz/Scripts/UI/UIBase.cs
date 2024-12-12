using UnityEngine;

namespace Brige_Race_Quiz.Scripts.UI
{
    public class UIBase
    {
        protected UIManager UIManager;

        public virtual void Starter(UIManager uIManagerInGame)
        {
            UIManager = uIManagerInGame;
        }
    }
}