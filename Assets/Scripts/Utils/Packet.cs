using System;
using System.Collections.Generic;
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
            public string nickname, socketId;
            public int win, lose, draw, score, ranking;

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"User: [ id: {id.ToString()}, ");
                sb.Append($"nickname: {nickname}, ");
                sb.Append($"win: {win.ToString()}, ");
                sb.Append($"lose: {lose.ToString()}, ");
                sb.Append($"draw: {draw.ToString()}, ");
                sb.Append($"score: {score.ToString()}, ");
                sb.Append($"ranking: {ranking.ToString()}, ");
                sb.Append($"socketId: {socketId} ]");

                return sb.ToString();
            }
        }

        [Serializable]
        public struct UserList
        {
            public List<User> users;

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("UserList: [\n");
                for (int i = 0; i < users.Count; i++)
                {
                    sb.Append(users[i].ToString());
                    sb.Append('\n');
                }
                sb.Append(" ]");

                return sb.ToString();
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
            public User hostile;

            public override string ToString()
            {
                return $"MatchMadeResult: [ type: {type.ToString()}, room: {room}, hostile: {hostile.ToString()} ]";
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

        [Serializable]
        public struct BattleResult
        {
            public string result;

            public override string ToString()
            {
                return $"BattleResult: [ result: {result}]";
            }
        }
    }
}