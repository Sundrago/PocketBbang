using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BbangControl : MonoBehaviour
{
    public Sprite[] lefts = new Sprite[3];
    public Sprite[] rights = new Sprite[3];
    public Sprite[] bbang = new Sprite[3];

    public GameObject left_ui, right_ui, bbang_ui;

    public void SetBbang(int idx)
    {
        left_ui.GetComponent<Image>().sprite = lefts[idx];
        right_ui.GetComponent<Image>().sprite = rights[idx];
        bbang_ui.GetComponent<Image>().sprite = bbang[idx];

        if(idx == 4)
        {
            bbang_ui.GetComponent<Bottom_control>().isMaple = true;
        }
    }
}
