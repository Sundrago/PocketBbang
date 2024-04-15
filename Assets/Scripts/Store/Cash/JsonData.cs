using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

public class JsonData : MonoBehaviour
{
    public static JsonData Instance;
    [ReadOnly]
    public List<ScrumbEventData> ScrumbEventDatas;
    public Dictionary<int, ScrumbEventData> ScrumbEventDatasDict;
    [ReadOnly]
    public List<StoreDiamondData> StoreDiamondDatas;
    public Dictionary<int, StoreDiamondData> StoreDiamondDataDict;
    [ReadOnly] 
    public List<StorePackageData> StorePackageDatas;
    public Dictionary<int, StorePackageData> StorePackageDataDict;
    
    [Button]
    private void Awake()
    {
        Instance = this;
    }

#if UNITY_EDITOR
    [Button]
    private void ImportScrumbEventData(string json)
    {
        ScrumbEventDatasDict = JsonConvert.DeserializeObject<Dictionary<int, ScrumbEventData>>(json);

        ScrumbEventDatas = new List<ScrumbEventData>();
        foreach (var chain in ScrumbEventDatasDict)
        {
            ScrumbEventDatas.Add(chain.Value);
        }
    }
    [Button]
    private void ImportStoreDiamondData(string json)
    {
        StoreDiamondDataDict = JsonConvert.DeserializeObject<Dictionary<int, StoreDiamondData>>(json);

        StoreDiamondDatas = new List<StoreDiamondData>();
        foreach (var chain in StoreDiamondDataDict)
        {
            StoreDiamondDatas.Add(chain.Value);
        }
    }
    [Button]
    private void ImportStorePackageData(string json)
    {
        Dictionary<int, StorePackageData> StorePackageDataDict = JsonConvert.DeserializeObject<Dictionary<int, StorePackageData>>(json);

        StorePackageDatas = new List<StorePackageData>();
        foreach (var chain in StorePackageDataDict)
        {
            StorePackageDatas.Add(chain.Value);
        }
    }
#endif
    
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
    
    public enum PackageType
    {
        oneTime, weekly
    }

    public enum NeedItemType
    {
        KRW, diamond
    }
}
