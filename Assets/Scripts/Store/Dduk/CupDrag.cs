using UnityEngine;
using UnityEngine.EventSystems;

public class CupDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject cupA;

    private Vector2 initialPoint;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPoint = eventData.position;
        cupA.transform.position = gameObject.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        gameObject.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(eventData.position).x,
            gameObject.transform.position.y);
        cupA.transform.position = gameObject.transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cupA.transform.position = gameObject.transform.position;
    }
}