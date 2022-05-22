using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Audio;
using Utils;


namespace Data.Cache
{
    public class CacheAudioSource : MonoSingleton<CacheAudioSource>
    {
        #region Private constants

        private const string SrcRootPath = "Audio";
        private const string SrcBgmPath = "/BGM";
        private const string SrcSfxPath = "/SFX";
        private const string SrcMixerPath = "/Mixer";
        
        private const string SrcLogoBgmPath = "/LogoBgm"; // 0
        private const string SrcMainBgm1Path = "/MainBgm01"; // 1
        private const string SrcMainBgm2Path = "/MainBgm02"; // 2
        private const string SrcMainBgm3Path = "/MainBgm03"; // 3
        private const string SrcMainBgm4Path = "/MainBgm04"; // 4
        private const string SrcBattleBgm1Path = "/BattleBgm1"; // 5
        private const string SrcBattleBgm2Path = "/BattleBgm2"; // 6
        private const string SrcBattleBgm3Path = "/BattleBgm3"; // 7
        private const string SrcBattleBgm4Path = "/BattleBgm4"; // 8

        #endregion


        #region Public variables

        public AudioMixer AudioMixer { get; private set; }
        
        public Dictionary<AudioManager.BgmTypes, AudioClip> BGMCache;

        #endregion
        

        #region Public methods

        public void Init()
        {
            // Init AudioMixer
            AudioMixer = Resources.Load<AudioMixer>($"{SrcRootPath}{SrcMixerPath}");
            
            // Cache bgm to dictionary
            BGMCache = new Dictionary<AudioManager.BgmTypes, AudioClip>
            {
                // Logo Bgm
                { AudioManager.BgmTypes.LogoBGM, Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcLogoBgmPath}") },
                
                // Main Bgm
                { AudioManager.BgmTypes.MainBGM1, Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcMainBgm1Path}") },
                { AudioManager.BgmTypes.MainBGM2, Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcMainBgm2Path}") },
                { AudioManager.BgmTypes.MainBGM3, Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcMainBgm3Path}") },
                { AudioManager.BgmTypes.MainBGM4, Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcMainBgm4Path}") },
                
                // Battle Bgm
                { AudioManager.BgmTypes.BattleBGM1, Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcBattleBgm1Path}") },
                { AudioManager.BgmTypes.BattleBGM2, Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcBattleBgm2Path}") },
                { AudioManager.BgmTypes.BattleBGM3, Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcBattleBgm3Path}") },
                { AudioManager.BgmTypes.BattleBGM4, Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcBattleBgm4Path}") }
            };
        }

        #endregion
    }
}