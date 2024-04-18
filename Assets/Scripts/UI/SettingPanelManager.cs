using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingPanelManager : MonoBehaviour
{
    [FormerlySerializedAs("myAudio")] 
    [SerializeField] private AudioController audioController;

    [FormerlySerializedAs("msg")] 
    [SerializeField] private PhoneMessageController phoneMessageController;
    [SerializeField] private Text audioBtnText, lowDataBtn, autoSaveBtn;

    private bool muteAudio;

    public void Init()
    {
        SetAudio();
        SetLowData();
        SetAutoSave();
    }

    public void CloseSettingPanel()
    {
        gameObject.SetActive(false);
    }

    private void SetAudio()
    {
        if (PlayerPrefs.GetInt("muteAudio") == 0)
        {
            muteAudio = false;
            audioBtnText.text = "배경음악 : 켜짐";
        }
        else
        {
            audioBtnText.text = "배경음악 : 꺼짐";
            muteAudio = true;
        }

        audioController.ToggleMute(muteAudio);
    }

    private void SetAutoSave()
    {
        if (PlayerPrefs.GetInt("autoSave") == 0)
        {
            autoSaveBtn.text = "클라우드 저장 : 꺼짐";
            phoneMessageController.SetMsg("클라우드 저장 기능을 사용해야 데이터를 안전하게 저장할 수 있습니다.", 1);
        }
        else
        {
            autoSaveBtn.text = "클라우드 저장 : 켜짐";
        }
    }

    public void ToggleMuteBtn()
    {
        if (PlayerPrefs.GetInt("muteAudio") == 0)
            PlayerPrefs.SetInt("muteAudio", 1);
        else
            PlayerPrefs.SetInt("muteAudio", 0);
        PlayerPrefs.Save();
        SetAudio();
    }

    public void ToggleAutoSaveBtn()
    {
        if (PlayerPrefs.GetInt("autoSave") == 0)
            PlayerPrefs.SetInt("autoSave", 1);
        else
            PlayerPrefs.SetInt("autoSave", 0);
        PlayerPrefs.Save();
        SetAutoSave();
    }

    public void SetLowData()
    {
        if (PlayerPrefs.GetInt("lowDataMode") == 0)
            lowDataBtn.text = "로우데이터 모드 : 꺼짐";
        else
            lowDataBtn.text = "로우데이터 모드 : 켜짐";
    }

    public void LowDataMode()
    {
        if (PlayerPrefs.GetInt("lowDataMode") == 0)
            PlayerPrefs.SetInt("lowDataMode", 1);
        else
            PlayerPrefs.SetInt("lowDataMode", 0);
        PlayerPrefs.Save();
        SetLowData();
    }
}