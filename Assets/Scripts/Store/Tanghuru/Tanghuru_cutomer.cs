using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Tanghuru_cutomer : MonoBehaviour
{
    [SerializeField] private Vector3 InPos, OutPos;
    private Vector3 localPos;

    private bool started;
    
    private void Start()
    {
        if (started) return;
        localPos = gameObject.transform.localPosition;
        started = true;
    }

    [Button]
    public void SetAnimIn()
    {
        if (!started) Start();
        gameObject.transform.localPosition = InPos + localPos;
        gameObject.transform.DOLocalMove(localPos, 2.5f);
    }

    [Button]
    public void SetAnimOut()
    {
        if (!started) Start();
        gameObject.transform.DOLocalMove(OutPos + localPos, 0.75f).SetEase(Ease.InOutExpo);
    }
}