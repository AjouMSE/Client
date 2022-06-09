using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Utils;

namespace UI.Lobby.LeaderBoard
{
    public class LeaderBoardUIController : MonoBehaviour
    {
        #region Private constants

        private const string PageReqPath = "/ranking/leader-board";
        private const int MaxUserInfoRowCnt = 10;

        #endregion


        #region Private variables

        [Header("Page Number Text")] 
        [SerializeField] private Text textPageNum;

        [Header("Page Buttons")] 
        [SerializeField] private Button buttonPageLeft;
        [SerializeField] private Button buttonPageRight;

        [Header("User Info Rows")] [SerializeField]
        private LeaderBoardUserInfoRowController[] userInfoRowControllers;

        [Header("3D Scroll Menu UI")] 
        [SerializeField] private ScrollScript3D scroll3D;

        private CanvasGroup _leaderBoardCanvasGroup;

        private int _pageIdx = 1;
        private int _maxPageNum;
        private long _totalUserCnt;

        private enum ButtonType
        {
            Left = 0,
            Right = 1
        }

        #endregion


        #region Unity event methods

        private void Start()
        {
            Init();
        }

        #endregion


        #region Private methods

        private void Init()
        {
            _leaderBoardCanvasGroup = GetComponent<CanvasGroup>();
            textPageNum.text = $"Page {_pageIdx.ToString()}.";
            HttpRequestManager.Instance.Get($"{PageReqPath}?page={_pageIdx.ToString()}", LeaderBoardReqCallback);
        }

        /// <summary>
        /// Check that user can move the page
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool CanMove(int type)
        {
            ButtonType enumButtonType = (ButtonType)type;

            switch (enumButtonType)
            {
                case ButtonType.Left:
                    return _pageIdx > 1;

                case ButtonType.Right:
                    return _pageIdx < _maxPageNum;

                default:
                    return false;
            }
        }

        #endregion


        #region Callbacks

        /// <summary>
        /// Leader board http request callback
        /// </summary>
        /// <param name="req"></param>
        private void LeaderBoardReqCallback(UnityWebRequest req)
        {
            // parse packet
            string json = req.downloadHandler.text;
            Packet.UserList userList = JsonUtility.FromJson<Packet.UserList>(json);

            // get maximum page num
            _totalUserCnt = userList.totalCount;
            _maxPageNum = (((int)_totalUserCnt - 1) / MaxUserInfoRowCnt) + 1;

            // Check that user can move the page
            buttonPageLeft.gameObject.SetActive(CanMove(0));
            buttonPageRight.gameObject.SetActive(CanMove(1));

            // Set UI data
            for (int i = 0; i < userList.users.Count; i++)
            {
                Packet.User user = userList.users[i];
                userInfoRowControllers[i].gameObject.SetActive(true);
                userInfoRowControllers[i].SetUserData(user);
            }

            for (int i = userList.users.Count; i < MaxUserInfoRowCnt; i++)
            {
                userInfoRowControllers[i].gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Page button click callback
        /// </summary>
        /// <param name="type"></param>
        public void OnPageButtonClick(int type)
        {
            if (CanMove(type))
            {
                _pageIdx += type == 0 ? -1 : 1;
                textPageNum.text = $"Page {_pageIdx.ToString()}.";
                HttpRequestManager.Instance.Get($"{PageReqPath}?page={_pageIdx.ToString()}", LeaderBoardReqCallback);
            }

            // Check that user can move the page
            buttonPageLeft.gameObject.SetActive(CanMove(0));
            buttonPageRight.gameObject.SetActive(CanMove(1));
        }

        /// <summary>
        /// Back button callback
        /// </summary>
        public void OnLeaderBoardBackBtnClick()
        {
            scroll3D.GetComponent<ScrollScript3D>().OpenScroll();
            UIManager.Instance.Fade(UIManager.FadeType.FadeOut, _leaderBoardCanvasGroup,
                UIManager.LobbyMenuFadeInOutDuration);
        }

        #endregion
    }
}