using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utils;


namespace Data.Cache
{
    public class CacheSpriteSource : MonoSingleton<CacheSpriteSource>, ICacheSource<Sprite>
    {


        #region Public methods

        public IEnumerator Init()
        {
            yield return null;
        }

        public Sprite GetSource(int id)
        {
            return null;
        }

        #endregion
    }
}
