using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace Manager
{
    public class BgmManager : MonoSingleton<BgmManager>
    {
        #region Private variables
        
        private const string SrcPath = "Audio/BGM";
        private const string SrcNameMixer = "/BgmMixer";
        private const float MuteValue = 0f;

        private AudioSource _audioSource;
        private AudioMixer _audioMixer;

        private Dictionary<string, AudioClip> _bgmDict;

        private MuteType _muteType;
        private float _bgmVolume;

        #endregion


        #region Public variables

        public enum MuteType
        {
            IsNotMute = 0,
            IsMute = 1
        }
        
        public const string SrcNameLogoBgm = "/LogoBgm";
        public const string SrcNameMainBgm1 = "/MainBgm01";
        public const string SrcNameMainBgm2 = "/MainBgm02";
        public const string SrcNameMainBgm3 = "/MainBgm03";
        public const string SrcNameMainBgm4 = "/MainBgm04";
        public const string SrcNameBattleBgm = "/BattleBgm";

        public MuteType muteType => _muteType;

        #endregion
        

        #region Custom methods
        
        private void Init()
        {
            // Init Audio source & audio mixer
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioMixer = Resources.Load<AudioMixer>(string.Format("{0}{1}", SrcPath, SrcNameMixer));
            
            // Init main bgm 
            _bgmDict = new Dictionary<string, AudioClip>();
            _bgmDict.Add(SrcNameLogoBgm, Resources.Load<AudioClip>($"{SrcPath}{SrcNameLogoBgm}"));
            _bgmDict.Add(SrcNameMainBgm1, Resources.Load<AudioClip>($"{SrcPath}{SrcNameMainBgm1}"));
            _bgmDict.Add(SrcNameMainBgm2, Resources.Load<AudioClip>($"{SrcPath}{SrcNameMainBgm2}"));
            _bgmDict.Add(SrcNameMainBgm3, Resources.Load<AudioClip>($"{SrcPath}{SrcNameMainBgm3}"));
            _bgmDict.Add(SrcNameMainBgm4, Resources.Load<AudioClip>($"{SrcPath}{SrcNameMainBgm4}"));
            _bgmDict.Add(SrcNameBattleBgm, Resources.Load<AudioClip>($"{SrcPath}{SrcNameBattleBgm}"));
            
            _muteType = MuteType.IsNotMute;
        }
        
        public void SetBgm(string bgmType)
        {
            if (_bgmDict[bgmType] != null)
                _audioSource.clip = _bgmDict[bgmType];
            else
                throw new Exception("UndefinedBgmTypeException");
        }

        public void Play(bool loop)
        {
            _audioSource.loop = loop;
            if(_audioSource.isPlaying) 
                _audioSource.Stop(); 
            _audioSource.Play();
        }

        public void AdjustBgmVolume(float volume)
        {
            //_audioMixer.SetFloat("BgmVolume", Mathf.Log10(_bgmVolume) * 20);
            _bgmVolume = volume;
            _audioSource.volume = volume;
            PlayerPrefs.SetFloat("BgmVolume", volume);
        }

        public void MuteBgm()
        {
            //_audioMixer.SetFloat("BgmVolume", MuteValue);
            _muteType = MuteType.IsMute;
            _audioSource.volume = MuteValue;
            PlayerPrefs.SetInt("IsMute", (int) MuteType.IsMute);
        }

        public void UnmuteBgm()
        {
            //_audioMixer.SetFloat("BgmVolume", _bgmVolume);
            _muteType = MuteType.IsNotMute;
            _audioSource.volume = _bgmVolume;
            PlayerPrefs.SetInt("IsMute", (int) MuteType.IsNotMute);
        }
        
        #endregion

        
        #region Unity event methods

        private void Awake()
        {
            Init();
        }
        
        #endregion
    }
}