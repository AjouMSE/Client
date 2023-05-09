using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Audio;
using Utils;


namespace Cache
{
    public class CacheAudioSource : AbstractCacheSource<CacheAudioSource, AudioManager.BgmTypes, AudioClip>
    {
        #region Private constants

        private const string SrcPathRoot = "Audio";
        private const string SrcPathBgm = "BGM";
        private const string SrcPathSfx = "SFX";
        private const string SrcPathMixer = "Mixer";

        private const string SrcPathLogoBgm = "LogoBgm"; // 0
        private const string SrcPathMainBgm1 = "MainBgm01"; // 1
        private const string SrcPathMainBgm2 = "MainBgm02"; // 2
        private const string SrcPathMainBgm3 = "MainBgm03"; // 3
        private const string SrcPathMainBgm4 = "MainBgm04"; // 4
        private const string SrcPathBattleBgm1 = "BattleBgm1"; // 5
        private const string SrcPathBattleBgm2 = "BattleBgm2"; // 6
        private const string SrcPathBattleBgm3 = "BattleBgm3"; // 7
        private const string SrcPathBattleBgm4 = "BattleBgm4"; // 8

        #endregion


        #region Public variables

        public AudioMixer AudioMixer { get; private set; }

        #endregion


        #region Public methods

        public override IEnumerator InitCoroutine()
        {
            // Init AudioMixer
            AudioMixer = Resources.Load<AudioMixer>($"{SrcPathRoot}/{SrcPathMixer}");

            // Cache bgm to dictionary
            Cache = new Dictionary<AudioManager.BgmTypes, AudioClip>
            {
                // Logo Bgm
                {
                    AudioManager.BgmTypes.LogoBGM,
                    Resources.Load<AudioClip>($"{SrcPathRoot}/{SrcPathBgm}/{SrcPathLogoBgm}")
                },

                // Main Bgm
                {
                    AudioManager.BgmTypes.MainBGM1,
                    Resources.Load<AudioClip>($"{SrcPathRoot}/{SrcPathBgm}/{SrcPathMainBgm1}")
                },
                {
                    AudioManager.BgmTypes.MainBGM2,
                    Resources.Load<AudioClip>($"{SrcPathRoot}/{SrcPathBgm}/{SrcPathMainBgm2}")
                },
                {
                    AudioManager.BgmTypes.MainBGM3,
                    Resources.Load<AudioClip>($"{SrcPathRoot}/{SrcPathBgm}/{SrcPathMainBgm3}")
                },
                {
                    AudioManager.BgmTypes.MainBGM4,
                    Resources.Load<AudioClip>($"{SrcPathRoot}/{SrcPathBgm}/{SrcPathMainBgm4}")
                },

                // Battle Bgm
                {
                    AudioManager.BgmTypes.BattleBGM1,
                    Resources.Load<AudioClip>($"{SrcPathRoot}/{SrcPathBgm}/{SrcPathBattleBgm1}")
                },
                {
                    AudioManager.BgmTypes.BattleBGM2,
                    Resources.Load<AudioClip>($"{SrcPathRoot}/{SrcPathBgm}/{SrcPathBattleBgm2}")
                },
                {
                    AudioManager.BgmTypes.BattleBGM3,
                    Resources.Load<AudioClip>($"{SrcPathRoot}/{SrcPathBgm}/{SrcPathBattleBgm3}")
                },
                {
                    AudioManager.BgmTypes.BattleBGM4,
                    Resources.Load<AudioClip>($"{SrcPathRoot}/{SrcPathBgm}/{SrcPathBattleBgm4}")
                }
            };

            IsInitialized = true;
            yield break;
        }

        #endregion
    }
}