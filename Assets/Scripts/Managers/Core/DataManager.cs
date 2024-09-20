using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.Stat> StatDict { get; private set; } = new Dictionary<int, Data.Stat>();
    public void Init()
    {
        //경로를 항상 생각해야함
        //빌드업 하던중에 하드코딩이라서 새롭게 만듬
        //TextAsset textAsset = Managers.Resources.Load<TextAsset>($"Data/StatData");
        //StatData data = JsonUtility.FromJson<StatData>(textAsset.text);
        //foreach (Stat stat in data.stats)
        //{
        //    statDict.Add(stat.level, stat);
        //}

        StatDict = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resources.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
