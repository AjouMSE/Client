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
        
        public static string GenColorText(string text, int r, int g, int b)
        {
            string strR = r.ToString("x");
            string strG = g.ToString("x");
            string strB = b.ToString("x");

            return String.Format("<color='#{0}{1}{2}'>{3}</color>", strR, strG, strB, text);
        }

        public static bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([0-9a-zA-Z]+)@([0-9a-zA-Z]+)(\.[0-9a-zA-Z]+){1,}$");
            return regex.IsMatch(email);
        }

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