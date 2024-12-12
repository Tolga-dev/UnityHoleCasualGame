using Brige_Race_Quiz.Scripts.So;
using UnityEngine;

namespace Brige_Race_Quiz.Scripts.Managers
{
    public class FxManager : MonoBehaviour
    {
        public GameSo gameSo;

        public void CreateEffects(string effectName = "", Transform spawnPoint = null)
        {
            if(effectName == "" || spawnPoint == null)
                return;
                
            var currentFx = gameSo.GetFx(effectName);
            var rotation = Quaternion.Euler(90, 0, 0); // Add 90 degrees to the X-axis
            var created = Instantiate(currentFx, spawnPoint.transform.position, rotation);
            var component = created.GetComponent<ParticleSystem>();
            component.Play();
            Destroy(created, component.main.duration);
        }
        
    }
}