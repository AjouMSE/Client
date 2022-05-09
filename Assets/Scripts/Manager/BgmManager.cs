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
        private const float MuteValue = -80;

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

        #endregion
        

        #region Custom methods
        
        private void Init()
        {
            // Init Audio source & audio mixer
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioMixer = Resources.Load<AudioMixer>(string.Format("{0}{1}", SrcPath, SrcNameMixer));
            
            // Init main bgm 
            _bgmDict = new Dictionary<string, AudioClip>();
            _bgmDict.Add(SrcNameLogoBgm, Resources.Load<AudioClip>(string.Format("{0}{1}", SrcPath, SrcNameLogoBgm)));
            _bgmDict.Add(SrcNameMainBgm1, Resources.Load<AudioClip>(string.Format("{0}{1}", SrcPath, SrcNameMainBgm1)));
            _bgmDict.Add(SrcNameMainBgm2, Resources.Load<AudioClip>(string.Format("{0}{1}", SrcPath, SrcNameMainBgm2)));
            _bgmDict.Add(SrcNameMainBgm3, Resources.Load<AudioClip>(string.Format("{0}{1}", SrcPath, SrcNameMainBgm3)));
            _bgmDict.Add(SrcNameMainBgm4, Resources.Load<AudioClip>(string.Format("{0}{1}", SrcPath, SrcNameMainBgm4)));
            
            _muteType = MuteType.IsNotMute;
        }
        
        public void SetBgm(string bgmType)
        {
            if (_bgmDict[bgmType] != null)
                _audioSource.clip = _bgmDict[bgmType];
            else
                throw new Exception("UndefinedBgmTypeException");
        }

        public void Play()
        {
            if(_audioSource.isPlaying) 
                _audioSource.Stop(); 
            _audioSource.Play();
        }

        public void AdjustBgmVolume(float volume)
        {
            _bgmVolume = volume;
            _audioMixer.SetFloat("BgmVolume", Mathf.Log10(_bgmVolume) * 20);
            PlayerPrefs.SetFloat("BgmVolume", volume);
        }

        public void MuteBgm()
        {
            _audioMixer.SetFloat("BgmVolume", MuteValue);
            PlayerPrefs.SetInt("IsMute", (int) MuteType.IsMute);
            _muteType = MuteType.IsMute;
        }

        public void UnmuteBgm()
        {
            _audioMixer.SetFloat("BgmVolume", _bgmVolume);
            PlayerPrefs.SetInt("IsMute", (int) MuteType.IsNotMute);
            _muteType = MuteType.IsNotMute;
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