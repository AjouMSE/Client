using System;
using System.Collections.Generic;

namespace DataTable
{
    public class TableDataBase
    {
        public int code;

        public int table { get { return CalculateTable(code); } }
        public int category { get { return CalculateCategory(code); } }
        public int index { get { return CalculateIndex(code); } }

        virtual public void SetEnc() { }

        public static int CalculateTable(int code) { return code / 1000000; }
        public static int CalculateCategory(int code) { return code / 1000 % 1000; }
        public static int CalculateIndex(int code) { return code % 1000; }
        public static int CalculateCode(int table, int category, int index) { return table * 1000000 + category * 1000 + index; }

    }   
}