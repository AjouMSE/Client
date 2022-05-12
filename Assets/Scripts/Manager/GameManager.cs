using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Object;
using Scene;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {

        #region Private variables

        private const int Width = 6, Height = 5;
        
        private GameObject[] _panels;
        private float _selectionTimer;
        private int _turnCnt;
        private bool _canSelect;

        #endregion
        
        
        #region Public variables

        public GameObject scroll3D, skillVfx, skillVfx2;
        public Text timerText, turnText;
        
        #endregion
        

        #region Custom methods

        public void Init(GameObject panel)
        {
            InitPanel(panel);
            _selectionTimer = 60;
            _turnCnt = 1;
            NetworkSynchronizer.Instance.Init();
        }
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

        public void BeginTimer()
        {
            StartCoroutine(CardSelectionTimer());
        }

        public void StopTimer()
        {
            _selectionTimer = 0;
            timerText.text = Math.Ceiling(_selectionTimer).ToString(CultureInfo.CurrentCulture);
        }

        #endregion




        #region Unity event functions

        void Awake()
        {
            
        }

        #endregion



        #region Coroutines

        public IEnumerator CardSelectionTimer()
        {
            turnText.text = $"TURN {_turnCnt}";
            _selectionTimer = 60f;
            _canSelect = true;
            scroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            
            while (_selectionTimer > 0)
            {
                _selectionTimer -= Time.deltaTime;
                timerText.text = Math.Ceiling(_selectionTimer).ToString(CultureInfo.CurrentCulture);
                yield return null;
            }
            
            StartCoroutine(WaitForReadToProcessCard());
        }

        public IEnumerator WaitForReadToProcessCard()
        {
            NetworkSynchronizer.Instance.ReadToRunTimer(false);
            NetworkSynchronizer.Instance.ReadToProcessCard(true);
            _canSelect = false;
            scroll3D.GetComponent<ScrollScript3D>().CloseScroll();
        
            while (!NetworkSynchronizer.Instance.hostReadToProcessCard.Value ||
                   !NetworkSynchronizer.Instance.clientReadyToProcessCard.Value)
            {
                yield return null;
            }

            StartCoroutine(ProcessCard());
        }

        IEnumerator ProcessCard()
        {
            for (int i = 0; i < 3; i++)
            {
                NetworkSynchronizer.Instance.RemoveCardFromList(0);
                GameObject.Find("GameSceneController").GetComponent<GameSceneController>().UpdateHostCardUI();
                GameObject.Find("GameSceneController").GetComponent<GameSceneController>().UpdateClientCardUI();
                
                GameObject obj1 = Instantiate(skillVfx, new Vector3(UnityEngine.Random.Range(0f, 7f), 1.5f, UnityEngine.Random.Range(0f, 7f)), Quaternion.identity);
                obj1.transform.localScale = new Vector3(4f, 4f, 4f);
                GameObject obj2 = Instantiate(skillVfx2, new Vector3(UnityEngine.Random.Range(6f, 14f), 1.5f, UnityEngine.Random.Range(0f, 7f)), Quaternion.identity);
                obj2.transform.localScale = new Vector3(4f, 4f, 4f);

                yield return new WaitForSeconds(2f);   
            }

            _turnCnt++;
            StartCoroutine(WaitForReadToRunTimer());
        }
        
        IEnumerator WaitForReadToRunTimer()
        {
            NetworkSynchronizer.Instance.ReadToRunTimer(true);
            NetworkSynchronizer.Instance.ReadToProcessCard(false);

            while (!NetworkSynchronizer.Instance.hostReadyToRunTimer.Value || 
                   !NetworkSynchronizer.Instance.clientReadyToRunTimer.Value)
            {
                yield return null;
            }
            
            GameManager.Instance.BeginTimer();
        }

        #endregion
    }
}