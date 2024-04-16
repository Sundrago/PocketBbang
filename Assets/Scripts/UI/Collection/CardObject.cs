using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CardObject : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    [SerializeField] private GameObject selectedObject;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void RemoveSelf()
    {
        parentObject.SetActive(false);
    }

    public void ShowCard(int index)
    {
        animator.SetTrigger("show");
    }

    public void HideCard()
    {
        animator.SetTrigger("hide");
    }

    public void SetSelectedUI(bool selected)
    {
        selectedObject.SetActive(selected);
    }
}