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
                return $"User: [ id: {id.ToString()}, " +
                       $"nickname: {nickname}, " +
                       $"win: {win.ToString()}, " +
                       $"lose: {lose.ToString()}, " +
                       $"draw: {draw.ToString()}, " +
                       $"ranking: {ranking.ToString()}]";
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

        [Serializable]
        public struct Auth
        {
            public int id;

            public override string ToString()
            {
                return $"Auth: [ idx: {id.ToString()}]";
            }
        }

        [Serializable]
        public struct AuthResult
        {
            public string result;

            public override string ToString()
            {
                return $"AuthResult: [ result: {result}]";
            }
        }
    }
}