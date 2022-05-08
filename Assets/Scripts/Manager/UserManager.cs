using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Manager
{
    public class UserManager : MonoSingleton<UserManager>
    {
        private Packet.User _user;
        private bool isInitialized;

        public void InitUserInfo(string json)
        {
            if (!isInitialized)
            {
                try
                {
                    _user = JsonUtility.FromJson<Packet.User>(json);
                    isInitialized = true;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
    }   
}
