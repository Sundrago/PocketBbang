using UnityEngine;
using UnityEngine.UI;

public class HeartIconCtrl : MonoBehaviour
{
    private int currnetState = -1;
    private RectMask2D mask;

    public void Start()
    {
        mask = GetComponent<RectMask2D>();
    }

    public void SetHeartStatus(int idx, bool isSmall = true)
    {
        if (idx == currnetState) return;

        if (mask == null) Start();

        float normal;
        if (isSmall) normal = Mathf.Lerp(60, 10, idx / 10f);
        else normal = Mathf.Lerp(120, 20, idx / 10f);
        var padding = mask.padding;
        padding.w = normal;
        mask.padding = padding;

        currnetState = idx;
    }
}