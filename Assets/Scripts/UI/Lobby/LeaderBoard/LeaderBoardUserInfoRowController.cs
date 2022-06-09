using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Lobby.LeaderBoard
{
    public class LeaderBoardUserInfoRowController : MonoBehaviour
    {
        #region Private constants

        private const string TextNameNickname = "Txt_LeaderBoard_Nickname";
        private const string TextNameRanking = "Txt_LeaderBoard_Ranking";
        private const string TextNameWin = "Txt_LeaderBoard_Win";
        private const string TextNameLose = "Txt_LeaderBoard_Lose";
        private const string TextNameDraw = "Txt_LeaderBoard_Draw";
        private const string TextNameScore = "Txt_LeaderBoard_Score";

        #endregion


        #region Private variables

        private Text _nicknameText;
        private Text _rankingText;
        private Text _winText;
        private Text _loseText;
        private Text _drawText;
        private Text _scoreText;

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
            _nicknameText = CustomUtils.FindComponentByName<Text>(gameObject, TextNameNickname);
            _rankingText = CustomUtils.FindComponentByName<Text>(gameObject, TextNameRanking);
            _winText = CustomUtils.FindComponentByName<Text>(gameObject, TextNameWin);
            _loseText = CustomUtils.FindComponentByName<Text>(gameObject, TextNameLose);
            _drawText = CustomUtils.FindComponentByName<Text>(gameObject, TextNameDraw);
            _scoreText = CustomUtils.FindComponentByName<Text>(gameObject, TextNameScore);
        }

        #endregion


        #region Public methods

        public void SetUserData(Packet.User user)
        {
            _nicknameText.text = user.nickname;
            _rankingText.text = user.ranking.ToString();
            _winText.text = user.win.ToString();
            _loseText.text = user.lose.ToString();
            _drawText.text = user.draw.ToString();
            _scoreText.text = user.score.ToString();
        }

        #endregion
    }
}