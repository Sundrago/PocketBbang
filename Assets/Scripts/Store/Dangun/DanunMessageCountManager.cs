using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DanunMessageCountManager : MonoBehaviour
{
    [FormerlySerializedAs("dangunCharacterController")] [FormerlySerializedAs("dangunCha")] [SerializeField]
    private DangunCharacterManager dangunCharacterManager;

    [SerializeField] private GameObject ok_ui, main, msg_ui, upBtn_ui, downBtn_ui, count_ui;

    public string actionCode;
    private int currentCount, maxCount = 3;
    private bool started;

    public void Start()
    {
        if (!started)
        {
            gameObject.SetActive(false);
            started = true;
        }
    }

    public void OkbtnClicked()
    {
        print(actionCode);
        switch (actionCode)
        {
            case "DangunChaMsgCallBack":
                dangunCharacterManager.DangunChaMsgCallBack(currentCount);
                break;
        }

        gameObject.SetActive(false);
    }

    public void SetMsg(string msg, int max, string action = "")
    {
        msg_ui.GetComponent<Text>().text = msg;
        currentCount = 1;

        upBtn_ui.GetComponent<Button>().interactable = true;
        downBtn_ui.GetComponent<Button>().interactable = true;

        gameObject.SetActive(true);
        actionCode = action;
        maxCount = max;

        UpdateCountInfo();
    }

    public void UpBtnClicked()
    {
        if (currentCount < maxCount)
        {
            currentCount += 1;
            if (currentCount >= maxCount) upBtn_ui.GetComponent<Button>().interactable = false;
            downBtn_ui.GetComponent<Button>().interactable = true;
            UpdateCountInfo();
        }
    }

    public void DownBtnClicked()
    {
        if (currentCount > 1)
        {
            currentCount -= 1;
            if (currentCount <= 1) downBtn_ui.GetComponent<Button>().interactable = false;
            upBtn_ui.GetComponent<Button>().interactable = true;
            UpdateCountInfo();
        }
    }

    private void UpdateCountInfo()
    {
        count_ui.GetComponent<Text>().text = currentCount + " 개";
    }
}