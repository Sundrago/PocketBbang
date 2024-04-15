using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DdukHit : MonoBehaviour
{
    [SerializeField] DdukCtrl ddukCtrl;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "dduck")
        {
            ddukCtrl.GotDduk(collision.gameObject);
        }
        else if (collision.gameObject.tag == "spider")
        {
            ddukCtrl.LostDduck(collision.gameObject);
        }
    }
}
