using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bbang_showroom : SerializedMonoBehaviour
{
    [SerializeField] GameObject[] bbang = new GameObject[4];
    [SerializeField] GameObject bbang_holder;
    [SerializeField] GameObject placePosition, bbangboy_ui;
    [SerializeField] GameObject scrum;
    [SerializeField] Heart_Control heart;
    [SerializeField] BbangBoyContol bbangBoy;

    int totalBbangCount;

    public static Bbang_showroom Instance;
    public bool removeBbangMode = false;
    int removedBbangCount = 0;
    int currentBbangCount = 0;

    public enum BbangType
    {
        bbang_mat, bbang_choco, bbang_strawberry, bbang_hot, bbang_vacance,
        bbang_yogurt, bbang_bingle, bbang_maple, bbang_purin
    }
    [SerializeField] private Dictionary<BbangType, BbangData> BbangDatas = new Dictionary<BbangType, BbangData>();

    [Button]
    private void InitData()
    {
        foreach (BbangType bbangType in Enum.GetValues((typeof(BbangType))))
        {
            if (BbangDatas.ContainsKey(bbangType))
            {
                continue;
            }

            var data = new BbangData();
            data.playerPrefID = bbangType.ToString();
            BbangDatas.Add(bbangType, data);
        }
    }
    

    [Serializable]
    private class BbangData
    {
        [ReadOnly] public List<GameObject> bbangs;
        public int prefabIdx;
        [ReadOnly] public int amt;
        public bool isBbang;
        public string playerPrefID;

        [Button]
        private void ForceUpdateAmt(int amt)
        {
            PlayerPrefs.SetInt(playerPrefID, amt);
            UpdateObj();
        }
        public void Init()
        {
            bbangs = new List<GameObject>();
            UpdateObj();
        }
        
        public void UpdateObj()
        {
            amt = PlayerPrefs.GetInt(playerPrefID, 0);
            if (bbangs.Count < amt)
            {
                int loop = amt - bbangs.Count;
                for (int i = 0; i < loop; i++)
                {
                    GameObject newBbang = Bbang_showroom.Instance.PlaceBbang(prefabIdx == 0 ? (int)Random.Range(0, 4) : prefabIdx);
                    bbangs.Add(newBbang);
                }
            }
            // if exceeds
             if (bbangs.Count > amt)
             {
                 int loop = bbangs.Count - amt;
                 for (int i = 0; i < loop; i++)
                 {
                     if(bbangs.Count == 0) return;
                     GameObject destroyObj = bbangs[0];
                     bbangs.Remove(destroyObj);
                     Destroy(destroyObj);
                 }
             }
        }
        public void RemoveThisObj(GameObject obj)
        {
            if(!bbangs.Contains(obj)) return;
            bbangs.Remove(obj);
            Destroy(obj);
            amt -= 1;
            PlayerPrefs.SetInt(playerPrefID, amt);
            PlayerPrefs.Save();
        }

        public void AddBbang(int amt)
        {
            PlayerPrefs.SetInt(playerPrefID, PlayerPrefs.GetInt(playerPrefID) + amt);
            PlayerPrefs.Save();
            UpdateObj();
        }
    }
    
    GameObject selectedObj;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("currentBbangCount"))
        {
            currentBbangCount = PlayerPrefs.GetInt("totalBbangCount") * 2;
            if (currentBbangCount >= 200) currentBbangCount = 200;

            print("currentBbangCount : " + currentBbangCount);
            PlayerPrefs.SetInt("currentBbangCount", currentBbangCount);
            PlayerPrefs.Save();
        } else
        {
            currentBbangCount = PlayerPrefs.GetInt("currentBbangCount");
        }

        if (!PlayerPrefs.HasKey("bbang_choco"))
        {
            PlayerPrefs.SetInt("bbang_choco", PlayerPrefs.GetInt("currentBbangCount"));

            if(PlayerPrefs.GetInt("bbang_choco") > 200)
            {
                PlayerPrefs.SetInt("bbang_choco", 200);
            }
            PlayerPrefs.Save();
        }

        foreach (var data in BbangDatas)
        {
            data.Value.Init();
        }

        foreach (GameObject bbang_i in bbang)
        {
            bbang_i.SetActive(false);
        }
    }

    void Update()
    {
        if (removeBbangMode & Time.frameCount % 10 == 0)
        {
            RemoveFromBbangBoy();
        }
    }

    public void PlaceScrum(Vector2 targetPosition)
    {
        GameObject newScrum = Instantiate(scrum, placePosition.transform);
        newScrum.transform.position = targetPosition;
        newScrum.GetComponent<SetMyActive>().Start();
        newScrum.SetActive(true);
        newScrum.GetComponent<Animator>().SetTrigger("play");
    }

    private GameObject PlaceBbang(int idx)
    {
        GameObject newBbang = Instantiate(bbang[idx], placePosition.transform);
        newBbang.transform.position = new Vector3(placePosition.transform.position.x + Random.Range(-2f, 2f), placePosition.transform.position.y + Random.Range(-3f, 3f));
        newBbang.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        newBbang.SetActive(true);

        return newBbang;
    }

    public void AddBbang(BbangType type, int amt)
    {
        BbangDatas[type].AddBbang(amt);
        UpdateBbangShow();
    }
    
    public void UpdateBbangShow()
    {
        currentBbangCount = 0;
        foreach (var data in BbangDatas)
        {
            data.Value.UpdateObj();
            if (data.Value.isBbang)
            {
                currentBbangCount += data.Value.amt;
            }
        }
        PlayerPrefs.SetInt("currentBbangCount", currentBbangCount);
        PlayerPrefs.Save();
        UpdateBbangInfo();
    }

    void RemoveFromBbangBoy()
    {
        RemoveBbang(bbangboy_ui.transform.position);

        removedBbangCount += 1;
        if(removedBbangCount >= 80)
        {
            removeBbangMode = false;
            bbangBoy.stopEat = true;
            removedBbangCount = 0;
        }
        print(currentBbangCount);
    }

    public void RemoveBbang(Vector2 target)
    {
        //Get Bbangs
        List<GameObject> bbangs = new List<GameObject>();
        foreach (var data in BbangDatas)
        {
            if (data.Value.isBbang)
            {
                bbangs.AddRange(data.Value.bbangs);
            }
        }
        
        //init
        float minDist = float.MaxValue;
        int minIdx = -1;

        if (bbangs.Count == 0) return;

        //FIND SHORTEST BBANG
        for (int i = 0; i < bbangs.Count; i++)
        {
            if (bbangs[i] != null)
            {
                float dist = Vector2.Distance(bbangs[i].transform.position, target);
                if (dist < minDist)
                {
                    minDist = dist;
                    minIdx = i;
                }
            }
        }
        
        if(minIdx == -1) return;
        
        //RemoveItem
        PlaceScrum(bbangs[minIdx].transform.position);
        foreach (var data in BbangDatas)
        {
            data.Value.RemoveThisObj(bbangs[minIdx]); 
        }
        currentBbangCount = bbangs.Count - 1;
        PlayerPrefs.SetInt("currentBbangCount", currentBbangCount);
        PlayerPrefs.Save();
        UpdateBbangInfo();
    }

    public void RemoveBbangAt(string type, GameObject target)
    {
        PlaceScrum(target.transform.position);
        foreach (var data in BbangDatas)
        {
            data.Value.RemoveThisObj(target); 
        }
        UpdateBbangInfo();
    }
    
    void UpdateBbangInfo()
    {
        heart.UpdateBbangInfo();
    }
}
