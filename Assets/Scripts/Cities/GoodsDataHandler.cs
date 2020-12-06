using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsDataHandler : Singleton<GoodsDataHandler>
{
    Dictionary<string,GoodsData> data = new Dictionary<string,GoodsData>();
    Dictionary<string, GoodsData> Data {
        get {
            if (data.Count < 1)
                LoadGoodsData();

            return data;
        }
    }

    public void LoadGoodsData()
    {
        int count = 0;
        foreach (GoodsData goods in Resources.LoadAll("Goods", typeof(GoodsData)))
        {
            data.Add(goods.name.ToLower(), goods);
            count++;
        }

        Debug.LogWarning("Loaded " + count + " goods from resources");
    }

    public GoodsData GetDataFor(string name)
    {
        if (Data.ContainsKey(name.ToLower()))
            return Data[name.ToLower()];

        Debug.LogError("please crete data for " + name);
        return null;
    }
}
