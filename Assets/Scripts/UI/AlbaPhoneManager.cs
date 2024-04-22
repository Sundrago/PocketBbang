using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AlbaPhoneManager : MonoBehaviour
{
    public static AlbaPhoneManager Instance { get; private set; }
    
    [SerializeField] private PhoneMessageController phoneMessageController;
    [SerializeField] private List<Image> btnImgs;
    [SerializeField] private GameManager main;
    [SerializeField] private BalloonUIManager balloon;

    private List<bool> available;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        available = new List<bool> { false, false, false };
        available[0] = PlayerPrefs.GetInt("albaExp", 0) != 0;
        available[1] = PlayerPrefs.GetInt("tanghuru_friend", 0) != 0;
        available[2] = PlayerPrefs.GetInt("bossam_friend", 0) != 0;

        for (var i = 0; i < 3; i++) btnImgs[i].color = available[i] ? Color.white : new Color(1, 1, 1, 0.5f);
    }

    public void AbabBtnClicked(int idx)
    {
        if (!available[idx])
            switch (idx)
            {
                case 0:
                    phoneMessageController.SetMsg("지금은 알바 자리가 없습니다.\n쌈마트24에 가서 맛동석 점장에게 얘기해보는 건 어떨까요?", 1);
                    return;
                    break;
                case 1:
                    phoneMessageController.SetMsg("지금은 알바 자리가 없습니다.\n탕후루 가게에서 왕형을 열심히 도와보는 건 어떨까요?", 1);
                    return;
                    break;
                case 2:
                    phoneMessageController.SetMsg("지금은 알바 자리가 없습니다.\n장족발보쌈 가게에서 장왕을 열심히 도와보는 건 어떨까요?", 1);
                    return;
                    break;
            }

        if (PlayerStatusManager.Instance.IsHeartEmpty())
        {
            balloon.ShowMsg("지금은 좀 피곤하다..");
            return;
        }

        if (main.currentLocation != "home" && main.currentLocation != "store")
        {
            balloon.ShowMsg("여기서는 할 수 없다..");
            return;
        }

        PlayerPrefs.SetInt("GotoAlbaIdx", idx);
        switch (idx)
        {
            case 0:
                phoneMessageController.SetMsg("알바를 하기 위해\n3마트24로 이동할까요?", 2, "albaGo");
                break;
            case 1:
                phoneMessageController.SetMsg("알바를 하기 위해\n탕후루가게로 이동할까요?", 2, "albaGo");
                break;
            case 2:
                phoneMessageController.SetMsg("알바를 하기 위해\n왕충동장보쌈으로 이동할까요?", 2, "albaGo");
                break;
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}