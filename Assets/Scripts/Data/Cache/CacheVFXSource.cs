using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utils;


namespace Data.Cache
{
    public class CacheVFXSource : MonoSingleton<CacheVFXSource>, ICacheSource<GameObject>
    {
        #region Public methods
        
        public IEnumerator Init()
        {
            yield return null;
        }

        public GameObject GetSource(int id)
        {
            return null;
        }

        #endregion
    }
}