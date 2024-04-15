using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditControl : MonoBehaviour
{
    [SerializeField] List<Text> texts = new List<Text>();
    [SerializeField] GameObject textAnim;
    [SerializeField] AudioControl myAudio;

    void UpdateData()
    {
        texts[0].text = PlayerPrefs.GetInt("storeCount") + "군데";
        texts[1].text = (PlayerPrefs.GetInt("totalBbangCount")) + "개";
        texts[2].text = (PlayerPrefs.GetInt("Matdongsan") * 10) + "개";
        texts[3].text = (PlayerPrefs.GetInt("ShuttleCount")) + "회";
        texts[4].text = (PlayerPrefs.GetInt("myTotalCards")) + "개" + " /" + (PlayerPrefs.GetInt("TotalCards")) + "개";
        texts[5].text = PlayerPrefs.GetString("ChaName");
    }

    public void Play()
    {
        myAudio.PlayMusic(5);
        UpdateData();
        textAnim.GetComponent<Animator>().SetTrigger("Play");
    }

    public void Close()
    {
        myAudio.PlayMusic(0);
        gameObject.SetActive(false);
    }

    public void AddReview()
    {
#if UNITY_ANDROID
    Application.OpenURL("market://details?id=net.sundragon.bbang");
#elif UNITY_IPHONE
        UnityEngine.iOS.Device.RequestStoreReview();
        Application.OpenURL("https://apps.apple.com/us/app/%ED%8F%AC%EC%BC%93%EB%B3%BC%EB%B9%B5-%EB%8D%94%EA%B2%8C%EC%9E%84/id1617538393");
#endif
    }
}
