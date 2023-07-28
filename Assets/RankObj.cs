using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RankObj : MonoBehaviour
{
    public GameObject parent;
    public GameObject[] objs;
    public GameObject title;
    public GameObject particleFx, particleFx2;

    public bool shot = false;

    public void RankUp()
    {
        gameObject.SetActive(true);
        shot = true;
        for (int i = 0; i<objs.Length; i++)
        {
            Color myColor = objs[i].GetComponent<Image>().color;
            myColor.a = 1;
            objs[i].GetComponent<Image>().color = myColor;

            objs[i].transform.DOShakePosition(0.5f, 0.5f, 5, 90, false, false)
            .SetEase(Ease.Linear);
            objs[i].transform.DOShakePosition(0.5f, 5f, 300, 90, false, false)
            .SetEase(Ease.Linear)
            .SetDelay(0.5f);
            objs[i].transform.DOShakePosition(0.5f, 20f, 300, 90, false, false)
            .SetEase(Ease.Linear)
            .SetDelay(1.0f);
            objs[i].transform.DOShakePosition(0.5f, 50f, 300, 90, false, false)
            .SetEase(Ease.Linear)
            .SetDelay(1.5f);
            objs[i].transform.DOShakePosition(1f, 100f, 300, 90, false, false)
            .SetEase(Ease.Linear)
            .SetDelay(2.0f);
            objs[i].GetComponent<Image>().DOFade(0f, 1f)
            .SetDelay(2.0f);
        }
        title.GetComponent<Text>().color = new Color(0, 0, 0, 1);

        title.transform.DOShakePosition(0.5f, 5f, 20, 90, false, false)
            .SetDelay(1.5f)
            .OnComplete(TransitionReady);
        title.GetComponent<Text>().DOFade(0f, 1.5f)
            .SetDelay(2.0f)
            .OnComplete(Hide);
    }

    public void ShortRankUp()
    {
        shot = true;
        gameObject.SetActive(true);

        for (int i = 0; i < objs.Length; i++)
        {
            Color myColor = objs[i].GetComponent<Image>().color;
            myColor.a = 1;
            objs[i].GetComponent<Image>().color = myColor;
            objs[i].transform.DOShakePosition(0.3f, 0.5f, 5, 90, false, false)
            .SetEase(Ease.Linear);
            objs[i].transform.DOShakePosition(0.3f, 5f, 300, 90, false, false)
            .SetEase(Ease.Linear)
            .SetDelay(0.3f);
            objs[i].transform.DOShakePosition(1f, 50f, 300, 90, false, false)
            .SetEase(Ease.Linear)
            .SetDelay(0.6f);
            objs[i].GetComponent<Image>().DOFade(0f, 0.5f)
            .SetDelay(0.6f);
        }
        title.GetComponent<Text>().color = new Color(0, 0, 0, 1);

        title.transform.DOShakePosition(0.5f, 5f, 20, 90, false, false)
            .SetDelay(0.6f)
            .OnComplete(TransitionReady);
        title.GetComponent<Text>().DOFade(0f, 1f)
            .SetDelay(0.6f)
            .OnComplete(Hide);
    }

    public void Appear()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < objs.Length; i++)
        {
            Color myColor = objs[i].GetComponent<Image>().color;
            myColor.a = 0;
            objs[i].GetComponent<Image>().color = myColor;
            objs[i].GetComponent<Image>().DOFade(0f, 0.01f);
            objs[i].transform.DOShakePosition(0.8f, 50f, 5, 90, false, true)
            .SetEase(Ease.Linear);
            objs[i].transform.DOMove(new Vector3(0, 0, 0), 0.2f)
            .SetDelay(0.8f);
            objs[i].GetComponent<Image>().DOFade(1f, 0.8f)
            .SetDelay(0.3f);
        }
        title.GetComponent<Text>().color = new Color(0, 0, 0, 0);

        title.transform.DOShakePosition(0.8f, 50f, 5, 90, false, true)
            .SetEase(Ease.Linear);
        title.GetComponent<Text>().DOFade(1f, 0.8f)
            .SetDelay(0.3f)
            .OnComplete(Standby);
        title.transform.DOShakePosition(0.5f).SetDelay(1f);
        title.transform.DOShakeScale(0.5f).SetDelay(1f);
        title.GetComponent<Text>().DOText(title.GetComponent<Text>().text, 1f, true, ScrambleMode.All);
        title.GetComponent<Text>().text = "";
    }

    public void TransitionReady()
    {
        if (!shot) return;
        shot = false;
        GameObject particle1 = Instantiate(particleFx, parent.transform);
        particle1.SetActive(true);
        GameObject particle2 = Instantiate(particleFx2, parent.transform);
        particle2.SetActive(true);

        parent.GetComponent<RankCtrl>().Ready();
    }

    public void Hide()
    {
        //gameObject.SetActive(false);
    }

    public void Standby()
    {
        parent.GetComponent<RankCtrl>().AnimFinished();
    }
}
