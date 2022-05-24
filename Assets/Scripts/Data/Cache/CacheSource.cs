using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Data.Cache
{
    public abstract class CacheSource<TChild, TSource> where TChild : new()
    {
        public static TChild Instance
        {
            get
            {
                if (Instance == null)
                {
                    Instance = new TChild();
                }

                return Instance;
            }
            private set { }
        }

        // cache dictionary
        protected Dictionary<int, TSource> Cache;

        // abstract method
        public abstract IEnumerator Init();

        public TSource GetSource(int id)
        {
            TSource source = default;

            try
            {
                source = Cache[id];
            }
            catch (KeyNotFoundException e)
            {
                Debug.LogError($"Cannot find key value of {id.ToString()}");
            }

            return source;
        }
    }
}