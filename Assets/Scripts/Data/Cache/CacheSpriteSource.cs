using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utils;


namespace Data.Cache
{
    public class CacheSpriteSource : MonoSingleton<CacheSpriteSource>, ICacheSource<Sprite>
    {
        #region Private constants
        
        private const string SrcPathRoot = "Image";
        private const string SrcPathButton = "/Button";
        private const string SrcPathSkillIcon = "/Skill_Icon";

        private const string SrcPathSoundOn = "/SoundOn";
        private const string SrcPathSoundOff = "/SoundOff";
        private const string SrcPath = "/";

        #endregion


        #region Private variables

        private Dictionary<int, Sprite> _spriteCache;

        #endregion
        
        
        #region Public methods

        public IEnumerator Init()
        {
            _spriteCache = new Dictionary<int, Sprite>
            {
                { 0, Resources.Load<Sprite>($"{SrcPathRoot}{SrcPathButton}{SrcPathSoundOn}")  },
                { 1, Resources.Load<Sprite>($"{SrcPathRoot}{SrcPathButton}{SrcPathSoundOn}")  },
                { 2, Resources.Load<Sprite>($"{SrcPathRoot}{SrcPathButton}{SrcPathSoundOn}")  }
            };
            
            yield return null;
        }

        public Sprite GetSource(int id)
        {
            return null;
        }

        #endregion
    }
}