using Brige_Race_Quiz.OutPacks.Timers.Scripts.Timers;
using UnityEngine;

namespace Brige_Race_Quiz.OutPacks.Timers.Scripts
{
   public class TimeManager : MonoBehaviour {

      [SerializeField] Timer timer1 ;
      [SerializeField] Timer timer2 ;
      [SerializeField] Timer timer3 ;
      [SerializeField] Timer timer4 ;


      private void Start()
      {
         // Timer 1: 6 seconds duration
         timer1.SetDuration(6)
            .OnBegin(() => Debug.Log("Timer 1 Started"))
            .OnEnd(() => Debug.Log("Timer 1 Ended"))
            .OnChange((remainingTime) => Debug.Log($"Timer 1: {remainingTime} seconds remaining"))
            .Begin();

         // Timer 2: 10 seconds duration
         timer2.SetDuration(10)
            .OnBegin(() => Debug.Log("Timer 2 Started"))
            .OnEnd(() => Debug.Log("Timer 2 Ended"))
            .OnChange((remainingTime) => Debug.Log($"Timer 2: {remainingTime} seconds remaining"))
            .Begin();

         // Timer 3: 5 seconds duration (with sound effect every second)
         timer3.SetDuration(5)
            .OnBegin(() => Debug.Log("Timer 3 Started"))
            .OnEnd(() => Debug.Log("Timer 3 Ended"))
            .OnChange((remainingTime) => Debug.Log($"Timer 3: {remainingTime} seconds remaining"))
            .Begin();

         // Timer 4: 15 seconds duration
         timer4.SetDuration(15)
            .OnBegin(() => Debug.Log("Timer 4 Started"))
            .OnEnd(() => Debug.Log("Timer 4 Ended"))
            .OnChange((remainingTime) => Debug.Log($"Timer 4: {remainingTime} seconds remaining"))
            .Begin();
      }
   }
}
