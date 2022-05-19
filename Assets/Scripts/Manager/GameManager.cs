using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Core;
using InGame;
using Scene;
using UI.Game;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Unity.Netcode;

namespace Manager
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region Private constants

        private const int DefaultTurnValue = 0;
        private const float DefaultTimerValue = 60;
        
        #endregion
        
        
        #region Private variables

        private NetworkSynchronizer _netSync;
        private PanelController _panelController; 
        private HUDGameUserInfoUIController _userInfoUIController;
        private HUDGameCardSelectionUIController _cardSelectionUIController;

        private int _turnValue;
        private float _timerValue;
        private bool _canSelect;

        #endregion


        #region Public variables
        
        public int turnValue => _turnValue;
        public float timerValue => _timerValue;
        public bool canSelect => _canSelect;
        
        #endregion


        #region Custom methods

        public void Init()
        {
            _netSync = GameObject.Find("NetworkSynchronizer").GetComponent<NetworkSynchronizer>();
            _panelController = GameObject.Find("GameSceneObjectController").GetComponent<PanelController>();
            
            GameObject uiController = GameObject.Find("GameSceneUIController");
            _userInfoUIController = uiController.GetComponent<HUDGameUserInfoUIController>();
            _cardSelectionUIController = uiController.GetComponent<HUDGameCardSelectionUIController>();

            _turnValue = DefaultTurnValue;
            _timerValue = DefaultTimerValue;
            _canSelect = false;
        }

        public void CheckTimerReady()
        {
            StartCoroutine(WaitForRunningTimer());
        }

        public void StopTimer()
        {
            _timerValue = 0;
            _userInfoUIController.UpdateTimerText();
        }

        #endregion




        #region Unity event functions

        private void Update()
        {
            Debug.Log($"Turn value is {_turnValue}");
        }
        

        #endregion



        #region Coroutines

        /// <summary>
        /// Wait for running timer
        /// Checks if both host and client are ready to run timer
        /// </summary>
        private IEnumerator WaitForRunningTimer()
        {
            Debug.Log("wait for running timer");
            _netSync.ReadyToRunTimer(true);
            _netSync.ReadyToProcessCards(false);

            while (!_netSync.BothReadyToRunTimer())
            {
                yield return null;
            }

            StartCoroutine(RunTimer());
        }

        /// <summary>
        /// Run card selection timer
        /// </summary>
        /// <returns></returns>
        private IEnumerator RunTimer()
        {
            _turnValue++;
            _timerValue = DefaultTimerValue;
            _canSelect = true;
            
            _userInfoUIController.UpdateTimerText();
            _userInfoUIController.UpdateTurnText();
            _cardSelectionUIController.OpenCardScroll();

            while (_timerValue > 0)
            {
                _timerValue -= Time.deltaTime;
                _userInfoUIController.UpdateTimerText();
                yield return null;
            }

            StartCoroutine(WaitForProcessingCards());
        }

        /// <summary>
        /// Wait for processing cards
        /// Checks if both host and client are ready to process cards in card list
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForProcessingCards()
        {
            Debug.Log("Wait for processing cards");
            _netSync.ReadyToRunTimer(false);
            _netSync.ReadyToProcessCards(true);
            
            _timerValue = 0;
            _canSelect = false;
            
            _userInfoUIController.UpdateTimerText();
            _cardSelectionUIController.CloseCardScroll();

            while (!_netSync.BothReadyToProcessCards())
            {
                yield return null;
            }

            StartCoroutine(ProcessCards()); 
        }

        /// <summary>
        /// Process cards in card list
        /// </summary>
        /// <returns></returns>
        private IEnumerator ProcessCards()
        {
            Debug.Log("Process cards");
            GameObject vfx = GameObject.Find("EvilDeath");
            // Important logic will come here
            // process logic
            for (int i = 0; i < 3; i++)
            {
                string range = "10101 00000 10101 00000 10101";
                BitMask.Bits30Field fieldRange = BitMask.CvtBits25ToBits30(new BitMask.Bits25Field(range));
                BitMask.ShiftBits30(ref fieldRange, -3, 0);

                int mask = 1 << 29;
                for (int j = 0; j < 30; j++)
                {
                    if ((fieldRange.element & mask) > 0)
                    {
                        _panelController.ChangeColor(j);
                    }
                    
                    mask = mask >> 1;
                }
                Instantiate(vfx, new Vector3(0, 0, 0), Quaternion.identity);
                yield return new WaitForSeconds(2f);
            }

            // if the game is not ended,
            StartCoroutine(WaitForRunningTimer());
        }
        
        
        /*IEnumerator ProcessCard()
        {
            for (int i = 0; i < 3; i++)
            {
                NetworkSynchronizer.Instance.RemoveCardFromList(0);
                GameObject.Find("GameSceneUIController").GetComponent<GameSceneController>().UpdateHostCardUI();
                GameObject.Find("GameSceneUIController").GetComponent<GameSceneController>().UpdateClientCardUI();

                GameObject obj1 = Instantiate(skillVfx, new Vector3(UnityEngine.Random.Range(0f, 7f), 1.5f, UnityEngine.Random.Range(0f, 7f)), Quaternion.identity);
                obj1.transform.localScale = new Vector3(4f, 4f, 4f);
                GameObject obj2 = Instantiate(skillVfx2, new Vector3(UnityEngine.Random.Range(6f, 14f), 1.5f, UnityEngine.Random.Range(0f, 7f)), Quaternion.identity);
                obj2.transform.localScale = new Vector3(4f, 4f, 4f);

                yield return new WaitForSeconds(2f);
            }

            // for (int i = 0; i < 3; i++)
            // {
            //     int hostCardCode = NetworkSynchronizer.Instance.GetHostCardFromList(0);
            //     int clientCardCode = NetworkSynchronizer.Instance.GetClientCardFromList(0);
            //
            //     NetworkSynchronizer.Instance.RemoveCardFromList(0);
            //     GameObject.Find("GameSceneController").GetComponent<GameSceneController>().UpdateHostCardUI();
            //     GameObject.Find("GameSceneController").GetComponent<GameSceneController>().UpdateClientCardUI();
            //
            //
            //     if (NetworkManager.Singleton.IsServer)
            //     {
            //         // // @todo - get range from table
            //         // int[,] hostRange = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 1, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
            //         // int[,] clientRange = new int[,] { { 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0 } };
            //         // NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<WizardController>().Move(hostRange);
            //         // NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(NetworkManager.Singleton.ConnectedClientsIds[1]).GetComponent<WizardController>().Move(clientRange);
            //
            //         // Test Code
            //         NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<SkillControllerTest>().Play(101000000);
            //         NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(NetworkManager.Singleton.ConnectedClientsIds[1]).GetComponent<SkillControllerTest>().Play(101000001);
            //     }
            //
            //     yield return new WaitForSeconds(2f);
            // }
            
        }*/

        #endregion
    }
}