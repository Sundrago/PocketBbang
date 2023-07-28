using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMyActive : MonoBehaviour
{
    bool started = false;
    public void Start()
    {
        if (started) return;

        gameObject.SetActive(false);
        started = true;
    }

    public void SetMeDeactive()
    {
        gameObject.SetActive(false);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
