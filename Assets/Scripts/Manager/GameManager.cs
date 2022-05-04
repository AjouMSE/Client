using System.Collections;
using System.Collections.Generic;
using Object;
using UnityEngine;
using Utils;

namespace Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private GameObject[] _panels;
        private const int Width = 6, Height = 5;

        public void Init(GameObject panel)
        {
            if (_panels == null)
            {
                int mapSize = Width * Height;
                _panels = new GameObject[mapSize];
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        int idx = i * Height + j;
                        _panels[idx] = Instantiate(panel, new Vector3(j * 4, 0.2f, i * 3), Quaternion.identity);
                    }
                }   
            }
        }
    }
}