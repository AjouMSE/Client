using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Object
{
    public class Panel : MonoBehaviour
    {
        public int idx, x, y;

        public Panel(int idx, int x, int y)
        {
            this.idx = idx;
            this.x = x;
            this.y = y;
        }
    }   
}
