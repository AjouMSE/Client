using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace DataTable
{
    public partial class TableDatas : MonoSingleton<TableDatas>
    {
        public Dictionary<int, CardData> cardDatas = new Dictionary<int, CardData>();
        public Dictionary<int, TierData> tierDatas = new Dictionary<int, TierData>();
        public Dictionary<int, SpecialData> specialDatas = new Dictionary<int, SpecialData>();

        public CardData GetCardData(int code)
        {
            return GetData(cardDatas, code);
        }

        public TierData GetTierData(int code)
        {
            return GetData(tierDatas, code);
        }

        public SpecialData GetSpecialData(int code)
        {
            return GetData(specialDatas, code);
        }

        public T GetData<T>(Dictionary<int, T> dic, int code)
        {
#if UNITY_EDITOR
            if (false == dic.ContainsKey(code))
            {
                Dictionary<int, T>.Enumerator etor = dic.GetEnumerator();
                etor.MoveNext();
                int table = (etor.Current.Value as TableDataBase).table;
                if (0 == code)
                    UnityEngine.Debug.LogWarning(
                        "Table code[" + table + "]" + "   Try get data zero code[" + code + "]");
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
            tierDatas.Clear();
            specialDatas.Clear();
        }

        public override void Init()
        {
        }
    }
}