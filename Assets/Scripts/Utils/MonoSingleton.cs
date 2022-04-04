using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Private static variables
        private static T _instance;
        private static object _lockObject = new object();
        private static bool onDestroyed = false;

        // Public static instance variables
        public static T Instance
        {
            get
            {
                if (onDestroyed)
                {
                    Debug.Log("Instance of " + typeof(T) + "already destroyed on application quit.");
                    return null;
                }

                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        // Find a singleton instance
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = "(s)" + typeof(T).ToString();
                            Debug.Log("Singleton GameObject " + _instance.gameObject.name + " is created in " 
                                      + SceneManager.GetActiveScene().name);
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
