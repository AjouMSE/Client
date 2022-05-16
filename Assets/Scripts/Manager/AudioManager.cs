using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace Manager
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        #region Private variables

        // Dir path
        private const string SrcRootPath = "Audio";
        private const string SrcBgmPath = "/BGM";
        private const string SrcSfxPath = "/SFX";
        private const string SrcMixerPath = "/Mixer";

        // Bgm path
        private const string SrcLogoBgmPath = "/LogoBgm"; // 0
        private const string SrcMainBgm1Path = "/MainBgm01"; // 1
        private const string SrcMainBgm2Path = "/MainBgm02"; // 2
        private const string SrcMainBgm3Path = "/MainBgm03"; // 3
        private const string SrcMainBgm4Path = "/MainBgm04"; // 4
        private const string SrcBattleBgm1Path = "/BattleBgm1"; // 5
        private const string SrcBattleBgm2Path = "/BattleBgm2"; // 6
        private const string SrcBattleBgm3Path = "/BattleBgm3"; // 7
        private const string SrcBattleBgm4Path = "/BattleBgm4"; // 8

        private const string BgmVolume = "BgmVolume";
        private const string SfxVolume = "SfxVolume";

        private const float MuteVolume = -80;

        private AudioSource _audioSource;
        private AudioMixer _audioMixer;

        private Dictionary<BgmTypes, AudioClip> _bgmDictionary;

        private float _bgmVolume, _sfxVolume;
        private bool _isBgmMute, _isSfxMute;

        #endregion


        #region Public variables

        public float bgmVolume => _bgmVolume;
        public float sfxVolume => _sfxVolume;
        public bool isBgmMute => _isBgmMute;
        public bool isSfxMute => _isSfxMute;

        public enum BgmTypes
        {
            LogoBgm = 0,
            MainBgm1 = 1,
            MainBgm2 = 2,
            MainBgm3 = 3,
            MainBgm4 = 4,
            BattleBgm1 = 5,
            BattleBgm2 = 6,
            BattleBgm3 = 7,
            BattleBgm4 = 8
        }

        public enum VolumeTypes
        {
            Bgm,
            Sfx
        }

        #endregion


        #region Custom methods

        /// <summary>
        /// Initialize instance variables, Cache audio clips.
        /// </summary>
        public void Init()
        {
            // Init Audio source & audio mixer
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioMixer = Resources.Load<AudioMixer>($"{SrcRootPath}{SrcMixerPath}");

            // _audioMixer.FindMatchingGroups("Master") -> Idx 0: Master, Idx 1: BGM, Idx 2: SFX
            _audioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Master")[1];

            // Cache bgm audio clip
            _bgmDictionary = new Dictionary<BgmTypes, AudioClip>();

            // Logo Bgm
            _bgmDictionary.Add(BgmTypes.LogoBgm,
                Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcLogoBgmPath}"));

            // Main Bgm
            _bgmDictionary.Add(BgmTypes.MainBgm1,
                Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcMainBgm1Path}"));
            _bgmDictionary.Add(BgmTypes.MainBgm2,
                Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcMainBgm2Path}"));
            _bgmDictionary.Add(BgmTypes.MainBgm3,
                Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcMainBgm3Path}"));
            _bgmDictionary.Add(BgmTypes.MainBgm4,
                Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcMainBgm4Path}"));

            // Battle Bgm
            _bgmDictionary.Add(BgmTypes.BattleBgm1,
                Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcBattleBgm1Path}"));
            _bgmDictionary.Add(BgmTypes.BattleBgm2,
                Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcBattleBgm2Path}"));
            _bgmDictionary.Add(BgmTypes.BattleBgm3,
                Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcBattleBgm3Path}"));
            _bgmDictionary.Add(BgmTypes.BattleBgm4,
                Resources.Load<AudioClip>($"{SrcRootPath}{SrcBgmPath}{SrcBattleBgm4Path}"));
        }

        /// <summary>
        /// Play bgm in AudioSource component of instance
        /// </summary>
        /// <param name="type">Type of bgm (ex. BgmTypes.LogoBgm)</param>
        /// <param name="loop">Whether to play Bgm repeatedly</param>
        public void PlayBgm(BgmTypes type, bool loop)
        {
            _audioSource.clip = _bgmDictionary[type];
            _audioSource.loop = loop;

            if (_audioSource.isPlaying)
                _audioSource.Stop();

            _audioSource.Play();
        }

        /// <summary>
        /// Set Bgm, Sfx volume
        /// </summary>
        /// <param name="type"> Volume types (ex. VolumeTypes.Bgm) </param>
        /// <param name="volume"> Volume scale value. (Please enter a value between 0.0001 and 1. This value is replaced by -80 to 0.) </param>
        public void SetVolume(VolumeTypes type, float volume)
        {
            if (volume < 0.0001f || volume > 1) return;
            float convertVolume = Mathf.Log10(volume) * 20;

            switch (type)
            {
                case VolumeTypes.Bgm:
                    _bgmVolume = convertVolume;
                    PlayerPrefs.SetFloat(BgmVolume, _bgmVolume);
                    if (!_isBgmMute) _audioMixer.SetFloat(BgmVolume, _bgmVolume);
                    break;

                case VolumeTypes.Sfx:
                    _sfxVolume = convertVolume;
                    PlayerPrefs.SetFloat(SfxVolume, _sfxVolume);
                    if (!_isSfxMute)
                        _audioMixer.SetFloat(SfxVolume, _sfxVolume);
                    break;
            }
        }
        
        /// <summary>
        /// Mute or unmute bgm, sfx
        /// </summary>
        /// <param name="type"> Volume types (ex. VolumeTypes.Bgm) </param>
        /// <param name="mute">
        /// If true, mute bgm (set volume of audioMixer to -80).
        /// Else, Set bgm volume to _bgmVolume
        /// </param>
        public void MuteOrUnmute(VolumeTypes type, bool mute)
        {
            switch (type)
            {
                case VolumeTypes.Bgm:
                    _isBgmMute = mute;
                    PlayerPrefs.SetInt($"{BgmVolume}Mute", mute ? 1 : 0);
                    _audioMixer.SetFloat(BgmVolume, mute ? MuteVolume : _bgmVolume);
                    break;
                
                case VolumeTypes.Sfx:
                    _isSfxMute = mute;
                    PlayerPrefs.SetInt($"{SfxVolume}Mute", mute ? 1 : 0);
                    _audioMixer.SetFloat(SfxVolume, mute ? MuteVolume : _sfxVolume);
                    break;
            }
        }
        
        #endregion
    }
}