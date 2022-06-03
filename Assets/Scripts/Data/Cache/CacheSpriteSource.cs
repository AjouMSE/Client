using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utils;


namespace Data.Cache
{
    public class CacheSpriteSource : CacheSource<CacheSpriteSource, int, Sprite>
    {
        #region Private constants

        private const string SrcPathRoot = "Image";
        private const string SrcPathButton = "Button";
        private const string SrcPathSkillIcon = "Skill_Icon";

        private const string SrcPathSoundOn = "SoundOn";
        private const string SrcPathSoundOff = "SoundOff";

        #endregion


        #region Public methods

        public override IEnumerator InitCoroutine()
        {
            Cache = new Dictionary<int, Sprite>
            {
                { 0, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathButton}/{SrcPathSoundOn}") },
                { 1, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathButton}/{SrcPathSoundOff}") }
            };

            foreach (int key in TableDatas.Instance.cardDatas.Keys)
            {
                CardData card = TableDatas.Instance.cardDatas[key];
                Cache.Add(key, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathSkillIcon}/{card.icon}"));
            }

            IsInitialized = true;
            yield break;
        }

        #endregion
    }
}