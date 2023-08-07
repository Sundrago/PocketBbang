using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bbang_showroom : MonoBehaviour
{
    [SerializeField] GameObject[] bbang = new GameObject[4];
    [SerializeField] GameObject bbang_holder;
    [SerializeField] GameObject placePosition, bbangboy_ui;
    [SerializeField] GameObject scrum;
    [SerializeField] Heart_Control heart;
    [SerializeField] BbangBoyContol bbangBoy;

    int totalBbangCount;
    List<GameObject> bbangs = new List<GameObject>();
    List<GameObject> chocos = new List<GameObject>();
    List<GameObject> mats = new List<GameObject>();
    List<GameObject> strawberries = new List<GameObject>();
    List<GameObject> hots = new List<GameObject>();
    List<GameObject> vacances = new List<GameObject>();
    List<GameObject> yogurts = new List<GameObject>();
    List<GameObject> bingles = new List<GameObject>();
    List<GameObject> maples = new List<GameObject>();
    List<GameObject> purins = new List<GameObject>();

    public bool removeBbangMode = false;
    int removedBbangCount = 0;
    int currentBbangCount = 0;

    int bbang_mat, bbang_choco, bbang_hot, bbang_strawberry, bbang_vacance, bbang_yogurt, bbang_bingle, bbang_maple, bbang_purin;

    GameObject selectedObj;

    void Start()
    {
        if (!PlayerPrefs.HasKey("totalBbangCount"))
        {
            PlayerPrefs.SetInt("totalBbangCount", 0);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("currentBbangCount"))
        {
            currentBbangCount = PlayerPrefs.GetInt("totalBbangCount") * 2;
            if (currentBbangCount >= 200) currentBbangCount = 200;

            print("currentBbangCount : " + currentBbangCount);
            PlayerPrefs.SetInt("currentBbangCount", currentBbangCount);
            PlayerPrefs.Save();
        } else
        {
            currentBbangCount = PlayerPrefs.GetInt("currentBbangCount");
        }

        if (!PlayerPrefs.HasKey("bbang_choco"))
        {
            PlayerPrefs.SetInt("bbang_choco", PlayerPrefs.GetInt("currentBbangCount"));

            if(PlayerPrefs.GetInt("bbang_choco") > 200)
            {
                PlayerPrefs.SetInt("bbang_choco", 200);
            }
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("bbang_mat"))
        {
            PlayerPrefs.SetInt("bbang_mat", PlayerPrefs.GetInt("Matdongsan"));
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("bbang_strawberry"))
        {
            PlayerPrefs.SetInt("bbang_strawberry", 0);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("bbang_hot"))
        {
            PlayerPrefs.SetInt("bbang_hot", 0);
            PlayerPrefs.Save();
        }

        if (!PlayerPrefs.HasKey("bbang_vacance"))
        {
            PlayerPrefs.SetInt("bbang_vacance", 0);
            PlayerPrefs.SetInt("bbang_vacance_total", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("bbang_yogurt"))
        {
            PlayerPrefs.SetInt("bbang_yogurt", 0);
            PlayerPrefs.SetInt("bbang_yogurt_total", 0);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("new_bingle"))
        {
            PlayerPrefs.SetInt("new_bingle", 0);
            PlayerPrefs.SetInt("bbang_bingle", 0);
            PlayerPrefs.SetInt("bbang_bingle_total", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("new_purin"))
        {
            PlayerPrefs.SetInt("new_purin", 0);
            PlayerPrefs.SetInt("bbang_purin", 0);
            PlayerPrefs.SetInt("bbang_purin_total", 0);
            PlayerPrefs.Save();
        }

        foreach (GameObject bbang_i in bbang)
        {
            bbang_i.SetActive(false);
        }

        UpdateBbangShow();
    }

    void Update()
    {
        if (removeBbangMode & Time.frameCount % 10 == 0)
        {
            RemoveFromBbangBoy();
        }
    }

    public void PlaceScrum(Vector2 targetPosition)
    {
        GameObject newScrum = Instantiate(scrum, placePosition.transform);
        newScrum.transform.position = targetPosition;
        newScrum.GetComponent<SetMyActive>().Start();
        newScrum.SetActive(true);
        newScrum.GetComponent<Animator>().SetTrigger("play");
    }

    public void UpdateBbangShow()
    {
        bbang_mat = PlayerPrefs.GetInt("bbang_mat");
        bbang_choco = PlayerPrefs.GetInt("bbang_choco");
        bbang_strawberry = PlayerPrefs.GetInt("bbang_strawberry");
        bbang_hot = PlayerPrefs.GetInt("bbang_hot");
        bbang_vacance = PlayerPrefs.GetInt("bbang_vacance");
        bbang_yogurt = PlayerPrefs.GetInt("bbang_yogurt");
        bbang_bingle  = PlayerPrefs.GetInt("bbang_bingle");
        bbang_maple = PlayerPrefs.GetInt("bbang_maple");
        bbang_purin = PlayerPrefs.GetInt("bbang_purin");

        currentBbangCount = bbang_choco + bbang_strawberry + bbang_hot + bbang_bingle + bbang_maple + bbang_purin;
        PlayerPrefs.SetInt("currentBbangCount", currentBbangCount);
        PlayerPrefs.Save();
        UpdateBbangInfo();

        //choco
        if (chocos.Count < bbang_choco)
        {
            int loop = bbang_choco - chocos.Count;
            for (int i = 0; i < loop; i++)
            {
                int rnd = Random.Range(0, 4);
                GameObject newBbang = Instantiate(bbang[rnd], placePosition.transform);
                newBbang.transform.position = new Vector3(placePosition.transform.position.x + Random.Range(-2f, 2f), placePosition.transform.position.y + Random.Range(-3f, 3f));
                newBbang.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                newBbang.SetActive(true);
                bbangs.Add(newBbang);
                chocos.Add(newBbang);
            }
        }

        //mat
        if (mats.Count < bbang_mat)
        {
            int loop = bbang_mat - mats.Count;
            for (int i = 0; i < loop; i++)
            {
                GameObject newBbang = Instantiate(bbang[4], placePosition.transform);
                newBbang.transform.position = new Vector3(placePosition.transform.position.x + Random.Range(-2f, 2f), placePosition.transform.position.y + Random.Range(-3f, 3f));
                newBbang.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                newBbang.SetActive(true);
                mats.Add(newBbang);
            }
        }
        //if exceeds
        if (mats.Count > bbang_mat)
        {
            int loop = mats.Count - bbang_mat;
            print(loop);
            for (int i = 0; i < loop; i++)
            {
                if(mats[0]!=null)
                {
                    PlaceScrum(mats[0].transform.position);
                    GameObject destroyObj = mats[0];
                    Destroy(destroyObj);
                    mats.Remove(destroyObj);
                }
                print("destry mat");
            }
            mats.RemoveAll(s => s == null);
        }

        //strawberries
        if (strawberries.Count < bbang_strawberry)
        {
            int loop = bbang_strawberry - strawberries.Count;
            for (int i = 0; i < loop; i++)
            {
                GameObject newBbang = Instantiate(bbang[5], placePosition.transform);
                newBbang.transform.position = new Vector3(placePosition.transform.position.x + Random.Range(-2f, 2f), placePosition.transform.position.y + Random.Range(-3f, 3f));
                newBbang.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                newBbang.SetActive(true);
                strawberries.Add(newBbang);
                bbangs.Add(newBbang);
            }
        }

        //hot
        if (hots.Count < bbang_hot)
        {
            int loop = bbang_hot - hots.Count;
            for (int i = 0; i < loop; i++)
            {
                GameObject newBbang = Instantiate(bbang[6], placePosition.transform);
                newBbang.transform.position = new Vector3(placePosition.transform.position.x + Random.Range(-2f, 2f), placePosition.transform.position.y + Random.Range(-3f, 3f));
                newBbang.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                newBbang.SetActive(true);
                hots.Add(newBbang);
                bbangs.Add(newBbang);
            }
        }

        //bingle
        if (bingles.Count < bbang_bingle)
        {
            int loop = bbang_bingle - bingles.Count;
            for (int i = 0; i < loop; i++)
            {
                GameObject newBbang = Instantiate(bbang[9], placePosition.transform);
                newBbang.transform.position = new Vector3(placePosition.transform.position.x + Random.Range(-2f, 2f), placePosition.transform.position.y + Random.Range(-3f, 3f));
                newBbang.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                newBbang.SetActive(true);
                bingles.Add(newBbang);
                bbangs.Add(newBbang);
            }
        }

        //maple
        if (maples.Count < bbang_maple)
        {
            int loop = bbang_maple - maples.Count;
            for (int i = 0; i < loop; i++)
            {
                GameObject newBbang = Instantiate(bbang[10], placePosition.transform);
                newBbang.transform.position = new Vector3(placePosition.transform.position.x + Random.Range(-2f, 2f), placePosition.transform.position.y + Random.Range(-3f, 3f));
                newBbang.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                newBbang.SetActive(true);
                maples.Add(newBbang);
                bbangs.Add(newBbang);
            }
        }

        //purin
        if (purins.Count < bbang_purin)
        {
            int loop = bbang_purin - purins.Count;
            for (int i = 0; i < loop; i++)
            {
                GameObject newBbang = Instantiate(bbang[11], placePosition.transform);
                newBbang.transform.position = new Vector3(placePosition.transform.position.x + Random.Range(-2f, 2f), placePosition.transform.position.y + Random.Range(-3f, 3f));
                newBbang.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                newBbang.SetActive(true);
                purins.Add(newBbang);
                bbangs.Add(newBbang);
            }
        }

        //vacance
        if (vacances.Count < bbang_vacance)
        {
            int loop = bbang_vacance - vacances.Count;
            for (int i = 0; i < loop; i++)
            {
                GameObject newBbang = Instantiate(bbang[7], placePosition.transform);
                newBbang.transform.position = new Vector3(placePosition.transform.position.x + Random.Range(-2f, 2f), placePosition.transform.position.y + Random.Range(-3f, 3f));
                newBbang.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                newBbang.SetActive(true);
                vacances.Add(newBbang);
            }
        }

        //yogurt
        if (yogurts.Count < bbang_yogurt)
        {
            int loop = bbang_yogurt - yogurts.Count;
            for (int i = 0; i < loop; i++)
            {
                GameObject newBbang = Instantiate(bbang[8], placePosition.transform);
                newBbang.transform.position = new Vector3(placePosition.transform.position.x + Random.Range(-2f, 2f), placePosition.transform.position.y + Random.Range(-3f, 3f));
                newBbang.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                newBbang.SetActive(true);
                yogurts.Add(newBbang);
            }
        }

        //print(bbang_vacance + "  " + vacances.Count);
        /*
        //if exceeds
        if (bbangs.Count > currentBbangCount)
        {
            int loop = bbangs.Count - currentBbangCount;
            for (int i = 0; i < loop; i++)
            {
                Destroy(bbang[0]);
                bbangs.RemoveAll(s => s == null);
            }
        }
        */
    }

    void RemoveFromBbangBoy()
    {
        RemoveBbang(bbangboy_ui.transform.position);

        removedBbangCount += 1;
        if(removedBbangCount >= 80)
        {
            removeBbangMode = false;
            bbangBoy.stopEat = true;
            removedBbangCount = 0;
        }
        print(currentBbangCount);
    }

    public void RemoveBbang(Vector2 target)
    {
        float minDist = 10000000;
        int minIdx = 0;
        float dist;

        if (bbangs.Count == 0) return;

        //FIND SHORTEST BBANG
        for (int i = 0; i < bbangs.Count; i++)
        {
            if (bbangs[i] != null)
            {
                dist = Vector2.Distance(bbangs[i].transform.position, target);
                if (dist < minDist)
                {
                    minDist = dist;
                    minIdx = i;
                }
            }
        }

        if(bbangs[minIdx]!=null)
        {
            PlaceScrum(bbangs[minIdx].transform.position);
            switch(bbangs[minIdx].GetComponent<DragMe>().type)
            {
                case "choco":
                    bbang_choco -= 1;
                    break;
                case "mat":
                    bbang_mat -= 1;
                    break;
                case "hot":
                    bbang_hot -= 1;
                    break;
                case "strawberry":
                    bbang_strawberry -= 1;
                    break;
                case "bingle":
                    bbang_bingle -= 1;
                    break;
                case "maple":
                    bbang_maple -= 1;
                    break;
                case "purin":
                    bbang_purin -= 1;
                    break;
            }
            Destroy(bbangs[minIdx]);
        }
        bbangs.RemoveAll(s => s == null);
        chocos.RemoveAll(s => s == null);
        hots.RemoveAll(s => s == null);
        strawberries.RemoveAll(s => s == null);
        mats.RemoveAll(s => s == null);
        bingles.RemoveAll(s => s == null);
        maples.RemoveAll(s => s == null);
        purins.RemoveAll(s => s == null);


        currentBbangCount = bbangs.Count - 1;
        PlayerPrefs.SetInt("currentBbangCount", currentBbangCount);
        PlayerPrefs.SetInt("bbang_choco", bbang_choco);
        PlayerPrefs.SetInt("bbang_mat", bbang_mat);
        PlayerPrefs.SetInt("bbang_strawberry", bbang_strawberry);
        PlayerPrefs.SetInt("bbang_hot", bbang_hot);
        PlayerPrefs.SetInt("bbang_bingle", bbang_bingle);
        PlayerPrefs.SetInt("bbang_maple", bbang_maple);
        PlayerPrefs.SetInt("bbang_purin", bbang_purin);
        PlayerPrefs.Save();
        UpdateBbangInfo();
    }

    public void RemoveBbangAt(string type, GameObject target)
    {
        if(type == "choco")
        {
            bbang_choco -= 1;
            currentBbangCount -= 1;
            PlayerPrefs.SetInt("currentBbangCount", currentBbangCount);
            PlayerPrefs.SetInt("bbang_choco", bbang_choco);
            chocos.Remove(target);
        } else if(type == "mat")
        {
            bbang_mat -= 1;
            PlayerPrefs.SetInt("bbang_mat", bbang_mat);
            mats.Remove(target);
        } else if (type == "hot")
        {
            bbang_hot -= 1;
            currentBbangCount -= 1;
            PlayerPrefs.SetInt("currentBbangCount", currentBbangCount);
            PlayerPrefs.SetInt("bbang_hot", bbang_hot);
            hots.Remove(target);
        }
        else if (type == "strawberry")
        {
            bbang_strawberry -= 1;
            currentBbangCount -= 1;
            PlayerPrefs.SetInt("currentBbangCount", currentBbangCount);
            PlayerPrefs.SetInt("bbang_strawberry", bbang_strawberry);
            strawberries.Remove(target);
        }
        else if (type == "bingle")
        {
            bbang_bingle -= 1;
            currentBbangCount -= 1;
            PlayerPrefs.SetInt("currentBbangCount", currentBbangCount);
            PlayerPrefs.SetInt("bbang_bingle", bbang_bingle);
            bingles.Remove(target);
        }
        else if (type == "maple")
        {
            bbang_maple -= 1;
            currentBbangCount -= 1;
            PlayerPrefs.SetInt("currentBbangCount", currentBbangCount);
            PlayerPrefs.SetInt("bbang_maple", bbang_maple);
            maples.Remove(target);
        }
        else if (type == "purin")
        {
            bbang_purin -= 1;
            currentBbangCount -= 1;
            PlayerPrefs.SetInt("currentBbangCount", currentBbangCount);
            PlayerPrefs.SetInt("bbang_purin", bbang_purin);
            purins.Remove(target);
        }
        else if (type == "vacance")
        {
            bbang_vacance -= 1;
            PlayerPrefs.SetInt("bbang_vacance", bbang_vacance);
            vacances.Remove(target);
        }
        else if (type == "yogurt")
        {
            bbang_yogurt -= 1;
            PlayerPrefs.SetInt("bbang_yogurt", bbang_yogurt);
            yogurts.Remove(target);
        }

        PlayerPrefs.Save();

        PlaceScrum(target.transform.position);
        Destroy(target);

        bbangs.RemoveAll(s => s == null);
        chocos.RemoveAll(s => s == null);
        mats.RemoveAll(s => s == null);
        vacances.RemoveAll(s => s == null);
        yogurts.RemoveAll(s => s == null);
        bingles.RemoveAll(s => s == null);
        maples.RemoveAll(s => s == null);
        purins.RemoveAll(s => s == null);
        UpdateBbangInfo();
    }


    void UpdateBbangInfo()
    {
        heart.UpdateBbangInfo();
    }
}
