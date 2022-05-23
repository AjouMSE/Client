using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;

namespace Core
{
    public class BitMask
    {
        #region Private constants

        private const int Bits5BlockSize = 5, Bits6BlockSize = 6;

        // Mask for 5 bits block
        private const int Bits5Mask0 = 0x1f;
        private const int Bits5Mask1 = 0x3e0;
        private const int Bits5Mask2 = 0x7c00;
        private const int Bits5Mask3 = 0xf8000;
        private const int Bits5Mask4 = 0x1f00000;

        // Mask for 6 bits block
        private const int Bits6Mask0 = 0x3f;
        private const int Bits6Mask1 = 0xfc0;
        private const int Bits6Mask2 = 0x3f000;
        private const int Bits6Mask3 = 0xfc0000;
        private const int Bits6Mask4 = 0x3f000000;

        private const int BitField25Full = 0x1ffffff;   // 11111 11111 11111 11111 11111
        private const int BitField30Full = 0x3fffffff;  // 111111 111111 111111 111111 111111
        private const int BitFieldEmpty = 0x0;

        #endregion
        
        
        #region Public Constant

        public const int BitField25Msb = 0x1000000;   // 10000 00000 00000 00000 00000 (1 << 24)
        public const int BitField30Msb = 0x20000000;  // 100000 000000 000000 000000 000000 (1 << 29)
        public const int BitFieldLsb = 0x1;
        
        #endregion


        #region Structs of bit field

        /// <summary>
        /// Struct of 25 bits field (5x5)
        /// Used for skill range
        /// </summary>
        public struct BitField25
        {
            private int _element;
            public int element => _element;

            public BitField25(int element)
            {
                _element = element & BitField25Full;
            }

            public BitField25(string binaryString)
            {
                binaryString = binaryString.Replace(" ", "");
                _element = Convert.ToInt32(binaryString, 2) & BitField25Full;
            }

            public override string ToString()
            {
                return Convert.ToString(element, 2).PadLeft(25, '0');
            }

            /// <summary>
            /// Shift BitField25 to x axis (-x: left, +x: right)
            /// ex) 00001                    00000
            ///     10101                    01010
            ///     10001  = shift x: +1 =>  01000
            ///     00000                    00000
            ///     11111                    01111
            /// </summary>
            /// <param name="x">Amount to shift along the x-axis</param>
            private void ShiftX(int x)
            {
                if (x == 0) return;
            
                int temp = 0;
                if (x > 0)
                {
                    temp += ((_element & Bits5Mask0) >> x) & Bits5Mask0;
                    temp += ((_element & Bits5Mask1) >> x) & Bits5Mask1;
                    temp += ((_element & Bits5Mask2) >> x) & Bits5Mask2;
                    temp += ((_element & Bits5Mask3) >> x) & Bits5Mask3;
                    temp += ((_element & Bits5Mask4) >> x) & Bits5Mask4;
                }
                else
                {
                    temp += ((_element & Bits5Mask0) << -x) & Bits5Mask0;
                    temp += ((_element & Bits5Mask1) << -x) & Bits5Mask1;
                    temp += ((_element & Bits5Mask2) << -x) & Bits5Mask2;
                    temp += ((_element & Bits5Mask3) << -x) & Bits5Mask3;
                    temp += ((_element & Bits5Mask4) << -x) & Bits5Mask4;
                }

                _element = temp;
            }
            
            
            /// <summary>
            /// Shift BitField25 to y axis (-y: up, +y: down)
            /// ex) 00001                    00000
            ///     10101                    00001
            ///     10001  = shift y: +1 =>  10101
            ///     00000                    10001
            ///     11111                    00000
            /// </summary>
            /// <param name="y">Amount to shift along the y-axis</param>
            private void ShiftY(int y)
            {
                if (y == 0) return;
                if (y > 0) _element = _element >> (Bits5BlockSize * y);
                else _element = _element << (Bits5BlockSize * -y);
            }
            
            
            /// <summary>
            /// Shift BitField25 to x, y axis(-x: left, +x: right, -y: up, +y: down)
            /// </summary>
            /// <param name="x">Amount to shift along the x-axis</param>
            /// <param name="y">Amount to shift along the y-axis</param>
            public void Shift(int x, int y)
            {
                ShiftX(x);
                ShiftY(y);
            }
            
            
            /// <summary>
            /// Convert BitField25 to BitField30 (Left zero padding to each 5bits block)
            /// ex) 10101               010101
            ///     10000               010000
            ///     00100  = convert => 000100
            ///     00000               000000
            ///     00001               000001
            /// </summary>
            /// <param name="bits25">Struct of 25 bits field</param>
            /// <returns> Converted struct of Bits30Field </returns>
            public BitField30 CvtBits25ToBits30()
            {
                int val = 0;
                val += (_element & Bits5Mask0);
                val += (_element & Bits5Mask1) << 1;
                val += (_element & Bits5Mask2) << 2;
                val += (_element & Bits5Mask3) << 3;
                val += (_element & Bits5Mask4) << 4;

                return new BitField30(val);;
            }
            
