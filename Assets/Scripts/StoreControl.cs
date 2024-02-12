using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreControl : MonoBehaviour
{
    [SerializeField] Sprite[] storeImgs;
    [SerializeField] GameObject storeImg_ui;
    [SerializeField] GameObject me;
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject cloud;
    [SerializeField] GameObject bbobgiMachine;
    [SerializeField] public BbobgiPanelCtrl bbobgiPanel;
    [SerializeField] private GameObject scrumb;
    
    public void UpdateStore(int i)
    {
        storeImg_ui.GetComponent<Image>().sprite = storeImgs[i];

        if (i == 10) cloud.SetActive(true);
        else cloud.SetActive(false);

        if (i == 5)
        {
            bbobgiMachine.SetActive(true);
            bbobgiPanel.reload = true;
        } else
        {
            scrumb.gameObject.SetActive(Random.value < 0.33f);
            bbobgiMachine.SetActive(false);
        }
    }

    public void DestroySelf()
    {
        Destroy(me);
    }

    public void WentToStore()
    {
        mainPanel.GetComponent<Main_control>().WentToStore();
    }
    public void WentToPark()
    {
        if(mainPanel.GetComponent<Main_control>().gointToPark)
        mainPanel.GetComponent<Main_control>().WentToPark();
    }

    public void StartBbobgi()
    {
        bbobgiPanel.GetComponent<BbobgiPanelCtrl>().OpenGame();
    }

    public void ScrumbBtnClicked()
    {
        int rnd = Random.Range(1, 4);
        RewardItemManager.Instance.Init(new int[1]{1001}, new int[1]{rnd}, "scrumb", "반짝이는 빵 부스러기를 주웠다!");
        scrumb.gameObject.SetActive(false);
    }
}
