using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class ItemData
{
    public string name;
    public string descr;
    public Sprite sprite;
    public bool isRare;
    public int cardIdx = -1;
    public int idx;
    public int amount;
}

public class ItemDataManager : SerializedMonoBehaviour
{
    public static ItemDataManager Instacne { get; private set; }
    public Dictionary<int, ItemData> ItemDatas { get; } = new();

    private void Awake()
    {
        Instacne = this;
    }
    
    public ItemData GetItemData(int idx)
    {
        if (idx == 2000)
        {
            var rnd = Random.Range(2001, 2006);
            return ItemDatas[rnd];
        }

        if (ItemDatas.ContainsKey(idx)) return ItemDatas[idx];
        print("Item Idx : " + idx + " Not found in ItemDatas");
        return null;
    }

    public IEnumerator GetItemDataAsync(int idx, int amount, string tag, Action<ItemData> callback)
    {
        ItemData data = null;
        yield return new WaitForSeconds(0.1f);

        /*
         * 6004	강냉이 한 줌
           6005	작은 강냉이 봉지
           6006	큰 강냉이 봉지
         */

        if (idx == 6004)
        {
            idx = 1001;
            amount *= 5;
        }
        else if (idx == 6005)
        {
            idx = 1001;
            amount *= 20;
        }
        else if (idx == 6006)
        {
            idx = 1001;
            amount *= 100;
        }

        /*
         * 7001	랜덤 빵
           7002	빵 바구니
           7003	빵 상자
         */

        if (idx == 7001)
        {
            idx = 2000;
        }
        else if (idx == 7002)
        {
            idx = 2000;
            amount *= 5;
        }
        else if (idx == 7003)
        {
            idx = 2000;
            amount *= 10;
        }

        /*
         * 5001	B급 스티커(모든 종류 중 랜덤)
           5002	A급 스티커(모든 종류 중 랜덤)
           5003	S급 스티커(모든 종류 중 랜덤)
         */
        if (idx == 5001 || idx == 5002 || idx == 5003)
        {
            int cardIdx;
            if (idx == 5001) cardIdx = CollectionManager.Instance.GetCardIdxByRank('B');
            else if (idx == 5002) cardIdx = CollectionManager.Instance.GetCardIdxByRank('A');
            else cardIdx = CollectionManager.Instance.GetCardIdxByRank('S');
            data = new ItemData();
            data.isRare = CollectionManager.Instance.AddData(cardIdx);
            data.name = CollectionManager.Instance.myCard[cardIdx].rank + "급 스티커";
            data.descr = CollectionManager.Instance.myCard[cardIdx].description;
            data.sprite = CollectionManager.Instance.GetSpriteAt(cardIdx);
            data.cardIdx = cardIdx;
            yield return new WaitForSeconds(0.2f);
            callback?.Invoke(data);
            yield break;
        }

        /*
         * 2000	포켓볼빵(아무 종류의 빵 랜덤)
         */
        if (idx == 2000) idx = Random.Range(2001, 2006);

        /*
         * 2200	뜯은 초코롤빵
           2201	뜯은 딸기크림빵
           2202	뜯은 핫소스빵
           2203	뜯은 빙글빙글빵
           2204	뜯은 메이풀빵
           2205	뜯은 푸린글스빵
           6001	황금 잉어빵
           6002	슈크림 잉어빵
           6003	팥 잉어빵
         */
        if (ItemDatas.ContainsKey(idx))
        {
            data = ItemDatas[idx];
            switch (idx)
            {
                case 1001:
                    PlayerStatusManager.Instance.AddMoney(PlayerStatusManager.MoneyType.Scrumb, amount);
                    break;
                case 1002:
                    PlayerStatusManager.Instance.AddMoney(PlayerStatusManager.MoneyType.Coin, amount);
                    break;
                case 1003:
                    PlayerStatusManager.Instance.AddMoney(PlayerStatusManager.MoneyType.Diamond, amount);
                    break;
                case 2001:
                    GameManager.Instance.AddBBangType(amount, tag, "purin");
                    break;
                case 2002:
                    GameManager.Instance.AddBBangType(amount, tag, "bingle");
                    break;
                case 2003:
                    GameManager.Instance.AddBBangType(amount, tag, "hot");
                    break;
                case 2004:
                    GameManager.Instance.AddBBangType(amount, tag, "strawberry");
                    break;
                case 2005:
                    GameManager.Instance.AddBBangType(amount, tag, "choco");
                    break;
                case 2100:
                    GameManager.Instance.AddBBangType(amount, tag, "maple");
                    break;
                case 2101:
                    GameManager.Instance.AddMatdongsan();
                    break;
                case 2102:
                    GameManager.Instance.Debug_AddVacance();
                    break;
                case 2103:
                    GameManager.Instance.Debug_AddYogurt();
                    break;
                case 2200:
                    GameManager.Instance.ShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_choco, amount);
                    break;
                case 2201:
                    GameManager.Instance.ShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_strawberry,
                        amount);
                    break;
                case 2202:
                    GameManager.Instance.ShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_hot, amount);
                    break;
                case 2203:
                    GameManager.Instance.ShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_bingle, amount);
                    break;
                case 2204:
                    GameManager.Instance.ShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_maple, amount);
                    break;
                case 2205:
                    GameManager.Instance.ShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_purin, amount);
                    break;
                case 6001:
                    PlayerStatusManager.Instance.AddMoney(PlayerStatusManager.MoneyType.Fish0, amount);
                    break;
                case 6002:
                    PlayerStatusManager.Instance.AddMoney(PlayerStatusManager.MoneyType.Fish1, amount);
                    break;
                case 6003:
                    PlayerStatusManager.Instance.AddMoney(PlayerStatusManager.MoneyType.Fish2, amount);
                    break;
            }
        }
        else
        {
            Debug.Log("Item Idx : " + idx + " Not found in ItemDatas");
        }

        yield return new WaitForSeconds(0.1f);
        data.amount = amount;
        callback?.Invoke(data);
    }

#if UNITY_EDITOR
    [Button]
    private void LoadItemData(string json)
    {
        var ItemDatasTmp = JsonConvert.DeserializeObject<Dictionary<int, ItemData>>(json);

        foreach (var pair in ItemDatasTmp)
        {
            if (pair.Key < 0) continue;
            if (ItemDatas.ContainsKey(pair.Key))
            {
                ItemDatas[pair.Key].name = pair.Value.name;
                ItemDatas[pair.Key].descr = pair.Value.descr.Replace("\\n", "\n");
                ItemDatas[pair.Key].idx = pair.Key;
            }
            else
            {
                ItemDatas.Add(pair.Key, pair.Value);
            }
        }

        print(json);
    }
#endif
}