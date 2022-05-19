using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Core;
using InGame;
using Scene;
using UI.Game;
using Unity.Collections;
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

        private void ProcessRange(BitMask.Bits30Field fieldRange, int vfxId)
        {
            int mask = 0x20000000;
            for (int j = 0; j < 30; j++)
            {
                if ((fieldRange.element & mask) > 0)
                {
                    _panelController.ChangeColor(j, vfxId);
                }
                    
                mask = mask >> 1;
            }
            
            
        }

        #endregion




        #region Unity event functions
        

        #endregion



        #region Coroutines

        /// <summary>
        /// Wait for running timer
        /// Checks if both host and client are ready to run timer
        /// </summary>
        private IEnumerator WaitForRunningTimer()
        {
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
            // Important logic will come here
            string range = "10101 00000 10101 00000 10101";
            BitMask.Bits30Field fieldRange = BitMask.CvtBits25ToBits30(new BitMask.Bits25Field(range));
            
            // process logic
            for (int i = 0; i < 3; i++)
            {
                BitMask.ShiftBits30(ref fieldRange, -3, 0);
                ProcessRange(fieldRange, 1);
                _netSync.PopCardFromHostCardList();
                yield return new WaitForSeconds(2f);

                BitMask.ShiftBits30(ref fieldRange, 2, 0);
                ProcessRange(fieldRange, 2);
                _netSync.PopCardFromClientCardList();
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