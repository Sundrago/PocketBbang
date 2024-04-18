using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BbangAnimationBbangSprites : MonoBehaviour
{
    [SerializeField] private BbangAnimationBottomPart bbangAnimationBottomPart;
    [SerializeField] private Sprite[] lefts = new Sprite[3];
    [SerializeField] private Sprite[] rights = new Sprite[3];
    [SerializeField] private Sprite[] bbang = new Sprite[3];
    [SerializeField] private Image left_ui, right_ui, bbang_ui;

    public void SetBbang(int idx)
    {
        left_ui.sprite = lefts[idx];
        right_ui.sprite = rights[idx];
        bbang_ui.sprite = bbang[idx];

        if (idx == 4) bbangAnimationBottomPart.isMaple = true;
    }
}