using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class RewardItemManager : MonoBehaviour
{
    [SerializeField] private GameObject button, title;
    [SerializeField] private List<RewardItemUI> rewardItems;
    [SerializeField] private Text lowerTitle;
    [SerializeField] private List<GameObject> hideObj;
    public static RewardItemManager Instance;
    

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    private IEnumerator InitAsync(int[] idxs, int[] amounts, string tag)
    {

        title.transform.DOPunchScale(Vector3.one * 0.5f, 1f, 7);
        yield return new WaitForSeconds(0.1f);
        
        for (int i = 0; i < idxs.Length; i++)
        {
            AudioCtrl.Instance.PlaySFXbyTag(SFX_tag.winItem);
            rewardItems[i].Init(idxs[i], amounts[i], tag);
            yield return new WaitForSeconds(0.2f);
        }
        
        
        
        yield return new WaitForSeconds(0.35f);
        button.transform.localScale = Vector3.one;
        button.SetActive(true);
        button.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 4);
    }

    [Button]
    private void DebugTestReward(int itemCode, int amt)
    {
        Init(new int[] {itemCode}, new int[]{amt}, "debug", "debug");
    }
    public void Init(int[] idxs, int[] amounts, string tag, string _lowerTitle)
    {
        gameObject.SetActive(true);
        button.SetActive(false);
        for (int i = 0; i < rewardItems.Count; i++)
        {
            rewardItems[i].gameObject.SetActive(false);
        }

        lowerTitle.text = _lowerTitle;
        foreach (var obj in hideObj)
        {
            obj.SetActive(false);
        }
        StartCoroutine(InitAsync(idxs, amounts, tag));
    }

    public void CloseBtnClicked(){
        if(!button.activeSelf) return;
        button.SetActive(false);
        foreach (var obj in hideObj)
        {
            obj.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
