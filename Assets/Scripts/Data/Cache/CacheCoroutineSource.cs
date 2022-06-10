using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Cache
{
    public class CacheCoroutineSource : AbstractCacheSource<CacheCoroutineSource, float, WaitForSeconds>
    {
        #region Public methods

        public override IEnumerator InitCoroutine()
        {
            IsInitialized = true;
            Cache = new Dictionary<float, WaitForSeconds>();
            yield break;
        }

        public new WaitForSeconds GetSource(float id)
        {
            WaitForSeconds wls;
            if (!Cache.TryGetValue(id, out wls))
            {
                wls = new WaitForSeconds(id);
                Cache.Add(id, wls);
            }

            return wls;
        }

        #endregion
    }
}