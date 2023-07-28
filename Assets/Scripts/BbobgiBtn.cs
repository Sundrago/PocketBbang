using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BbobgiBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool goLeft;
    public BbobgiCtrl bbogi;
    bool OnPressed;
    const float velocityRate = 0.1f;

    public void OnPointerDown( PointerEventData eventData )
    {
        OnPressed = true;
    }

    public void OnPointerUp( PointerEventData eventData )
    {
        OnPressed = false;
        bbogi.velocity = 0;
    }

    private void Update()
    {
        if (!gameObject.GetComponent<Button>().interactable) return;

        if (!OnPressed)
        {
            return;
        } else
        {
        if (goLeft) {
            bbogi.velocity -= velocityRate;
        } else
        {
            bbogi.velocity += velocityRate;
        }
        }
    }
}
