using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalloonControl : MonoBehaviour
{
    public Text msg_text;
    bool hiding;

    public void ShowMsg(string msg)
    {
        gameObject.SetActive(true);
        gameObject.GetComponent<Animator>().SetTrigger("show");
        msg_text.text = msg;
        hiding = false;
    }

    public void HideMsg()
    {
        if(!hiding) gameObject.GetComponent<Animator>().SetTrigger("hide");
        hiding = true;
    }

    public void SetMeDeactive()
    {
        gameObject.SetActive(false);
    }

    public void Hiding()
    {
        hiding = true;
    }
}
