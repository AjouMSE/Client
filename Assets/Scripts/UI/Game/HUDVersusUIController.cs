using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Game
{
    public class HUDVersusUIController : MonoBehaviour
    {
        #region Private constants 
        
        private readonly string HudNameVersus = "HUD_Versus";
        private readonly string HudNameUserInfo = "HUD_UserInfo";
        private readonly string HudNameCardSelection = "HUD_CardSelection";
        
        private readonly float MinMainCameraAngle = -150;
        private readonly float MaxMainCameraAngle = 30;
        private readonly float RotationScale = 2f;
        
        #endregion 
        
        #region Private variables

        [Header("Camera")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera hudCamera;
        
        [Header("Text Information")]
        [SerializeField] private Text versusText;
        [SerializeField] private Text[] hostInfoTextArr;
        [SerializeField] private Text[] clientInfoTextArr;
        
        [Header("3D Scroll UI")]
        [SerializeField] private GameObject scroll3D;

        private GameObject _hudVersus, _hudUserInfo, _hudCardSelection;
        private float _mainCameraXAngle;
        
        #endregion
        
        #region Custom methods
        
        private void InitVersusIntoText()
        {
            Packet.User host, client;

            if (NetworkManager.Singleton.IsHost)
            {
                host = UserManager.Instance.User;
                client = UserManager.Instance.Hostile;
            }
            else
            {
                host = UserManager.Instance.Hostile;
                client = UserManager.Instance.User;
            }
            
            hostInfoTextArr[0].text = host.nickname;
            hostInfoTextArr[1].text = host.win.ToString();
            hostInfoTextArr[2].text = host.lose.ToString();
            hostInfoTextArr[3].text = host.draw.ToString();
            hostInfoTextArr[4].text = host.ranking.ToString();

            clientInfoTextArr[0].text = client.nickname;
            clientInfoTextArr[1].text = client.win.ToString();
            clientInfoTextArr[2].text = client.lose.ToString();
            clientInfoTextArr[3].text = client.draw.ToString();
            clientInfoTextArr[4].text = client.ranking.ToString();
        }
        
        private void Init()
        {
            _mainCameraXAngle = MinMainCameraAngle;
            
            _hudVersus = hudCamera.transform.Find(HudNameVersus).gameObject;
            _hudUserInfo = hudCamera.transform.Find(HudNameUserInfo).gameObject;
            _hudCardSelection = hudCamera.transform.Find(HudNameCardSelection).gameObject;

            if (UserManager.Instance.IsHost) 
                NetworkManager.Singleton.StartHost();
            else 
                NetworkManager.Singleton.StartClient();

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
            versusText.fontSize = 125;
            yield return new WaitForSeconds(2.5f);

            versusText.fontSize = 100;
            versusText.text = "Are you\nReady?";
            yield return new WaitForSeconds(2.5f);
            
            _hudVersus.SetActive(false);
            StartCoroutine(RotateCamera());
        }

        IEnumerator RotateCamera()
        {
            // Camera rotation effect
            while (_mainCameraXAngle < MaxMainCameraAngle)
            {
                _mainCameraXAngle += RotationScale;
                mainCamera.transform.localEulerAngles = new Vector3(_mainCameraXAngle, 0, 0);
                yield return null;
            }
            
            // Set user info, card selection ui activation to true 
            _hudUserInfo.SetActive(true);
            _hudCardSelection.SetActive(true);
            
            GameManager.Instance.timerText = GameObject.Find("Txt_Timer").GetComponent<Text>();
            GameManager.Instance.turnText = GameObject.Find("Txt_Turn").GetComponent<Text>();
            GameManager.Instance.scroll3D = GameObject.Find("ScrollObject_Short_WithScript");
            GameManager.Instance.skillVfx =  GameObject.Find("EvilDeath");
            GameManager.Instance.skillVfx2 = GameObject.Find("PoisonDeath");
            
            // Wait until opponent is ready to run timer
            StartCoroutine(WaitForReadToRunTimer());
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
