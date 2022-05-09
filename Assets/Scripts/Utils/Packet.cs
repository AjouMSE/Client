using System;
using System.Text;

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

            public override string ToString()
            {
                return $"Account: [ email: {email}, password: {password}, nickname: {nickname} ]";
            }
        }

        [Serializable]
        public struct User
        {
            public int id;
            public string nickname;
            public int win, lose, draw, ranking;

            public override string ToString()
            {
                return $"User: [ id: {id}, nickname: {nickname}, win: {win}, lose: {lose}, draw: {draw}, ranking: {ranking}]";
            }
        }

        [Serializable]
        public struct WebServerException
        {
            public string code;
            public string message;

            public override string ToString()
            {
                return $"WebServerException: [ code: {code}, message: {message} ]";
            }
        }
    }
}