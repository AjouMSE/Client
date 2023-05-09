using System.Collections;
using System.Collections.Generic;
using InGame;
using UnityEngine;
using Utils;

namespace Test.Game
{
    public class PlayerControllerTest2 : MonoBehaviour
    {
        #region Private constants

        private const int Width = 6, Height = 5;

        #endregion


        #region Private variables

        private int _x, _y, _idx;
        private BitMask.BitField30 _bitIdx; // msb is idx 0, lsb is idx 29 (0 ~ 29)

        #endregion


        #region Custom methods

        private BitMask.BitField30 ConvertIdxToBitIdx(int idx)
        {
            int zero = BitMask.BitField30Msb;
            return new BitMask.BitField30(zero >> idx);
        }

        private int ConvertBitIdxToIdx(BitMask.BitField30 bitIdx)
        {
            int mask = BitMask.BitField30Msb;
            for (int i = 0; i < 30; i++)
            {
                if ((bitIdx.element & mask) > 0)
                {
                    return i;
                }
            }

            return -1;
        }

        public bool CheckPlayerHit(BitMask.BitField30 fieldRange)
        {
            return (_bitIdx.element & fieldRange.element) > 0;
        }

        public void Move(int x, int y)
        {
            _x = x;
            _y = y;
            _idx = y * Width + x;
            _bitIdx = ConvertIdxToBitIdx(_idx);
        }

        #endregion
    }
}