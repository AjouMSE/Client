using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;

namespace Utils
{
    public class CustomUtils
    {
        #region public constants

        public const int SHA256 = 0, SHA512 = 1;

        #endregion


        #region UI utility methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string GenColorText(string text, int r, int g, int b)
        {
            string strR = r.ToString("x");
            string strG = g.ToString("x");
            string strB = b.ToString("x");

            return $"<color='#{strR}{strG}{strB}'>{text}</color>";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        public static T FindComponentByName<T>(GameObject root, string name) where T : Component
        {
            var components = root.GetComponentsInChildren<T>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i].name.Equals(name))
                    return components[i];
            }

            Debug.LogWarning($"Cannot Find Component {typeof(T)}");
            return null;
        }

        #endregion


        #region hash utility methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringRange"></param>
        /// <returns></returns>
        public static List<(int i, int j)> ParseRange(string stringRange)
        {
            string[] range = stringRange.Split(' ');

            List<(int i, int j)> list = new List<(int, int)>();

            for (int i = 0; i < Consts.RangeLength; i++)
            {
                for (int j = 0; j < Consts.RangeLength; j++)
                {
                    if (range[i][j] == '0')
                    {
                        continue;
                    }

                    int mid = (int)Math.Floor(Consts.RangeLength / 2.0f);
                    list.Add((mid - i, j - mid));
                }
            }

            return list;
        }


        #region Utility methods

        public static bool IsValidEmail(string email)
        {
            Regex regex = new Regex(@"^([0-9a-zA-Z]+)@([0-9a-zA-Z]+)(\.[0-9a-zA-Z]+){1,}$");
            return regex.IsMatch(email);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawData"></param>
        public static void PrintRawData(byte[] rawData)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < rawData.Length; i++)
            {
                sb.Append(Convert.ToString(rawData[i], 16));
            }

            Debug.Log(sb.ToString());
        }

        #endregion
    }
}