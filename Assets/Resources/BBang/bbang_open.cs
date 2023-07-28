using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bbang_open : MonoBehaviour
{
    public void BbangOpen()
    {
        gameObject.GetComponent<Animator>().SetTrigger("hide");
    }
}
