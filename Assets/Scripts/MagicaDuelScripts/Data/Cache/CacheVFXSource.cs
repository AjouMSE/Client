using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;
using Utils;


namespace Cache
{
    public class CacheVFXSource : AbstractCacheSource<CacheVFXSource, int, ParticleSystem>
    {
        #region Private constants

        private const string SrcPathRoot = "VFX";

        #endregion


        #region Public methods

        public override IEnumerator InitCoroutine()
        {
            Cache = new Dictionary<int, ParticleSystem>();

            foreach (int key in TableDatas.Instance.cardDatas.Keys)
            {
                var card = TableDatas.Instance.cardDatas[key];
                if (string.IsNullOrEmpty(card.effect)) continue;

                var obj = Object.Instantiate(Resources.Load<GameObject>($"{SrcPathRoot}/{card.effect}"));
                obj.SetActive(false);
                Cache.Add(key, obj.GetComponent<ParticleSystem>());
                Object.DontDestroyOnLoad(obj);
            }

            IsInitialized = true;
            yield break;
        }

        #endregion
    }
}