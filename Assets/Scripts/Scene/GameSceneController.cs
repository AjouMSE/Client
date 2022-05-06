using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Manager;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scene
{
    public class GameSceneController : MonoBehaviour
    {
        public GameObject cam;
        public GameObject panel;
        public GameObject scroll3D;

        public GameObject[] selectedCards;
        public GameObject[] selectedHostileCards;
        public Sprite[] cardImgs;
        public GameObject[] vfxs;

        private List<GameObject> tmp = new List<GameObject>();
        private List<GameObject> tmp2 = new List<GameObject>();

        private float x = -120;

        void Init()
        {
            Application.targetFrameRate = 60;
            GameManager.Instance.InitPanel(panel);
        }
        
        void UpdateSelectedCardUI()
        {
            List<int> cards = GameManager.Instance.selectedCardList;
            for (int i = 0; i < cards.Count; i++)
            {
                selectedCards[i].GetComponent<Image>().sprite = cardImgs[cards[i]];
                Color color = selectedCards[i].GetComponent<Image>().color;
                color.a = 1;
                selectedCards[i].GetComponent<Image>().color = color;
            }

            for (int i = cards.Count; i < 3; i++)
            {
                Color color = selectedCards[i].GetComponent<Image>().color;
                color.a = 0;
                selectedCards[i].GetComponent<Image>().color = color;
            }
        }

        void UpdateSelectedHostileCardUI()
        {
            List<int> hostileCards = GameManager.Instance.selectedCardListHostile;
            if (hostileCards.Count == 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    selectedHostileCards[i].GetComponent<Image>().sprite = cardImgs[hostileCards[i]];
                    Color color = selectedHostileCards[i].GetComponent<Image>().color;
                    color.a = 1;
                    selectedHostileCards[i].GetComponent<Image>().color = color;
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    Color color = selectedHostileCards[i].GetComponent<Image>().color;
                    color.a = 0;
                    selectedHostileCards[i].GetComponent<Image>().color = color;
                }
            }
        }

        void ProcessCards()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                StartCoroutine(Vfxs());
                StartCoroutine(Vfxs2());   
            }
        }

        IEnumerator Vfxs()
        {
            for (int i = 0; i < GameManager.Instance.selectedCardList.Count; i++)
            {
                tmp.Add(Instantiate(vfxs[GameManager.Instance.selectedCardList[i]],
                    NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().transform.position
                    + new Vector3(5, 0, 0), Quaternion.identity));
                GameManager.Instance.selectedCardList.RemoveAt(0);
                
                int[] cards = GameManager.Instance.selectedCardList.ToArray();
                if(NetworkManager.Singleton.IsServer)
                    NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<WizardController>().SyncCardClientRpc(cards);
                else
                    NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<WizardController>().SyncCardServerRpc(cards);

                yield return new WaitForSeconds(1.5f);
                
                if (tmp.Count > 0)
                {
                    foreach (GameObject obj in tmp)
                    {
                        Destroy(obj);
                        tmp.RemoveAt(0);
                    }
                }
            }
        }

        IEnumerator Vfxs2()
        {
            for (int i = 0; i < GameManager.Instance.selectedCardListHostile.Count; i++)
            {
                tmp2.Add(Instantiate(vfxs[GameManager.Instance.selectedCardListHostile[i]],
                    NetworkManager.Singleton.SpawnManager.GetClientOwnedObjects(NetworkManager.Singleton.ConnectedClientsIds[0])[0].transform.position
                    + new Vector3(-5, 0), Quaternion.identity));
                GameManager.Instance.selectedCardListHostile.RemoveAt(0);
                
                int[] cards = GameManager.Instance.selectedCardListHostile.ToArray();
                if(NetworkManager.Singleton.IsServer)
                    NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<WizardController>().SyncCardClientRpc(cards);
                else
                    NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<WizardController>().SyncCardServerRpc(cards);

                yield return new WaitForSeconds(1.5f);
                
                if (tmp2.Count > 0)
                {
                    foreach (GameObject obj in tmp)
                    {
                        Destroy(obj);
                        tmp2.RemoveAt(0);
                    }
                }
            }
        }

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (x < 30)
            {
                cam.transform.localEulerAngles = new Vector3(x, 0, 0);
                x += 2.5f;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                scroll3D.GetComponent<ScrollScript3D>().OpenOrCloseScroll();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                NetworkManager.Singleton.StartHost();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                NetworkManager.Singleton.StartClient();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                ProcessCards();
            }
            
            UpdateSelectedCardUI();
            UpdateSelectedHostileCardUI();
            
            Debug.Log(GameManager.Instance.selectedCardList.Count + " / " + GameManager.Instance.selectedCardListHostile.Count);
            
        }
    }
}
