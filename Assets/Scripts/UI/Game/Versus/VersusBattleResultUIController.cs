using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using Utils;

namespace UI.Game.Versus
{
    public class VersusBattleResultUIController : MonoBehaviour
    {
        #region Private variables

        [SerializeField] private bool isHostInfo;

        private Text[] _battleResultTexts;

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
            _battleResultTexts = GetComponentsInChildren<Text>();
        }

        #endregion


        #region Public methods

        public void SetBattleResult(Consts.BattleResult hostResult, Consts.BattleResult clientResult)
        {
            if (isHostInfo)
            {
                _battleResultTexts[0].text = (hostResult == Consts.BattleResult.WIN)
                    ? "WIN"
                    : (clientResult == Consts.BattleResult.WIN ? "LOSE" : "DRAW");
                _battleResultTexts[1].text = (hostResult == Consts.BattleResult.WIN)
                    ? "+10"
                    : (clientResult == Consts.BattleResult.WIN ? "-8" : "+0");
            }
            else
            {
                _battleResultTexts[0].text = (clientResult == Consts.BattleResult.WIN)
                    ? "WIN"
                    : (hostResult == Consts.BattleResult.WIN ? "LOSE" : "DRAW");
                _battleResultTexts[1].text = (clientResult == Consts.BattleResult.WIN)
                    ? "+10"
                    : (hostResult == Consts.BattleResult.WIN ? "-8" : "+0");
            }
        }

        #endregion
    }
}