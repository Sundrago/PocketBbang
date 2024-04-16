using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class BalloonUIManager : MonoBehaviour
{
    [SerializeField] private Text msg_text;

    private Animator animator;
    private bool isBeingHiding;
    public static BalloonUIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ShowMsg(string msg)
    {
        gameObject.SetActive(true);
        animator.SetTrigger("show");
        msg_text.text = msg;
        isBeingHiding = false;
    }

    public void HideMsg()
    {
        if (!isBeingHiding) animator.SetTrigger("hide");
        isBeingHiding = true;
    }

    public void SetMeDeactive()
    {
        gameObject.SetActive(false);
    }

    public void Hiding()
    {
        isBeingHiding = true;
    }
}