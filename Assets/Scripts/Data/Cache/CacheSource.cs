using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Utils;

namespace Data.Cache
{
    public abstract class CacheSource<TChild, TId, TSource>
        where TChild : new()
        where TId : unmanaged
    {
        #region Private static variables

        private static TChild _instance;
        private static readonly object _lockObject = new object();

        #endregion


        #region Protected variables

        // cache dictionary
        protected Dictionary<TId, TSource> Cache;

        #endregion


        #region Public variables

        public static bool IsDestroyed { get; protected set; }
        public static bool IsInitialized { get; protected set; }

        public static TChild Instance
        {
            get
            {
                if (IsDestroyed)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Instance of ");
                    sb.Append(typeof(TChild));
                    sb.Append("already has been destroyed");
                    return default;
                }

                lock (_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new TChild();
                    }
                }

                return _instance;
            }
        }

        #endregion


        #region Public methods

        /// <summary>
        /// abstract Init coroutine
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator InitCoroutine();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TSource GetSource(TId id)
        {
            TSource source = default;

            try
            {
                source = Cache[id];
            }
            catch (KeyNotFoundException e)
            {
                Debug.LogError($"Cannot find key value of {id.ToString()} {e.Message}");
            }

            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Destroy()
        {
            Cache = null;
            IsDestroyed = true;
        }

        #endregion
    }
}