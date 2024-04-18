using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BbobgiBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private const float velocityRate = 0.1f;
    [SerializeField] private BbobgiController bbobgiController;

    public bool goLeft;
    private Button button;
    private bool OnPressed;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (!button.interactable) return;

        if (!OnPressed) return;

        if (goLeft)
            bbobgiController.velocity -= velocityRate;
        else
            bbobgiController.velocity += velocityRate;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnPressed = false;
        bbobgiController.velocity = 0;
    }
}