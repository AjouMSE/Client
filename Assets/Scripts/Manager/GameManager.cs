using System.Collections;
using System.Collections.Generic;
using Object;
using UnityEngine;
using Utils;

namespace Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {

        #region Private variables
        private GameObject[] _panels;
        private const int Width = 6, Height = 5;
        private int _selectionTimer;
        
        public readonly List<int> selectedCardList = new List<int>();
        public readonly List<int> selectedCardListHostile = new List<int>();

        #endregion

        #region Init methods
        public void InitPanel(GameObject panel)
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
                        _panels[idx] = Instantiate(panel, new Vector3(j * 4.2f, 0.2f, i * 3.2f), Quaternion.identity);
                    }
                }
            }
        }

        #endregion
        
        


        #region Logic methods
        

        private void InvokeTurn()
        {

        }

        #endregion


        #region Coroutines

        IEnumerator Timer()
        {
            yield return null;
        }


        #endregion
    }
}