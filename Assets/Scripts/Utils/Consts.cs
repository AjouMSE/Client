using System;
using System.Text;
using UnityEngine;

namespace Utils
{
    public class Consts
    {
        public const int RangeLength = 5;
        public const int Width = 6, Height = 5;
        public const float PanelX = 4.2f, PanelY = 3.2f;
        public const int PanelCnt = Width * Height;

        public const int MaxHP = 100;
        public const int StartMana = 3, MaxMana = 10;

        public enum SkillType { Move = 1, Attack = 100, Special = 200, ManaCharge = 201 }

        public enum GameUIType { HP, Mana }

        public enum BattleResult { WIN, LOSE, DRAW }

        public enum UserType { Host, Client }
    }
}