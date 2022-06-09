using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Manager
{
    public class UserManager : MonoSingleton<UserManager>
    {
        #region Public variables

        public Packet.User User { get; private set; }
        public Packet.User Hostile { get; private set; }

        public bool IsSignedIn { get; private set; }
        public bool IsHost { get; private set; }
        public bool HostileExist { get; private set; }

        #endregion


        #region Public methods

        public override void Init()
        {
            if (!IsInitialized)
            {
                IsInitialized = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        public void SignInUserInfo(string json)
        {
            if (IsSignedIn) return;
            
            try
            {
                User = JsonUtility.FromJson<Packet.User>(json);
                IsSignedIn = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void SignOutUserInfo()
        {
            if (!IsSignedIn) return;

            try
            {
                User = default;
                IsSignedIn = false;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostile"></param>
        public void AddHostileInfo(Packet.User hostile)
        {
            if (!HostileExist) return;
            
            try
            {
                Hostile = hostile;
                HostileExist = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveHostileInfo()
        {
            if (HostileExist) return;
            
            try
            {
                Hostile = default;
                HostileExist = false;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        public void UpdateUserInfo(string json)
        {
            try
            {
                User = JsonUtility.FromJson<Packet.User>(json);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        public void UpdateHostileInfo(string json)
        {
            try
            {
                Hostile = JsonUtility.FromJson<Packet.User>(json);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// Set user to host
        /// </summary>
        public void SetHost()
        {
            IsHost = true;
        }

        /// <summary>
        /// Set user to client
        /// </summary>
        public void SetClient()
        {
            IsHost = false;
        }

        #endregion
    }
}