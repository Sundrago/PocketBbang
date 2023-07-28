using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetWidthHeight : MonoBehaviour
{
    void Start()
    {
        RectTransform scaler = gameObject.GetComponent<RectTransform>();
        Rect myRect = scaler.rect;
        myRect.height = myRect.width;
        scaler.sizeDelta = new Vector2(scaler.sizeDelta.x, scaler.rect.width);
    }
}
