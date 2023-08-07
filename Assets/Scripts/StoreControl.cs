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
}
