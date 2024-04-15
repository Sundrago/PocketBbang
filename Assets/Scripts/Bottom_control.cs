using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottom_control : MonoBehaviour
{
    [SerializeField] GameObject fx1, fx2, fx3, sticker, card, mainCanvas;

    bool hide = false;
    bool stickerShow = false;
    public bool isMaple = false;

    public void HideBottom()
    {
        if(!hide)
        {
            gameObject.GetComponent<Animator>().SetTrigger("hide");
            hide = true;
        }
    }

    public void ShowFx()
    {
        if (!stickerShow)
        {
            fx1.GetComponent<Animator>().SetTrigger("show");
            fx2.GetComponent<Animator>().SetTrigger("show");
            fx3.GetComponent<Animator>().SetTrigger("show");
            sticker.GetComponent<Animator>().SetTrigger("show");
            HideBottom();
            stickerShow = true;
        } else
        {
            fx1.GetComponent<Animator>().SetTrigger("show");
            fx2.GetComponent<Animator>().SetTrigger("show");
            fx3.GetComponent<Animator>().SetTrigger("show");
            sticker.GetComponent<Animator>().SetTrigger("open");
            card.GetComponent<CollectionControl>().Start();
            if(isMaple) card.GetComponent<CollectionControl>().OpenMaple();
            else card.GetComponent<CollectionControl>().OpenNew();
            mainCanvas.GetComponent<Animator>().SetTrigger("hide");
        }
    }
}
