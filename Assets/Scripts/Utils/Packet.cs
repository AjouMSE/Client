using System;

namespace Utils
{
    public class Packet
    {
        [Serializable]
        public struct Account
        {
            public string id, pw;
        }
    }
}