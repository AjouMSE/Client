using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utils;


namespace Data.Cache
{
    public class CacheSpriteSource : AbstractCacheSource<CacheSpriteSource, int, Sprite>
    {
        #region Private constants

        private const string SrcPathRoot = "Image";
        private const string SrcPathButtonIcon = "Button_Icon";
        private const string SrcPathLoadingIcon = "Loading_Icon";
        private const string SrcPathSkillIcon = "Skill_Icon";
        private const string SrcPathSpecialIcon = "Special_Icon";

        private const string SrcPathSoundOn = "Button_SoundOn";
        private const string SrcPathSoundOff = "Button_SoundOff";
        private const string SrcPathButtonBig = "Button_Big";
        private const string SrcPathButtonBigPressed = "Button_Big_Pressed";

        private const string SrcPathLoadingIconFire = "LoadingFire";
        private const string SrcPathLoadingIconIce = "LoadingIce";
        private const string SrcPathLoadingIconEarth = "LoadingEarth";
        private const string SrcPathLoadingIconLightning = "LoadingLightning";
        private const string SrcPathLoadingIconNature = "LoadingNature";

        #endregion


        #region Public methods

        public override IEnumerator InitCoroutine()
        {
            Cache = new Dictionary<int, Sprite>
            {
                // Button Icons (0 ~ 99)
                { 0, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathButtonIcon}/{SrcPathSoundOn}") },
                { 1, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathButtonIcon}/{SrcPathSoundOff}") },
                { 2, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathButtonIcon}/{SrcPathButtonBig}") },
                { 3, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathButtonIcon}/{SrcPathButtonBigPressed}") },

                // Loading Icons (100 ~ 200)
                { 100, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathLoadingIcon}/{SrcPathLoadingIconFire}") },
                { 101, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathLoadingIcon}/{SrcPathLoadingIconIce}") },
                { 102, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathLoadingIcon}/{SrcPathLoadingIconEarth}") },
                { 103, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathLoadingIcon}/{SrcPathLoadingIconLightning}") },
                { 104, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathLoadingIcon}/{SrcPathLoadingIconNature}") }
            };
            
            foreach (int key in TableDatas.Instance.cardDatas.Keys)
            {
                // Add skill icon
                var card = TableDatas.Instance.cardDatas[key];
                Cache.Add(key, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathSkillIcon}/{card.icon}"));

                // Add special icon
                if (card.special == 0) continue;
                if (Cache.ContainsKey(card.special)) continue;
                var special = TableDatas.Instance.GetSpecialData(card.special);
                Cache.Add(card.special, Resources.Load<Sprite>($"{SrcPathRoot}/{SrcPathSpecialIcon}/{special.icon}"));
            }

            IsInitialized = true;
            yield break;
        }

        public bool IsInit()
        {
            return IsInitialized;
        }

        #endregion
    }
}