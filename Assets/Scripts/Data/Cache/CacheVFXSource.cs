using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utils;


namespace Data.Cache
{
    public class CacheVFXSource : CacheSource<CacheVFXSource, ParticleSystem>
    {
        #region Private constants

        private const string SrcPathRoot = "VFX";

        #endregion


        #region Public methods

        public override IEnumerator Init()
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

            yield break;
        }

        #endregion
    }
}