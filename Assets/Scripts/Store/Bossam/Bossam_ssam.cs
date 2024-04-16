using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

public class Bossam_ssam : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private const float updateTime = 0.1f;
    // private Vector3 initPos;

    // private void Awake()
    // {
    //     initPos = gameObject.transform.position;
    // }

    private char directionChar;

    private float directionFloat;
    // [SerializeField] private Bossam_character _bossamCharacter;

    private bool isDragging;
    private Vector2 lastUpdatePos, loastUpdatePos_rotation;
    private float lastUpdateTime, lastUpdateTime_rotation;
    private readonly float threshold = 0.15f;

    public void Reset()
    {
        DOTween.Kill(gameObject.transform);
        Bossam_GameManager.Instance.GotSssamPos(gameObject.transform.position);
        Bossam_GameManager.Instance.ShowHand();
        gameObject.transform.localScale = Vector3.one;
        directionFloat = 0;
        gameObject.transform.localEulerAngles = Vector3.zero;
    }


    private void Update()
    {
        if (true)
        {
            directionFloat *= 0.999f;
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x,
                gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z + directionFloat * -15f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        if (Time.time - lastUpdateTime_rotation > updateTime / 2f)
        {
            var direction = mousePosition - loastUpdatePos_rotation;
            // print(directionFloat);

            var lastDrectionChar = directionChar;
            loastUpdatePos_rotation = mousePosition;

            if (direction.x > threshold && direction.y > threshold) directionChar = 'a';
            else if (direction.x > threshold && direction.y < -threshold) directionChar = 'b';
            else if (direction.x < -threshold && direction.y < -threshold) directionChar = 'c';
            else if (direction.x < -threshold && direction.y < threshold) directionChar = 'd';

            switch (lastDrectionChar)
            {
                case 'a':
                    if (directionChar == 'b') directionFloat += 0.1f;
                    else if (directionChar == 'd') directionFloat -= 0.1f;
                    break;
                case 'b':
                    if (directionChar == 'c') directionFloat += 0.1f;
                    else if (directionChar == 'a') directionFloat -= 0.1f;
                    break;
                case 'c':
                    if (directionChar == 'd') directionFloat += 0.1f;
                    else if (directionChar == 'b') directionFloat -= 0.1f;
                    break;
                case 'd':
                    if (directionChar == 'a') directionFloat += 0.1f;
                    else if (directionChar == 'c') directionFloat -= 0.1f;
                    break;
            }
        }

        // if (Time.time - lastUpdateTime > 0.15f)
        // {
        //     lastUpdateTime = Time.time;
        //     lastUpdatePos = mousePosition;
        // }
        gameObject.transform.position = mousePosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DOTween.Kill(gameObject.transform);
        gameObject.transform.SetParent(Bossam_GameManager.Instance.gameObject.transform, true);
        Bossam_GameManager.Instance.HideHand();
        lastUpdateTime = Time.time;
        lastUpdatePos = Camera.main.ScreenToWorldPoint(eventData.position);
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        var diff = lastUpdatePos - mousePosition;
        var velocity = 6f; // / (Time.time - lastUpdateTime);

        var targetPos = gameObject.transform.position;
        targetPos.x += -diff.x * velocity / 200f * 50f + directionFloat / 1.5f;
        targetPos.y += -diff.y * velocity / 200f * 75f;

        var targetPos2 = gameObject.transform.position;
        targetPos2.x += -diff.x * velocity / 200f * 65f;
        targetPos2.y += -diff.y * velocity / 200f * 45f;

        var wayPoints = new Vector3[3];
        wayPoints.SetValue(transform.position, 0);
        wayPoints.SetValue(targetPos, 1);
        wayPoints.SetValue(targetPos2, 2);

        transform.DOPath(wayPoints, 0.6f, PathType.CatmullRom).OnComplete(() => { Reset(); });

        gameObject.transform.DOScale(0.5f, 0.59f)
            .SetEase(Ease.OutQuad);
        ;
        Debug.DrawLine(gameObject.transform.position, targetPos, Color.red, 2f);
        isDragging = false;
    }
}