using System.Collections;
using System.Collections.Generic;
using MyUtility;
using UnityEngine;
using UnityEngine.UI;

public class BbobgiController : MonoBehaviour
{
    private const float MaxVelocity = 7f;

    [Header("Managers and Controllers")] 
    [SerializeField] private GameManager gameManager;
    [SerializeField] private BalloonUIManager balloonUIManager;
    [SerializeField] private BbobgiPanelController bbobgiPanelController;

    [Header("UI/Game Elements")] 
    [SerializeField] private Animator clawAnimator;
    [SerializeField] private GameObject guidelineObject;
    [SerializeField] private GameObject timerSlider;
    [SerializeField] private Button downButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    
    private readonly List<GameObject> BbangObj = new();
    private readonly List<GameObject> bbangObjects = new();
    private readonly List<GameObject> clawBoneParts = new();
    private readonly List<GameObject> clawParts = new();
    private readonly List<GameObject> items = new();
    private readonly List<GameObject> obj = new();
    
    private GameObject left, right, bottom;
    
    public float velocity { get; set; }
    public bool playing { get; set; }
    public int bbangCount { get; private set; }

    private void Update()
    {
        if (Mathf.Abs(velocity) > 0.15f)
        {
            if (velocity < -MaxVelocity) velocity = -MaxVelocity;
            else if (velocity > MaxVelocity) velocity = MaxVelocity;

            if (velocity < 0)
            {
                if (timerSlider.transform.position.x - timerSlider.transform.localScale.x / 2 +
                    velocity * Time.deltaTime <
                    left.transform.position.x)
                    timerSlider.transform.position =
                        new Vector2(left.transform.position.x + timerSlider.transform.localScale.x / 2,
                            timerSlider.transform.position.y);
                else
                    timerSlider.transform.position = new Vector2(
                        timerSlider.transform.position.x + velocity * Time.deltaTime,
                        timerSlider.transform.position.y);
            }
            else
            {
                if (timerSlider.transform.position.x + timerSlider.transform.localScale.x / 2 +
                    velocity * Time.deltaTime >
                    right.transform.position.x)
                    timerSlider.transform.position =
                        new Vector2(right.transform.position.x - timerSlider.transform.localScale.x / 2,
                            timerSlider.transform.position.y);
                else
                    timerSlider.transform.position = new Vector2(
                        timerSlider.transform.position.x + velocity * Time.deltaTime,
                        timerSlider.transform.position.y);
            }
        }
    }

    public void StartGame(int count = 1)
    {
        StartCoroutine(StartGameDelay(count));
    }

    public void CloseClaw()
    {
        if (clawAnimator.GetCurrentAnimatorStateInfo(0).IsName("claw_open"))
            clawAnimator.SetTrigger("close");
    }

    public void OepnClaw()
    {
        if (!clawAnimator.GetCurrentAnimatorStateInfo(0).IsName("claw_open"))
            clawAnimator.SetTrigger("open");
    }

    public void GoDown()
    {
        clawAnimator.gameObject.GetComponent<SpringJoint2D>().enabled = false;
    }

    public void GoUp()
    {
        clawAnimator.gameObject.GetComponent<SpringJoint2D>().enabled = true;
    }

    public void CheckScore()
    {
        StartCoroutine(CheckScoreAsync());
    }

    private IEnumerator CheckScoreAsync()
    {
        string bbangType;
        List<int> idxs, amts;
        idxs = new List<int>();
        amts = new List<int>();

        for (var i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].transform.position.y >= guidelineObject.transform.position.y)
            {
                if (items[i].CompareTag("bbang"))
                {
                    bbangType = items[i].GetComponent<BbangTypeHolder>().bbangType;

                    switch (bbangType)
                    {
                        case "choco":
                            idxs.Add(2005);
                            amts.Add(1);
                            break;
                        case "hot":
                            idxs.Add(2003);
                            amts.Add(1);
                            break;
                        case "bingle":
                            idxs.Add(2002);
                            amts.Add(1);
                            break;
                        case "strawberry":
                            idxs.Add(2004);
                            amts.Add(1);
                            break;
                    }

                    // gameManager.AddBBangType(1,"뽑기", bbangType);
                    bbangCount -= 1;

                    PlayerPrefs.SetInt("bbobgiCount", PlayerPrefs.GetInt("bbobgiCount") + 1);
                }

                var Clone = items[i];
                items.RemoveAt(i);
                Destroy(Clone);
            }

