using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utils;


namespace Data.Cache
{
    public class CacheSpriteSource : CacheSource<CacheSpriteSource, Sprite>
    {
        #region Private constants

        private const string SrcPathRoot = "Image";
        private const string SrcPathButton = "Button";
        private const string SrcPathSkillIcon = "Skill_Icon";

        private const string SrcPathSoundOn = "SoundOn";
        private const string SrcPathSoundOff = "SoundOff";

        #endregion


        #region Public methods

        public override IEnumerator Init()
        {
            Cache = new Dictionary<int, Sprite>
            {
                { 0, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathButton}/{SrcPathSoundOn}") },
                { 1, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathButton}/{SrcPathSoundOff}") }
            };

            foreach (int key in TableDatas.Instance.cardDatas.Keys)
            {
                CardData card = TableDatas.Instance.cardDatas[key];
                Debug.Log($"{card.icon} / {card.text}");
                Cache.Add(key, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathSkillIcon}/{card.icon}"));
            }

            yield break;
        }

        #endregion
    }
}