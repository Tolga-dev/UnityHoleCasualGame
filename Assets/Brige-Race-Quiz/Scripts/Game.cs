namespace Brige_Race_Quiz.Scripts
{
	public abstract class Game
	{
		public static bool isGameover = false;
		public static bool isMoving = false;

		public static void StartGame()
		{
			isMoving = false;
			isGameover = false;
		}
		public static void StopGame()
		{
			isMoving = false;
			isGameover = true;
		}
	}
}
