using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Manager
{
    public class UserManager : MonoSingleton<UserManager>
    {
        private long _id { get; }
        private string _nickname { get; set; }
        private int _score { get; set; }
        private int _win { get; }
        private int _lose { get; }
        private int _ranking { get; }

        public void InitUserInfo()
        {
        }
    }   
}
