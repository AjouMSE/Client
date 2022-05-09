using UnityEngine;
using UnityEngine.SceneManagement;


namespace Utils
{
    /// <summary>
    /// MonoSingleton.cs
    /// Author: Lee Hong Jun (Arcane22, hong3883@naver.com)
    /// Version: 1.0
    /// Last Modified: 2022. 04. 04
    /// Description: Singleton class for Manager class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Private static variables
        private static T _instance;
        private static object _lockObject = new object();
        private static bool onDestroyed = false;
        protected static bool isCreated = false;

        // Public static instance variables
        public static T Instance
        {
            get
            {
                // Singleton object is destroyed.
                if (onDestroyed)
                {
                    Debug.Log("Instance of " + typeof(T) + " already destroyed.");
                    Debug.Log("Instance of " + typeof(T) + " is null");
                    return null;
                }

                // critical section
                lock (_lockObject)
                {
                    // If instance is null,
                    if (_instance == null)
                    {
                        // Find a singleton instance in current scene
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError(
                                "To many singleton instance exist in the scene. Reload the scene to fix it.");
                            return _instance;
                        }

                        // If instance is null,
                        if (_instance == null)
                        {
                            // Make new instance 
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = "(s)" + typeof(T);
                            isCreated = true;
                            Debug.Log("Singleton GameObject " + _instance.gameObject.name + " is created in "
                                      + SceneManager.GetActiveScene().name);

                            DontDestroyOnLoad(_instance);
                        }
                        else
                        {
                            Debug.Log("GameObject" + _instance.gameObject.name + " is already created.");
                        }
                    }

                    return _instance;
                }
            }
        }

        private void OnDestroy()
        {
            onDestroyed = true;
        }
    }
}