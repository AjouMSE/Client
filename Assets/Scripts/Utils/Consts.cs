using System;
using System.Text;
using UnityEngine;

namespace Utils
{
    public class Consts
    {
        #region UI Constants

        public const string TagMainCamera = "MainCamera";
        public const string TagHudCamera = "HUDCamera";

        #endregion


        #region InGame-Skill constants

        public enum SkillCategory
        {
            All = 0, 
            Move = 1, 
            Attack = 100,
            Special = 200
        }

        #endregion


        #region InGame-Panel (Map) Constants

        #endregion


        #region InGame-Player Constants

        public const int DefaultHp = 100, DefaultMana = 3;
        public const int MaximumHp = 100, MaximumMana = 10;

        #endregion


        public const int RangeLength = 5;
        public const int Width = 6, Height = 5;
        public const float PanelX = 4.2f, PanelY = 3.2f;
        public const int PanelCnt = Width * Height;

        public const int MaxHP = 100;

        // public const int StartMana = 3, MaxMana = 10;
        public const int StartMana = 3, MaxMana = 10;

        public enum SkillType
        {
            Move = 1,
            Attack = 100,
            Special = 200,
            ManaCharge = 201,
            ManaOverload = 202,
            Concentration = 203,
            Cleanse = 204,
            ManaDisorder = 205,
            LifeRecovery = 206,
            ManaShield = 207
        }

        public enum GameUIType
        {
            Hp,
            Mana
        }

        public enum BattleResult
        {
            WIN,
            LOSE,
            DRAW
        }

        public enum UserType
        {
            Host,
            Client
        }
    }
}