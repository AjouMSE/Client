using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public partial class TableLoader : MonoSingleton<TableLoader>
{
    private IEnumerator LoadTableDataProcess()
    {
        _CompleteStep = 2;

#if UNITY_EDITOR
        Load_table_skill_card("101_table_skill_card");
#else
		yield return Load_table_skill_card("101_table_skill_card");
#endif

        yield return Resources.UnloadUnusedAssets();
        System.GC.Collect();


        if (m_isLoadFail)
            yield break;

        if (_tableHashs != null)
            _tableHashs.Clear();

        m_Clue = string.Empty;

        if (_CurrentStep < _CompleteStep)
        {
            _CurrentStep = _CompleteStep - 1;

            yield return null;
            UpdateStep();
        }

        if (_CompleteEvent != null)
        {
            yield return null;

            _CompleteEvent();
            _CompleteEvent = null;
        }

        yield return null;

        _CurrentStep = 0;
        _CompleteStep = 0;

        _UpdateEvent = null;
    }

#if UNITY_EDITOR
    private void Load_table_skill_card(string fileName)
#else
	private IEnumerator Load_table_skill_card(string fileName)
#endif
    {
        List<object> dataList = GetDataList(fileName);
        if (dataList == null)
        {
            Debug.Log(GetType().Name + " Load_table_skill_card() : data list is null" + fileName);
        }
        else
        {
            for (int i = 0, count = dataList.Count; i < count; ++i)
            {
                List<object> elem = dataList[i] as List<object>;
                if (elem == null || elem.Count == 0) break;

                CardData data = CardData.Create(elem);
                TableDatas.Instance.cardDatas.Add(data.code, data);
            }
        }
#if !UNITY_EDITOR
		yield break;
#endif
    }

}
