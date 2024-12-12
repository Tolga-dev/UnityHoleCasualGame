using UnityEngine;

namespace Core
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static object _lock = new object();
        private static bool _applicationIsQuitting = false;
 
        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debugger.LogWarning("[Singleton] Instance '" + typeof(T) +
                                     "' already destroyed on application quit." +
                                     " Won't create again - returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debugger.Log("There are already two " + typeof(T));
                            return _instance;
                        }
                        if (_instance == null)
                        {
                            var singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof(T);

                            DontDestroyOnLoad(singleton);

                            Debugger.Log("[Singleton] An instance of " + typeof(T) +
                                      " is needed in the scene, so '" + singleton +
                                      "' was created with DontDestroyOnLoad.");
                        }
                        else
                        {
                            Debugger.Log("[Singleton] Using instance already created: " +
                                         _instance.gameObject.name);
                        }
                    }

                    return _instance;
                }
            }
        }
        public void Awake()
        {
            _applicationIsQuitting = false;

            if (_instance == null)
                _instance = Instance;

        }

        public void OnDestroy()
        {
            _applicationIsQuitting = true;
        }
    }
}