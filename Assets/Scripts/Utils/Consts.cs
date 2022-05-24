using System;
using System.Text;
using UnityEngine;

namespace Utils
{
    public class Consts
    {
        public static int RangeLength = 5;
        public static int Width = 6, Height = 5;
        public static int PanelCnt = Width * Height;

        public static int MaxHP = 100;
        public static int StartMana = 3, MaxMana = 10;

        public enum SkillType { Move = 1, Attack = 100, Special = 200 }

        public enum GameUIType { HP, Mana }

        public enum BattleResult { WIN, LOSE, DRAW }
    }
}