            yield return new WaitForSeconds(0.01f);
        }

        if (idxs.Count > 0)
            RewardItemManager.Instance.Init(Converter.List2Array(idxs), Converter.List2Array(amts),
                "Bbobgi", "포켓볼빵을 뽑았다!");
        yield return new WaitForSeconds(0.5f);
        bbobgiPanelController.GameFinished();
    }

    public void DownBtn()
    {
        gameObject.GetComponent<Animator>().SetTrigger("play");
        SetBtnActive(false);
    }

    private IEnumerator StartGameDelay(int count = 1)
    {
        foreach (var obj in clawBoneParts) obj.GetComponent<CapsuleCollider2D>().enabled = false;

        bbangCount = 0;
        var rnd = Random.Range(0, 10);
        var objCount = 20;
        if (rnd <= 7)
        {
            objCount = Random.Range(25, 35);
            count = 1;
        }
        else if (rnd <= 8)
        {
            objCount = Random.Range(35, 40);
            count = 2;
        }
        else if (rnd <= 9)
        {
            objCount = Random.Range(40, 45);
            count = 3;
        }

        bbangCount = count;
        SetBtnActive(false);
        CloseClaw();

        foreach (var item in items) Destroy(item);

        foreach (var item in bbangObjects) Destroy(item);

        items.Clear();
        bbangObjects.Clear();

        var len = obj.Count - 1;

        var myIdxs = new List<int>();

        for (var i = 0; i < count; i++)
        {
            var myIdx = Random.Range(0, objCount);
            myIdxs.Add(myIdx);
        }

        for (var i = 0; i < objCount; i++)
        {
            //yield return new WaitForSeconds(0.01f);
            var idx = Random.Range(0, obj.Count - 1) + 1;
            GameObject newObj;
            if (myIdxs.Contains(i))
                newObj = Instantiate(BbangObj[Random.Range(0, BbangObj.Count)], gameObject.transform);
            else newObj = Instantiate(obj[idx], gameObject.transform);

            var posX = left.transform.position.x +
                       (right.transform.position.x - left.transform.position.x - 1f) * (i % 7 + 1.5f) / 8f;
            var posY = bottom.transform.position.y + 1f +
                       (guidelineObject.transform.position.y - bottom.transform.position.y) * Mathf.Floor(i / 7) / 6;

            //newObj.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), guidelineObject.transform.position.y + Random.Range(-0.3f, 0.3f));
            newObj.transform.position = new Vector3(posX, posY);
            newObj.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            if (!newObj.CompareTag("bbang"))
            {
                var rndSclae = Random.Range(80, 170);
                newObj.transform.localScale = new Vector3(rndSclae, rndSclae, 1);
            }

            newObj.SetActive(true);
            newObj.GetComponent<Rigidbody2D>()
                .AddForce(new Vector2(Random.Range(-1.2f, 1.2f), Random.Range(-1.2f, 1.2f)), ForceMode2D.Impulse);
            items.Add(newObj);
            yield return new WaitForSeconds(0.015f);
        }

        for (var i = 0; i < myIdxs.Count; i++)
        {
            for (var j = 0; j < items.Count; j++)
            {
                if (myIdxs[i] == j) continue;

                if (myIdxs.Contains(j))
                {
                    if (items[myIdxs[i]].GetComponent<BoxCollider2D>().bounds
                        .Intersects(items[j].GetComponent<BoxCollider2D>().bounds))
                        items[j].transform.position = new Vector3(items[j].transform.position.x,
                            guidelineObject.transform.position.y + 2f);
                }
                else if (items[myIdxs[i]].GetComponent<BoxCollider2D>().bounds
                         .Intersects(items[j].GetComponent<CircleCollider2D>().bounds))
                {
                    items[j].transform.position = new Vector3(items[j].transform.position.x,
                        guidelineObject.transform.position.y + 1f);
                }
            }

            yield return new WaitForSeconds(0.015f);
        }

        for (var i = 0; i < myIdxs.Count; i++)
        for (var j = 0; j < items.Count; j++)
        {
            if (myIdxs[i] == j) continue;

            if (myIdxs.Contains(j))
            {
                if (items[myIdxs[i]].GetComponent<BoxCollider2D>().bounds
                    .Intersects(items[j].GetComponent<BoxCollider2D>().bounds))
                    items[j].SetActive(false);
            }
            else if (items[myIdxs[i]].GetComponent<BoxCollider2D>().bounds
                     .Intersects(items[j].GetComponent<CircleCollider2D>().bounds))
            {
                items[j].SetActive(false);
            }
        }

        yield return new WaitForSeconds(1f);
        foreach (var obj in clawBoneParts) obj.GetComponent<CapsuleCollider2D>().enabled = true;
    }

    public void SetBtnActive(bool active)
    {
        leftButton.interactable = active;
        rightButton.interactable = active;
        downButton.interactable = active;
    }

    public void HideObjs()
    {
        Color myColor;
        foreach (var obj in items)
        {
            myColor = obj.GetComponent<SpriteRenderer>().color;
            myColor.a = 0;
            obj.GetComponent<SpriteRenderer>().color = myColor;
        }

        foreach (var obj in clawParts)
        {
            myColor = obj.GetComponent<SpriteRenderer>().color;
            myColor.a = 0;
            obj.GetComponent<SpriteRenderer>().color = myColor;
        }
    }

    public void ShowObjs()
    {
        Color myColor;
        foreach (var obj in items)
        {
            myColor = obj.GetComponent<SpriteRenderer>().color;
            myColor.a = 1;
            obj.GetComponent<SpriteRenderer>().color = myColor;
        }

        foreach (var obj in clawParts)
        {
            myColor = obj.GetComponent<SpriteRenderer>().color;
            myColor.a = 1;
            obj.GetComponent<SpriteRenderer>().color = myColor;
        }
    }
}