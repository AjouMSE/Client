using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Manager
{
    public class UserManager : MonoSingleton<UserManager>
    {
        #region Private variables

        private Packet.User _user;
        private Packet.Hostile _hostile;
        private bool _isSignedIn;
        private bool _isHostileExist;
        private bool _isHost;

        #endregion
        
        
        #region Public variables
        
        public Packet.User User => _user;
        public Packet.Hostile Hostile => _hostile;
        public bool IsHost => _isHost;

        #endregion

        
        #region Custom methods
        
        public void SignInUserInfo(string json)
        {
            if (!_isSignedIn)
            {
                try
                {
                    _user = JsonUtility.FromJson<Packet.User>(json);
                    _isSignedIn = true;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public void SignOutUserInfo()
        {
            if (_isSignedIn)
            {
                try
                {
                    _user = default;
                    _isSignedIn = false;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public void AddHostileInfo(Packet.Hostile hostile)
        {
            if (!_isHostileExist)
            {
                try
                {
                    _hostile = hostile;
                    _isHostileExist = true;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public void RemoveHostileInfo()
        {
            if (_isHostileExist)
            {
                try
                {
                    _hostile = default;
                    _isHostileExist = false;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        public void SetHost()
        {
            _isHost = true;
        }

        public void SetClient()
        {
            _isHost = false;
        }
        
        #endregion
    }   
}
