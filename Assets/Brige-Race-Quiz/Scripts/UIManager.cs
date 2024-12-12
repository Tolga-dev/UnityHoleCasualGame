using Brige_Race_Quiz.Scripts.UI;
using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



namespace Brige_Race_Quiz.Scripts
{
	public class UIManager : Singleton<UIManager>
	{
		public InGameUI inGameUI;

		void Start()
		{
			inGameUI.Starter(this);

		}
	}
}
