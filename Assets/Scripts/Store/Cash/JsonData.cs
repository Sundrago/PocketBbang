using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

public class JsonData : MonoBehaviour
{
    public enum NeedItemType
    {
        KRW,
        diamond
    }

    public enum PackageType
    {
        oneTime,
        weekly
    }

    public static JsonData Instance;

    [ReadOnly] public List<ScrumbEventData> ScrumbEventDatas;

    [ReadOnly] public List<StoreDiamondData> StoreDiamondDatas;

    [ReadOnly] public List<StorePackageData> StorePackageDatas;

    public Dictionary<int, ScrumbEventData> ScrumbEventDatasDict;
    public Dictionary<int, StoreDiamondData> StoreDiamondDataDict;
    public Dictionary<int, StorePackageData> StorePackageDataDict;

    [Button]
    private void Awake()
    {
        Instance = this;
    }

    [Serializable]
    public class ScrumbEventData
    {
        public int percentage;
        public int itemIdx;
        public int rewardItemAmt_min;
        public int rewardItemAmt_max;
    }

    [Serializable]
    public class StoreDiamondData
    {
        public int amount;
        public int price;
    }

    [Serializable]
    public class StorePackageData
    {
        public PackageType packageType;
        public NeedItemType NeedItemType;
        public int needAmt;
        public string name, descr;
        public int[] itemCode, itemAmt;
    }

#if UNITY_EDITOR
    [Button]
    private void ImportScrumbEventData(string json)
    {
        ScrumbEventDatasDict = JsonConvert.DeserializeObject<Dictionary<int, ScrumbEventData>>(json);

        ScrumbEventDatas = new List<ScrumbEventData>();
        foreach (var chain in ScrumbEventDatasDict) ScrumbEventDatas.Add(chain.Value);
    }

    [Button]
    private void ImportStoreDiamondData(string json)
    {
        StoreDiamondDataDict = JsonConvert.DeserializeObject<Dictionary<int, StoreDiamondData>>(json);

        StoreDiamondDatas = new List<StoreDiamondData>();
        foreach (var chain in StoreDiamondDataDict) StoreDiamondDatas.Add(chain.Value);
    }

    [Button]
    private void ImportStorePackageData(string json)
    {
        var StorePackageDataDict = JsonConvert.DeserializeObject<Dictionary<int, StorePackageData>>(json);

        StorePackageDatas = new List<StorePackageData>();
        foreach (var chain in StorePackageDataDict) StorePackageDatas.Add(chain.Value);
    }
#endif
}