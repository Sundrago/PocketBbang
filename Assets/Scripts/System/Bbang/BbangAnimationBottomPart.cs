using UnityEngine;

public class BbangAnimationBottomPart : MonoBehaviour
{
    [SerializeField] private GameObject fx1, fx2, fx3, sticker, card, mainCanvas;

    public bool isMaple;
    private bool hide;
    private bool stickerShow;

    public void HideBottom()
    {
        if (!hide)
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
        }
        else
        {
            fx1.GetComponent<Animator>().SetTrigger("show");
            fx2.GetComponent<Animator>().SetTrigger("show");
            fx3.GetComponent<Animator>().SetTrigger("show");
            sticker.GetComponent<Animator>().SetTrigger("open");
            card.GetComponent<CollectionManager>().Start();
            if (isMaple) card.GetComponent<CollectionManager>().OpenMaple();
            else card.GetComponent<CollectionManager>().OpenNew();
            mainCanvas.GetComponent<Animator>().SetTrigger("hide");
        }
    }
}