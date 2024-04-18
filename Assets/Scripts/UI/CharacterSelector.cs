using UnityEngine;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour
{
    [Header("Managers and Controllers")] 
    [SerializeField] private PrompterController prompterController;

    [Header("UI Components")] [SerializeField]
    private GameObject namePanel;

    [SerializeField] private GameObject characterPanel;
    [SerializeField] private Text nameInputText;
    [SerializeField] private Text defaultNameText;
    [SerializeField] private Image characterA;
    [SerializeField] private Image characterB;

    private Color alpha;
    private int ChaIdx;
    private bool started;

    private void Start()
    {
        if (started) return;
        started = true;
        alpha = Color.white;
        alpha.a = 0.5f;
    }

    private void UpdateChaSelection()
    {
        if (ChaIdx == 0)
        {
            characterA.color = Color.white;
            characterB.color = alpha;
        }
        else
        {
            characterA.color = alpha;
            characterB.color = Color.white;
        }
    }

    public void NamePanelBtnClicked(string idx)
    {
        if (idx == "next")
        {
            namePanel.SetActive(false);
            characterPanel.SetActive(true);

            if (PlayerPrefs.HasKey("ChaIdx")) ChaIdx = PlayerPrefs.GetInt("ChaIdx");

            UpdateChaSelection();

            PlayerPrefs.SetString("ChaName", nameInputText.text);
            PlayerPrefs.Save();
        }
        else if (idx == "close")
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
        namePanel.SetActive(true);
        characterPanel.SetActive(false);

        if (PlayerPrefs.HasKey("ChaName"))
        {
            defaultNameText.text = PlayerPrefs.GetString("ChaName");
            nameInputText.text = PlayerPrefs.GetString("ChaName");
        }
    }

    public void CloseChaSelect()
    {
        namePanel.SetActive(false);
        characterPanel.SetActive(false);
        prompterController.LoadChaData();
        gameObject.SetActive(false);
    }

    public void ChaBtnClicked(int idx)
    {
        ChaIdx = idx;
        UpdateChaSelection();
    }
}