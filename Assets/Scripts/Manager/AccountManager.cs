using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // @todo - 로그인 성공 후 table load 하도록 수정 필요
        TableLoader.Instance.LoadTableData();

        // Table usage Example
        foreach (KeyValuePair<int, CardData> cardData in TableDatas.Instance.cardDatas)
        {
            Debug.Log(cardData.Value.code);
            Debug.Log(cardData.Value.text);
        }

        CardData data = TableDatas.Instance.GetCardData(101000000);
        Debug.Log(data.code);
        Debug.Log(data.cost);
        Debug.Log(data.range);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
