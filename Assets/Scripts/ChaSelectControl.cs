using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaSelectControl : MonoBehaviour
{

    public GameObject name_panel, cha_panel;
    public Text NameInput_ui, NameInput_default_ui;
    public Image Cha_0, Cha_1;

    Color alpha;
    int ChaIdx = 0;

    public PrompterControl pmtc;
    bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        if (started) return;
        started = true;
        alpha = Color.white;
        alpha.a = 0.5f;
    }

    void UpdateChaSelection()
    {
        if(ChaIdx == 0)
        {
            Cha_0.color = Color.white;
            Cha_1.color = alpha;
        } else
        {
            Cha_0.color = alpha; 
            Cha_1.color = Color.white;
        }
    }

    public void NamePanelBtnClicked(string idx)
    {
        if(idx == "next")
        {
            name_panel.SetActive(false);
            cha_panel.SetActive(true);

            if (PlayerPrefs.HasKey("ChaIdx"))
            {
                ChaIdx = PlayerPrefs.GetInt("ChaIdx");
            }

            UpdateChaSelection();

            PlayerPrefs.SetString("ChaName", NameInput_ui.text);
            PlayerPrefs.Save();
        } else if (idx == "close")
        {
            CloseChaSelect();
        }
    }

    public void ChaPanelBtnClicked(string idx)
    {
        if (idx == "next")
        {
            CloseChaSelect();

            PlayerPrefs.SetInt("ChaIdx", ChaIdx);
            PlayerPrefs.Save();
        }
        else if (idx == "close")
        {
            OpenChaSelect();
        }
    }

    public void OpenChaSelect()
    {
        gameObject.SetActive(true);
        name_panel.SetActive(true);
        cha_panel.SetActive(false);

        if(PlayerPrefs.HasKey("ChaName"))
        {
            NameInput_default_ui.text = PlayerPrefs.GetString("ChaName");
            NameInput_ui.text = PlayerPrefs.GetString("ChaName");
        }
    }

    public void CloseChaSelect()
    {
        name_panel.SetActive(false);
        cha_panel.SetActive(false);
        pmtc.LoadChaData();
        gameObject.SetActive(false);
    }

    public void ChaBtnClicked(int idx)
    {
        ChaIdx = idx;
        UpdateChaSelection();
    }
}
