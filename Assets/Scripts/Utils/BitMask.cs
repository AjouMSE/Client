using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class BitMask
    {
        #region Constants

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

        #endregion
        
        
        #region Structs of bit field
        
        /// <summary>
        /// Struct of 25 bits field (5x5)
        /// Used for skill range
        /// </summary>
        public struct Bits25Field
        {
            public int element;

            public Bits25Field(int element)
            {
                this.element = element;
            }

            public Bits25Field(string binaryString)
            {
                element = Convert.ToInt32(binaryString, 2);
            }
        }

        /// <summary>
        /// Struct of 30 bits field (6x5)
        /// Used for battle field range
        /// </summary>
        public struct Bits30Field
        {
            public int element;
            
            public Bits30Field(int element)
            {
                this.element = element;
            }

            public Bits30Field(string binaryString)
            {
                element = Convert.ToInt32(binaryString, 2);
            }
        }
        
        #endregion

        
        #region Custom static methods
        
        /// <summary>
        /// Convert 25 bits field to 30 bits field (Left zero padding to each 5bits block)
        /// ex) 10101               010101
        ///     10000               010000
        ///     00100  = convert => 000100
        ///     00000               000000
        ///     00001               000001
        /// </summary>
        /// <param name="bits25">Struct of 25 bits field</param>
        /// <returns> Converted struct of Bits30Field </returns>
        public static Bits30Field CvtBits25ToBits30(Bits25Field bits25)
        {
            Bits30Field bits30 = new Bits30Field();
            bits30.element += (bits25.element & Bits5Mask0);
            bits30.element += (bits25.element & Bits5Mask1) << 1;
            bits30.element += (bits25.element & Bits5Mask2) << 2;
            bits30.element += (bits25.element & Bits5Mask3) << 3;
            bits30.element += (bits25.element & Bits5Mask4) << 4;

            return bits30;
        }
        
        
        /// <summary>
        /// Shift 25 bits field to x axis (-x: left, +x: right)
        /// ex) 00001                    00000
        ///     10101                    01010
        ///     10001  = shift x: +1 =>  01000
        ///     00000                    00000
        ///     11111                    01111
        /// </summary>
        /// <param name="bits25">Struct of 25 bits field</param>
        /// <param name="x">Amount to shift along the x-axis</param>
        public static void ShiftXBits25(Bits25Field bits25, int x)
        {
            int temp = 0;
            if (x > 0)
            {
                temp += ((bits25.element & Bits5Mask0) >> x) & Bits5Mask0;
                temp += ((bits25.element & Bits5Mask1) >> x) & Bits5Mask1;
                temp += ((bits25.element & Bits5Mask2) >> x) & Bits5Mask2;
                temp += ((bits25.element & Bits5Mask3) >> x) & Bits5Mask3;
                temp += ((bits25.element & Bits5Mask4) >> x) & Bits5Mask4;
            }
            else
            {
                temp += ((bits25.element & Bits5Mask0) << -x) & Bits5Mask0;
                temp += ((bits25.element & Bits5Mask1) << -x) & Bits5Mask1;
                temp += ((bits25.element & Bits5Mask2) << -x) & Bits5Mask2;
                temp += ((bits25.element & Bits5Mask3) << -x) & Bits5Mask3;
                temp += ((bits25.element & Bits5Mask4) << -x) & Bits5Mask4;
            }

            bits25.element = temp;
        }

        
        /// <summary>
        /// Shift 25 bits field to y axis(-y: up, +y: down)
        /// ex) 00001                    00000
        ///     10101                    00001
        ///     10001  = shift y: +1 =>  10101
        ///     00000                    10001
        ///     11111                    00000
        /// </summary>
        /// <param name="bits25">Struct of 25 bits field</param>
        /// <param name="y">Amount to shift along the y-axis</param>
        public static void ShiftYBits25(Bits25Field bits25, int y)
        {
            if (y > 0) bits25.element = bits25.element >> (5 * y);
            else bits25.element = bits25.element << (5 * -y);
        }
        
        
        /// <summary>
        /// Shift 30 bits field to x axis (-x: left, +x: right)
        /// ex) 000001                    000000
        ///     000010                    000001
        ///     001000  = shift x: +1 =>  000100
        ///     100000                    010000
        ///     000000                    000000
        /// </summary>
        /// <param name="bits30">Struct of 30 bits field</param>
        /// <param name="x">Amount to shift along the x-axis</param>
        public static void ShiftXBits30(Bits30Field bits30, int x)
        {
            int temp = 0;
            if (x > 0)
            {
                temp += ((bits30.element & Bits6Mask0) >> x) & Bits6Mask0;
                temp += ((bits30.element & Bits6Mask1) >> x) & Bits6Mask1;
                temp += ((bits30.element & Bits6Mask2) >> x) & Bits6Mask2;
                temp += ((bits30.element & Bits6Mask3) >> x) & Bits6Mask3;
                temp += ((bits30.element & Bits6Mask4) >> x) & Bits6Mask4;
            }
            else
            {
                temp += ((bits30.element & Bits6Mask0) << -x) & Bits6Mask0;
                temp += ((bits30.element & Bits6Mask1) << -x) & Bits6Mask1;
                temp += ((bits30.element & Bits6Mask2) << -x) & Bits6Mask2;
                temp += ((bits30.element & Bits6Mask3) << -x) & Bits6Mask3;
                temp += ((bits30.element & Bits6Mask4) << -x) & Bits6Mask4;
            }

            bits30.element = temp;
        }

        /// <summary>
        /// Shift 30 bits field to y axis (-y: up, +y: down)
        /// ex) 000001                    000000
        ///     000010                    000001
        ///     001000  = shift y: +1 =>  000010
        ///     100000                    001000
        ///     000000                    100000
        /// </summary>
        /// <param name="bits30">Struct of 30 bits field</param>
        /// <param name="y">Amount to shift along the y-axis</param>
        public static void ShiftYBits30(Bits30Field bits30, int y)
        {
            if (y > 0) bits30.element = bits30.element >> (6 * y);
            else bits30.element = bits30.element << (6 * -y);
        }
        
        #endregion
    }
}
