using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;


namespace Data.Cache
{
    public class CacheEmojiSource : AbstractCacheSource<CacheEmojiSource, CacheEmojiSource.EmojiType, ParticleSystem>
    {
        #region Private constants

        private const string SrcPathEmojiRoot = "Emoji";
        private const string SrcPathTableRoot = "Table";
        private const string SrcPathEmojiTable = "emoji_table";

        #endregion

        #region Public constants

        public enum EmojiType
        {
            EmojiAngry = 0,
            EmojiAngry2 = 1,
            EmojiCool = 2,
            EmojiCry = 3,
            EmojiCute = 4,
            EmojiDeadTired = 5,
            EmojiDisappointed = 6,
            EmojiHeart = 7,
            EmojiInjured = 8,
            EmojiMad = 9,
            EmojiPleading = 10,
            EmojiPoop = 11,
            EmojiQueasy = 12,
            EmojiSad = 13,
            Emojisick = 14,
            EmojiSinister = 15,
            EmojiSmile = 16,
            EmojiThumbsDown = 17,
            EmojiThumbsUp = 18,
            EmojiXD = 19,
            EmojiYawn = 20,
        }

        #endregion


        #region Private methods

        private ParticleSystem SpawnObj(string path)
        {
            var obj = Object.Instantiate(Resources.Load<GameObject>(path));
            obj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            obj.SetActive(false);
            Object.DontDestroyOnLoad(obj);
            return obj.GetComponent<ParticleSystem>();
        }

        #endregion


        #region Public methods

        public override IEnumerator InitCoroutine()
        {
            Cache = new Dictionary<EmojiType, ParticleSystem>();
            var res = Resources.Load<TextAsset>($"{SrcPathTableRoot}/{SrcPathEmojiTable}").text;
            var lines = res.Split(',');
            
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Split('-');
                var id = Convert.ToInt32(line[0]);
                var path = line[1];
                
                Cache.Add((EmojiType) id, SpawnObj($"{SrcPathEmojiRoot}/{path}"));
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