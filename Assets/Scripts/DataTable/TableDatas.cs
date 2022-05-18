using System.Collections.Generic;
using Utils;

public partial class TableDatas : MonoSingleton<TableDatas>
{
    public Dictionary<int, CardData> cardDatas = new Dictionary<int, CardData>();

    public CardData GetCardData(int code) { return GetData(cardDatas, code); }

    public T GetData<T>(Dictionary<int, T> dic, int code)
    {
#if UNITY_EDITOR
        if (false == dic.ContainsKey(code))
        {
            Dictionary<int, T>.Enumerator etor = dic.GetEnumerator();
            etor.MoveNext();
            int table = (etor.Current.Value as TableDataBase).table;
            if (0 == code)
                UnityEngine.Debug.LogWarning("Table code[" + table + "]" + "   Try get data zero code[" + code + "]");
            else
                UnityEngine.Debug.LogError(table + "테이블에 " + code + "코드가 없습니다");
            return default(T);
        }
        return dic[code];
#else
		return code > 0 && dic.ContainsKey(code) ? dic[code] : default(T);
#endif
    }
    public void ClearAll()
    {
        cardDatas.Clear();
    }
}
