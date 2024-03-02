using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BbobgiCtrl : MonoBehaviour
{
    public Animator claw;
    public GameObject guideline;
    public GameObject slider;
    public Button downBtn, leftBtn, rightBtn;
    public List<GameObject> obj = new List<GameObject>();
    public List<GameObject> BbangObj = new List<GameObject>();
    List<GameObject> gameObjects = new List<GameObject>();
    List<GameObject> bbangObjects = new List<GameObject>();

    public List<GameObject> clawParts = new List<GameObject>();
    public List<GameObject> clawBoneParts = new List<GameObject>();
    public BalloonControl balloon;

    public GameObject left, right, bottom;
    public float velocity = 0;
    const float maxVelocity = 7f;
    public bool playing = false;
    public BbobgiPanelCtrl bbobgiPanel;
    public Main_control main;

    public int bbangCount;

    public void StartGame(int count = 1)
    {
        StartCoroutine(StartGameDelay(count));
    }

    public void CloseClaw()
    {
        if (claw.GetCurrentAnimatorStateInfo(0).IsName("claw_open"))
            claw.SetTrigger("close");
    }
    public void OepnClaw()
    {
        if (!claw.GetCurrentAnimatorStateInfo(0).IsName("claw_open"))
        claw.SetTrigger("open");
    }
    public void GoDown()
    {
        claw.gameObject.GetComponent<SpringJoint2D>().enabled = false;
    }
    public void GoUp()
    {
        claw.gameObject.GetComponent<SpringJoint2D>().enabled = true;
    }

    public void CheckScore()
    {
        StartCoroutine(CheckScoreAsync());
    }

    IEnumerator CheckScoreAsync()
    {
        string bbangType;
        List<int> idxs, amts;
        idxs = new List<int>();
        amts = new List<int>();
        
        for (int i = gameObjects.Count - 1; i >= 0; i--)
        {
            if (gameObjects[i].transform.position.y >= guideline.transform.position.y)
            {
                if(gameObjects[i].CompareTag("bbang"))
                {
                    bbangType = gameObjects[i].GetComponent<BbangType>().bbangType;

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
                    
                    // main.AddBBangType(1,"뽑기", bbangType);
                    bbangCount -= 1;

                    PlayerPrefs.SetInt("bbobgiCount", PlayerPrefs.GetInt("bbobgiCount") + 1);
                }
                GameObject Clone = gameObjects[i];
                gameObjects.RemoveAt(i);
                Destroy(Clone);
            }
            yield return new WaitForSeconds(0.01f);
        }
        if(idxs.Count > 0) RewardItemManager.Instance.Init(MyUtility.Converter.List2Array(idxs),MyUtility.Converter.List2Array(amts), "Bbobgi","포켓볼빵을 뽑았다!");
        yield return new WaitForSeconds(0.5f);
        bbobgiPanel.GameFinished();
    }

    public void DownBtn()
    {
        gameObject.GetComponent<Animator>().SetTrigger("play");
        SetBtnActive(false);
    }

    IEnumerator StartGameDelay(int count = 1)
    {
        foreach (GameObject obj in clawBoneParts)
        {
            obj.GetComponent<CapsuleCollider2D>().enabled = false;
        }

        bbangCount = 0;
        int rnd = Random.Range(0, 10);
        int objCount = 20;
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

        foreach (GameObject item in gameObjects)
        {
            Destroy(item);
        }
        foreach (GameObject item in bbangObjects)
        {
            Destroy(item);
        }
        gameObjects.Clear();
        bbangObjects.Clear();

        int len = obj.Count - 1;

        List<int> myIdxs = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int myIdx = Random.Range(0, objCount);
            myIdxs.Add(myIdx);
        }

        for (int i = 0; i < objCount; i++)
        {
            //yield return new WaitForSeconds(0.01f);
            int idx = Random.Range(0, obj.Count - 1) + 1;
            GameObject newObj;
            if (myIdxs.Contains(i))
            {
                newObj = Instantiate(BbangObj[Random.Range(0, BbangObj.Count)], gameObject.transform);
            }
            else newObj = Instantiate(obj[idx], gameObject.transform);

            float posX = left.transform.position.x + (right.transform.position.x - left.transform.position.x - 1f) * ((i % 7) + 1.5f) / 8f;
            float posY = bottom.transform.position.y + 1f + (guideline.transform.position.y - bottom.transform.position.y) * Mathf.Floor(i / 7) / 6;

            //newObj.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), guideline.transform.position.y + Random.Range(-0.3f, 0.3f));
            newObj.transform.position = new Vector3(posX, posY);
            newObj.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            if (!newObj.CompareTag("bbang"))
            {
                int rndSclae = Random.Range(80, 170);
                newObj.transform.localScale = new Vector3(rndSclae, rndSclae, 1);
            }
            newObj.SetActive(true);
            newObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1.2f, 1.2f), Random.Range(-1.2f, 1.2f)), ForceMode2D.Impulse);
            gameObjects.Add(newObj);
            yield return new WaitForSeconds(0.015f);
        }

        for (int i = 0; i < myIdxs.Count; i++)
        {
            for (int j = 0; j < gameObjects.Count; j++)
            {
                if (myIdxs[i] == j) continue;

                if (myIdxs.Contains(j))
                {
                    if (gameObjects[myIdxs[i]].GetComponent<BoxCollider2D>().bounds.Intersects(gameObjects[j].GetComponent<PolygonCollider2D>().bounds))
                    {
                        gameObjects[j].transform.position = new Vector3(gameObjects[j].transform.position.x, guideline.transform.position.y + 2f);
                    }
                }
                else if (gameObjects[myIdxs[i]].GetComponent<BoxCollider2D>().bounds.Intersects(gameObjects[j].GetComponent<CircleCollider2D>().bounds))
                {
                    gameObjects[j].transform.position = new Vector3(gameObjects[j].transform.position.x, guideline.transform.position.y + 1f);
                }
            }
            yield return new WaitForSeconds(0.015f);
        }

        for (int i = 0; i < myIdxs.Count; i++)
        {
            for (int j = 0; j < gameObjects.Count; j++)
            {
                if (myIdxs[i] == j) continue;

                if (myIdxs.Contains(j))
                {
                    if (gameObjects[myIdxs[i]].GetComponent<BoxCollider2D>().bounds.Intersects(gameObjects[j].GetComponent<PolygonCollider2D>().bounds))
                    {
                        gameObjects[j].SetActive(false);
                    }
                }
                else if (gameObjects[myIdxs[i]].GetComponent<BoxCollider2D>().bounds.Intersects(gameObjects[j].GetComponent<CircleCollider2D>().bounds))
                {
                    gameObjects[j].SetActive(false);
                }
            }
        }

        yield return new WaitForSeconds(1f);
        foreach (GameObject obj in clawBoneParts)
        {
            obj.GetComponent<CapsuleCollider2D>().enabled = true;
        }
    }

    private void Update()
    {
        if (Mathf.Abs(velocity) > 0.15f)
        {
            if (velocity < -maxVelocity) velocity = -maxVelocity;
            else if (velocity > maxVelocity) velocity = maxVelocity;

            if (velocity < 0)
            {
                if(slider.transform.position.x - slider.transform.localScale.x/2 + velocity*Time.deltaTime < left.transform.position.x)
                {
                    slider.transform.position = new Vector2(left.transform.position.x + slider.transform.localScale.x / 2, slider.transform.position.y);
                } else
                {
                    slider.transform.position = new Vector2(slider.transform.position.x + velocity * Time.deltaTime, slider.transform.position.y);
                }
            } else
            {
                if (slider.transform.position.x + slider.transform.localScale.x / 2 + velocity * Time.deltaTime > right.transform.position.x)
                {
                    slider.transform.position = new Vector2(right.transform.position.x - slider.transform.localScale.x / 2, slider.transform.position.y);
                }
                else
                {
                    slider.transform.position = new Vector2(slider.transform.position.x + velocity * Time.deltaTime, slider.transform.position.y);
                }
            }

        }
    }

    public void SetBtnActive(bool active)
    {
        leftBtn.interactable = active;
        rightBtn.interactable = active;
        downBtn.interactable = active;
    }

    public void HideObjs()
    {
        Color myColor;
        foreach(GameObject obj in gameObjects)
        {
            myColor = obj.GetComponent<SpriteRenderer>().color;
            myColor.a = 0;
            obj.GetComponent<SpriteRenderer>().color = myColor;
        }
        foreach (GameObject obj in clawParts)
        {
            myColor = obj.GetComponent<SpriteRenderer>().color;
            myColor.a = 0;
            obj.GetComponent<SpriteRenderer>().color = myColor;
        }
    }

    public void ShowObjs()
    {
        Color myColor;
        foreach (GameObject obj in gameObjects)
        {
            myColor = obj.GetComponent<SpriteRenderer>().color;
            myColor.a = 1;
            obj.GetComponent<SpriteRenderer>().color = myColor;
        }
        foreach (GameObject obj in clawParts)
        {
            myColor = obj.GetComponent<SpriteRenderer>().color;
            myColor.a = 1;
            obj.GetComponent<SpriteRenderer>().color = myColor;
        }
    }
    
}
