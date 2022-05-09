using System;
using System.Text;
using UnityEngine;

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
            public long id;
            public string nickname;
            public int win, lose, draw, ranking;

            public override string ToString()
            {
                return $"User: [ id: {id.ToString()}, " +
                       $"nickname: {nickname}, " +
                       $"win: {win.ToString()}, " +
                       $"lose: {lose.ToString()}, " +
                       $"draw: {draw.ToString()}, " +
                       $"ranking: {ranking.ToString()} ]";
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
            public long id;

            public override string ToString()
            {
                return $"Auth: [ idx: {id.ToString()} ]";
            }
        }

        [Serializable]
        public struct SioReqResult
        {
            public bool result;

            public override string ToString()
            {
                return $"AuthResult: [ result: {result.ToString()} ]";
            }
        }

        [Serializable]
        public struct MatchMadeResult
        {
            public int type;
            public string room;
            public Hostile hostile;
            
            public override string ToString()
            {
                return $"MatchMadeResult: [ type: {type.ToString()}, room: {room}, hostile: {hostile.ToString()} ]";
            }
        }

        [Serializable]
        public struct Hostile
        {
            public int id;
            public string nickname, socketId;
            public int win, lose, draw, ranking;
            
            public override string ToString()
            {
                return $"User: [ id: {id.ToString()}, " +
                       $"nickname: {nickname}, " +
                       $"win: {win.ToString()}, " +
                       $"lose: {lose.ToString()}, " +
                       $"draw: {draw.ToString()}, " +
                       $"ranking: {ranking.ToString()}, " + 
                       $"socketId: {socketId} ]";
            }
        }

        [Serializable]
        public struct MatchCode
        {
            public string room;
            public string code;
            
            public override string ToString()
            {
                return $"MatchCode: [ room: {room}, code: {code} ]";
            }
        }
    }
}