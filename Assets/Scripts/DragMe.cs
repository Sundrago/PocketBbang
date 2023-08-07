using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragMe : MonoBehaviour
{
    [SerializeField] GameObject bbangBtn;

    public string type;

    Collider2D objectCollider;
    Vector2 startPosition;
    bool isDraggable;
    bool isDragging;
    bool drag = false;

    void Update()
    {
        if(Time.frameCount % 10 == 0 & transform.position.y < -1000)
        {
            Destroy(gameObject);
        }
    }

    public void OnMouseDown()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        startPosition = eventDataCurrentPosition.position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        if (results.Count == 0)
        {
            drag = true;
        }
        else drag = false;
        
    }

    void OnMouseDrag()
    {
        if (drag)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.transform.position = mousePosition;
        }
    }

    private void OnMouseUp()
    {
        if (drag & Vector2.Distance(startPosition, new Vector2(Input.mousePosition.x, Input.mousePosition.y)) < 15f)
            bbangBtn.GetComponent<BbangBtnControl>().ShowBbangBtn(this.transform.position, type, gameObject);
    }
}