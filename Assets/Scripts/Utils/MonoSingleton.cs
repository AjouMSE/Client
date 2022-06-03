using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace Utils
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Private static variables

        private static T _instance;
        private static readonly object _lockObject = new object();

        #endregion

        #region Public Properties

        public static bool IsDestroyed { get; private set; }
        public static bool IsCreated { get; private set; }

        public static T Instance
        {
            get
            {
                if (IsDestroyed)
                {
                    if (IsCreated) IsCreated = false;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Instance of ");
                    sb.Append(typeof(T));
                    sb.Append("already has been destroyed");
                    return null;
                }

                // Critical section (thread-safe)
                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        // Find a singleton object in current scene
                        T[] instances = FindObjectsOfType<T>();

                        if (instances.Length == 0)
                        {
                            IsCreated = true;
                            _instance = new GameObject().AddComponent<T>();
                            _instance.gameObject.name = $"(s) {typeof(T)}";
                            DontDestroyOnLoad(_instance);
                        }
                        else
                        {
                            _instance = instances[0];
                            if (instances.Length > 1)
                            {
                                for (int i = 1; i < instances.Length; i++)
                                {
                                    Destroy(instances[i]);
                                }

                                Debug.LogWarning(
                                    "There is more than one instance in the scene. Other instances are destroyed.");
                            }
                        }
                    }

                    return _instance;
                }
            }
        }

        #endregion

        /// <summary>
        /// Abstract Init methods to initialize singleton
        /// </summary>
        public abstract void Init();
    }
}