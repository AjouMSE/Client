using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Cache;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace Manager
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        #region Private constants

        private const string BgmVolume = "BgmVolume";
        private const string SfxVolume = "SfxVolume";
        
        private const float MuteVolume = -80;

        #endregion
        
        
        #region Private variables
        
        private AudioSource _audioSource;

        #endregion


        #region Public variables

        public float BGMVolume { get; private set; }
        public float SFXVolume { get; private set; }

        public bool IsBgmMute { get; private set; }
        public bool IsSfxMute { get; private set; }

        public enum BgmTypes
        {
            LogoBGM = 0,
            MainBGM1 = 1,
            MainBGM2 = 2,
            MainBGM3 = 3,
            MainBGM4 = 4,
            BattleBGM1 = 5,
            BattleBGM2 = 6,
            BattleBGM3 = 7,
            BattleBGM4 = 8
        }

        public enum VolumeTypes
        {
            BGM,
            SFX
        }

        #endregion


        #region Custom methods

        /// <summary>
        /// Initialize instance variables, Cache audio clips.
        /// </summary>
        public override void Init()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
            
                // Init Audio source & audio mixer
                _audioSource = gameObject.AddComponent<AudioSource>();

                // _audioMixer.FindMatchingGroups("Master") -> Idx 0: Master, Idx 1: BGM, Idx 2: SFX
                _audioSource.outputAudioMixerGroup = CacheAudioSource.Instance.AudioMixer.FindMatchingGroups("BGM")[0];   
            }
        }

        /// <summary>
        /// Play bgm in AudioSource component of instance
        /// </summary>
        /// <param name="type">Type of bgm (ex. BgmTypes.LogoBgm)</param>
        /// <param name="loop">Whether to play Bgm repeatedly</param>
        public void PlayBgm(BgmTypes type, bool loop)
        {
            _audioSource.clip = CacheAudioSource.Instance.GetSource(type);
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
            float convertVolume = ConvertVolume(volume);

            switch (type)
            {
                case VolumeTypes.BGM:
                    BGMVolume = volume;
                    PlayerPrefs.SetFloat(BgmVolume, volume);
                    if (!IsBgmMute) 
                        CacheAudioSource.Instance.AudioMixer.SetFloat(BgmVolume, convertVolume);
                    break;

                case VolumeTypes.SFX:
                    SFXVolume = volume;
                    PlayerPrefs.SetFloat(SfxVolume, volume);
                    if (!IsSfxMute) 
                        CacheAudioSource.Instance.AudioMixer.SetFloat(SfxVolume, convertVolume);
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
                case VolumeTypes.BGM:
                    IsBgmMute = mute;
                    PlayerPrefs.SetInt($"{BgmVolume}Mute", mute ? 1 : 0);
                    CacheAudioSource.Instance.AudioMixer.SetFloat(BgmVolume, mute ? MuteVolume : ConvertVolume(BGMVolume));
                    break;
                
                case VolumeTypes.SFX:
                    IsSfxMute = mute;
                    PlayerPrefs.SetInt($"{SfxVolume}Mute", mute ? 1 : 0);
                    CacheAudioSource.Instance.AudioMixer.SetFloat(SfxVolume, mute ? MuteVolume : ConvertVolume(SFXVolume));
                    break;
            }
        }

        /// <summary>
        /// Convert volume value
        /// </summary>
        /// <param name="volume"> Volume scale value. (Please enter a value between 0.0001 and 1. This value is replaced by -80 to 0.) </param>
        private float ConvertVolume(float volume)
        {
            return Mathf.Log10(volume) * 20;
        }
        
        #endregion
    }
}