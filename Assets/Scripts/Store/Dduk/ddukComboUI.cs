using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ddukComboUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Text text;
    [SerializeField] private GameObject endPos;

    public void ShowCombo(int idx)
    {
        print("idx");
        text.text = idx + " 콤보";

        image.DOFade(0.5f, 0.65f)
            .SetEase(Ease.OutQuint);
        text.DOFade(0.8f, 0.65f)
            .SetEase(Ease.OutQuint);

        image.DOFade(0f, 0.35f)
            .SetDelay(0.65f);
        text.DOFade(0f, 0.35f)
            .SetDelay(0.65f);

        gameObject.transform.DOMove(endPos.transform.position, 1f)
            .SetEase(Ease.OutQuint)
            .OnComplete(DestroySelf);

        gameObject.SetActive(true);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}