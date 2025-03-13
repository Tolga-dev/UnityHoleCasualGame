using Brige_Race_Quiz.Scripts.UI;
using Core;

namespace Brige_Race_Quiz.Scripts.Managers
{
	public class UIManager : Singleton<UIManager>
	{
		public InGameUI inGameUI;

		private void Start()
		{
			inGameUI.Starter(this);
		}
	}
}
