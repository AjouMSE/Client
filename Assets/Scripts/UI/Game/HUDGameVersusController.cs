using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class HUDGameVersusController : MonoBehaviour
    {
        #region Private variables

        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject hudVersus, hudUserInfo, hudCardSelection;
        [SerializeField] private Text versusText;
        [SerializeField] private Text[] hostInfoTextArr;
        [SerializeField] private Text[] clientInfoTextArr;
        [SerializeField] private GameObject scroll3D;

        private float x = -120;
        
        #endregion
        
        #region Custom methods

        private void Swap<T>(ref T[] arr1, ref T[] arr2)
        {
            T[] tmp = arr1;
            arr1 = arr2;
            arr2 = tmp;
        }

        private void InitVersusIntoText()
        {
            hostInfoTextArr[0].text = UserManager.Instance.User.nickname;
            hostInfoTextArr[1].text = UserManager.Instance.User.win.ToString();
            hostInfoTextArr[2].text = UserManager.Instance.User.lose.ToString();
            hostInfoTextArr[3].text = UserManager.Instance.User.draw.ToString();
            hostInfoTextArr[4].text = UserManager.Instance.User.ranking.ToString();

            clientInfoTextArr[0].text = UserManager.Instance.Hostile.nickname;
            clientInfoTextArr[1].text = UserManager.Instance.Hostile.win.ToString();
            clientInfoTextArr[2].text = UserManager.Instance.Hostile.lose.ToString();
            clientInfoTextArr[3].text = UserManager.Instance.Hostile.draw.ToString();
            clientInfoTextArr[4].text = UserManager.Instance.Hostile.ranking.ToString();
        }
        

        private void Init()
        {
            if (!UserManager.Instance.IsHost) 
            {
                // swap
                Swap<Text>(ref hostInfoTextArr, ref clientInfoTextArr);
                NetworkManager.Singleton.StartClient();
            }
            else
            {
                NetworkManager.Singleton.StartHost();
            }
            
            InitVersusIntoText();
            StartCoroutine(ShowVersus());
        }

        #endregion
        
        
        
        #region Unity event methods
        
        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        #endregion



        #region Coroutines

        IEnumerator ShowVersus()
        {
            versusText.text = "VS";
            versusText.fontSize = 125;
            yield return new WaitForSeconds(3f);

            versusText.fontSize = 100;
            versusText.text = "Are you\nReady?";
            yield return new WaitForSeconds(2f);
            
            hudVersus.SetActive(false);
            StartCoroutine(RotateCamera());
        }

        IEnumerator RotateCamera()
        {
            hudUserInfo.SetActive(true);
            hudCardSelection.SetActive(true);
            GameManager.Instance.timerText = GameObject.Find("Txt_Timer").GetComponent<Text>();
            GameManager.Instance.turnText = GameObject.Find("Txt_Turn").GetComponent<Text>();
            GameManager.Instance.scroll3D = GameObject.Find("ScrollObject_Short_WithScript");
            GameManager.Instance.skillVfx =  GameObject.Find("EvilDeath");
            GameManager.Instance.skillVfx2 = GameObject.Find("PoisonDeath");
                
            // Wait until opponent is ready to run timer
            StartCoroutine(WaitForReadToRunTimer());

            // Rotate Camera
            while (x < 30)
            {
                mainCamera.transform.localEulerAngles = new Vector3(x, 0, 0);
                x += 1.5f;
                yield return null;
            }
        }

        IEnumerator WaitForReadToRunTimer()
        {
            NetworkSynchronizer.Instance.ReadToRunTimer(true);

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
