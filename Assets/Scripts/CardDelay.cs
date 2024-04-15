using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDelay : MonoBehaviour
{
    [SerializeField] CollectionControl collection;
    [SerializeField] GameObject cardPanel;
    [SerializeField] Main_control main;


    public void SetDelayFalse()
    {
        collection.delay = false;
    }

    public void CardHidden()
    {
        if (main.currentLocation == "bbobgi") main.newStore.GetComponent<StoreControl>().bbobgiPanel.bbobgi.ShowObjs();
        //cardPanel.SetActive(false);
    }
}
