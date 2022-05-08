using System;

namespace Utils
{
    public class Packet
    {
        [Serializable]
        public struct Account
        {
            public string email;
            public string password;
            public string nickname;
        }

        [Serializable]
        public struct User
        {
            public int id;
            public string nickname;
            public int win, lose, draw, ranking;
        }
    }
}