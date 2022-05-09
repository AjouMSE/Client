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
        private bool _isSignedIn;

        #endregion
        
        
        #region Public variables
        
        public Packet.User User => _user;
        
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
        
        #endregion
    }   
}
