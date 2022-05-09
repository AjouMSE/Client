using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Utils
{
    public class CustomUtils
    {
        public const int SHA256 = 0, SHA512 = 1;
        
        public static bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([0-9a-zA-Z]+)@([0-9a-zA-Z]+)(\.[0-9a-zA-Z]+){1,}$");
            return regex.IsMatch(email);
        }
        
        #region UI methods
        public static string GenColorText(string text, int r, int g, int b)
        {
            string strR = r.ToString("x");
            string strG = g.ToString("x");
            string strB = b.ToString("x");

            return $"<color='#{strR}{strG}{strB}'>{text}</color>";
        }

        public static string MakeTitleColor()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GenColorText("m", 140, 128, 255));
            sb.Append('a');
            sb.Append(GenColorText("g", 140, 128, 255));
            sb.Append('i');
            sb.Append(GenColorText("c", 140, 128, 255));
            sb.Append("a ");
            sb.Append(GenColorText("d", 140, 128, 255));
            sb.Append('u');
            sb.Append(GenColorText("e", 140, 128, 255));
            sb.Append('l');

            return sb.ToString();
        }
        
        #endregion

        #region hash methods

        public static string MD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(str));
            byte[] hash = md5.Hash;

            StringBuilder sBuilder = new StringBuilder();
            foreach (byte bt in hash)
            {
                sBuilder.Append(bt.ToString("x2"));
            }

            return sBuilder.ToString();
        }


        public static string SHA(string str, int type)
        {
            byte[] hash = null;
            StringBuilder result = new StringBuilder();

            switch (type)
            {
                case SHA256:
                    SHA256 sha256 = new SHA256Managed();
                    hash = sha256.ComputeHash(Encoding.ASCII.GetBytes(str));
                    break;
                case SHA512:
                    SHA512 sha512 = new SHA512Managed();
                    hash = sha512.ComputeHash(Encoding.ASCII.GetBytes(str));
                    break;
                default:
                    Debug.LogError("UnDefinedHashAlgorithmException");
                    break;
            }

            foreach (byte bt in hash)
                result.AppendFormat("{0:x2}", bt);

            return result.ToString();
        }

        #endregion
    }
}