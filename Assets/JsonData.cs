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
    [ReadOnly]
    public List<StoreDiamondData> StoreDiamondDatas;
    [ReadOnly] 
    public List<StorePackageData> StorePackageDatas;
    
    [Button]
    private void Awake()
    {
        Instance = this;
    }

#if UNITY_EDITOR
    [Button]
    private void ImportScrumbEventData(string json)
    {
        // string json = File.ReadAllText(Application.dataPath + "/Resources/JSON/" + "ScrumbEventData" + ".json"); 
        Dictionary<int, ScrumbEventData> ScrumbEventDatasDict = JsonConvert.DeserializeObject<Dictionary<int, ScrumbEventData>>(json);

        ScrumbEventDatas = new List<ScrumbEventData>();
        foreach (var chain in ScrumbEventDatasDict)
        {
            ScrumbEventDatas.Add(chain.Value);
        }
    }
    [Button]
    private void ImportStoreDiamondData(string json)
    {
        // string json = File.ReadAllText(Application.dataPath + "/Resources/JSON/" + "StoreDiamondData" + ".json"); 
        Dictionary<int, StoreDiamondData> tmp = JsonConvert.DeserializeObject<Dictionary<int, StoreDiamondData>>(json);

        StoreDiamondDatas = new List<StoreDiamondData>();
        foreach (var chain in tmp)
        {
            StoreDiamondDatas.Add(chain.Value);
        }
    }
    [Button]
    private void ImportStorePackageData(string json)
    {
        // string json = File.ReadAllText(Application.dataPath + "/Resources/JSON/" + "StoreDiamondData" + ".json"); 
        Dictionary<int, StorePackageData> tmp = JsonConvert.DeserializeObject<Dictionary<int, StorePackageData>>(json);

        StorePackageDatas = new List<StorePackageData>();
        foreach (var chain in tmp)
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
    
    //------ENUMS------//
    
    public enum PackageType
    {
        oneTime, weekly
    }

    public enum NeedItemType
    {
        KRW, diamond
    }
}
