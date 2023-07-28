using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleteparent : MonoBehaviour
{
    public GameObject parent;
    public Main_control myMain;

    public void Remove()
    {
        Destroy(parent);
    }

    public void RemoveBBang()
    {
        myMain.BBangClosed();
        Destroy(parent);
    }

    public void HideMe()
    {
        gameObject.SetActive(false);
    }
}
