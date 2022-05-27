using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public partial class CardData : TableDataBase
{
    public const int Table = 101;

    public string text;
    public int type;
    public string range;
    public int cost;
    public int priority;
    public int value;
    public string effect;
    public string icon;

    public static CardData Create(List<object> elements)
    {
        CardData data = new CardData();

        if (elements != null && elements.Count > 0)
        {
            int index = 0;

            data.code = Convert.ToInt32(elements[index]); ++index;
            data.text = Convert.ToString(elements[index]); ++index;
            data.type = Convert.ToInt32(elements[index]); ++index;
            data.range = Convert.ToString(elements[index]); ++index;
            data.cost = Convert.ToInt32(elements[index]); ++index;
            data.priority = Convert.ToInt32(elements[index]); ++index;
            data.value = Convert.ToInt32(elements[index]); ++index;
            data.effect = Convert.ToString(elements[index]); ++index;
            data.icon = Convert.ToString(elements[index]); ++index;
        }

        data.SetEnc();
        return data;
    }
}

public partial class TierData : TableDataBase
{
    public const int Table = 201;

    public string tier;
    public int required;

    public static TierData Create(List<object> elements)
    {
        TierData data = new TierData();

        if (elements != null && elements.Count > 0)
        {
            int index = 0;

            data.code = Convert.ToInt32(elements[index]); ++index;
            data.tier = Convert.ToString(elements[index]); ++index;
            data.required = Convert.ToInt32(elements[index]); ++index;
        }

        data.SetEnc();
        return data;
    }
}