            /// <summary>
            /// Convert BitField25 to 2D string
            /// </summary>
            /// <returns> string to display the element in 2D </returns>
            private string Bits25To2DString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"\n{Convert.ToString((_element & Bits5Mask4) >> 20, 2).PadLeft(Bits5BlockSize, '0')}");
                sb.Append($"\n{Convert.ToString((_element & Bits5Mask3) >> 15, 2).PadLeft(Bits5BlockSize, '0')}");
                sb.Append($"\n{Convert.ToString((_element & Bits5Mask2) >> 10, 2).PadLeft(Bits5BlockSize, '0')}");
                sb.Append($"\n{Convert.ToString((_element & Bits5Mask1) >> 5, 2).PadLeft(Bits5BlockSize, '0')}");
                sb.Append($"\n{Convert.ToString((_element & Bits5Mask0), 2).PadLeft(Bits5BlockSize, '0')}");

                return sb.ToString();
            }
            
            /// <summary>
            /// Print out BitField25 (2D form)
            /// </summary>
            public void PrintBy2D()
            {
                Debug.Log(Bits25To2DString());
            }
        }
        
        

        /// <summary>
        /// Struct of 30 bits field (6x5)
        /// Used for battle field range
        /// </summary>
        public struct BitField30
        {
            private int _element;
            public int element => _element;

            public BitField30(int element)
            {
                _element = element & BitField30Full;
            }

            public BitField30(string binaryString)
            {
                binaryString = binaryString.Replace(" ", "");
                _element = Convert.ToInt32(binaryString, 2) & BitField30Full;
            }

            public override string ToString()
            {
                return Convert.ToString(_element, 2).PadLeft(30, '0');
            }
            
            
            /// <summary>
            /// Shift BitField30 to x axis (-x: left, +x: right)
            /// ex) 000001                    000000
            ///     000010                    000001
            ///     001000  = shift x: +1 =>  000100
            ///     100000                    010000
            ///     000000                    000000
            /// </summary>
            /// <param name="x">Amount to shift along the x-axis</param>
            private void ShiftX(int x)
            {
                if (x == 0) return;
            
                int temp = 0;
                if (x > 0)
                {
                    temp += ((_element & Bits6Mask0) >> x) & Bits6Mask0;
                    temp += ((_element & Bits6Mask1) >> x) & Bits6Mask1;
                    temp += ((_element & Bits6Mask2) >> x) & Bits6Mask2;
                    temp += ((_element & Bits6Mask3) >> x) & Bits6Mask3;
                    temp += ((_element & Bits6Mask4) >> x) & Bits6Mask4;
                }
                else
                {
                    temp += ((_element & Bits6Mask0) << -x) & Bits6Mask0;
                    temp += ((_element & Bits6Mask1) << -x) & Bits6Mask1;
                    temp += ((_element & Bits6Mask2) << -x) & Bits6Mask2;
                    temp += ((_element & Bits6Mask3) << -x) & Bits6Mask3;
                    temp += ((_element & Bits6Mask4) << -x) & Bits6Mask4;
                }

                _element = temp;
            }
            
            
            /// <summary>
            /// Shift BitField30 to y axis (-y: up, +y: down)
            /// ex) 000001                    000000
            ///     000010                    000001
            ///     001000  = shift y: +1 =>  000010
            ///     100000                    001000
            ///     000000                    100000
            /// </summary>
            /// <param name="y">Amount to shift along the y-axis</param>
            private void ShiftY(int y)
            {
                if (y == 0) return;
                if (y > 0) _element = _element >> (Bits6BlockSize * y);
                else _element = _element << (Bits6BlockSize * -y);
            }
            
            
            /// <summary>
            /// Shift BitField30 to x, y axis (-x: left, +x: right, -y: up, +y: down)
            /// </summary>
            /// <param name="x">Amount to shift along the x-axis</param>
            /// <param name="y">Amount to shift along the y-axis</param>
            public void Shift(int x, int y)
            {
                ShiftX(x);
                ShiftY(y);
            }
            
            
            /// <summary>
            /// Convert BitField30 to 2D string
            /// </summary>
            /// <returns> string to display the element in 2D </returns>
            private string Bits30To2DString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append($"\n{Convert.ToString((_element & Bits6Mask4) >> 24, 2).PadLeft(Bits6BlockSize, '0')}");
                sb.Append($"\n{Convert.ToString((_element & Bits6Mask3) >> 18, 2).PadLeft(Bits6BlockSize, '0')}");
                sb.Append($"\n{Convert.ToString((_element & Bits6Mask2) >> 12, 2).PadLeft(Bits6BlockSize, '0')}");
                sb.Append($"\n{Convert.ToString((_element & Bits6Mask1) >> 6, 2).PadLeft(Bits6BlockSize, '0')}");
                sb.Append($"\n{Convert.ToString((_element & Bits6Mask0), 2).PadLeft(Bits6BlockSize, '0')}");

                return sb.ToString();
            }

            /// <summary>
            /// Print out BitField30 (2D form)
            /// </summary>
            public void PrintBy2D()
            {
                Debug.Log(Bits30To2DString());
            }
        }

        #endregion
    }
}