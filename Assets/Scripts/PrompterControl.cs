using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class Strings
{
    public string name;
    public string text;
}

public class NextActions
{
    public string parent = "null";
    public string action;
}

public class Options
{
    public string text;
    public string parent;
    public string action;
}

public class PrompterControl : MonoBehaviour
{
    [SerializeField] GameObject[] nextBtns = new GameObject[3];
    [SerializeField] GameObject[] nextBtnTexts = new GameObject[3];
    [SerializeField] GameObject MainPanel, board;
    [SerializeField] CollectionControl cc;
    [SerializeField] GameObject[] chaImgs;
    [SerializeField] DangunChaCtrl dangunCha;
    [SerializeField] Text prompter, name_ui;
    [SerializeField] GameObject nextBtn;

    [SerializeField] BalloonControl balloon;
    
    public List<Strings> myStrings = new List<Strings>();
    public List<Options> myOptions = new List<Options>();
    public NextActions myNextActions;
    public bool imageMode;


    int currentIdx;
    int ChaIdx;
    float textUpdateSpeed = 0.03f;
    float updateFrame;
    bool textUpdateDone;
    bool hasOption, hasNextOption;
    bool started = false;
    string ChaName;

    public void Start()
    {
        if (!started)
        {
            imageMode = false;
            LoadChaData();
            Reset();
            started = true;
        }
    }

    public void LoadChaData()
    {
        if (PlayerPrefs.HasKey("ChaIdx"))
        {
            ChaIdx = PlayerPrefs.GetInt("ChaIdx");
        }

        if (PlayerPrefs.HasKey("ChaName"))
        {
            ChaName = PlayerPrefs.GetString("ChaName");
        }
    }

    void Update()
    {
        //print(myStrings.Count);
        if (textUpdateDone) return;
        if (myStrings.Count == 0)
        {
            if (hasOption)
            {
                textUpdateDone = true;
                UpdateOptionBtn();
                return;
            }
        }

        if(currentIdx < myStrings.Count)
        {
            if(updateFrame + textUpdateSpeed < Time.time)
            {
                if(prompter.text == "")
                {

                    if(myStrings[currentIdx].name == "훈이")
                    {
                        name_ui.text = ChaName;
                    } else if(myStrings[currentIdx].name == "점주") {
                        name_ui.text = "맛동석(점주)";
                    } else
                    {
                        name_ui.text = myStrings[currentIdx].name;
                    }


                    if (imageMode) showChaImg(myStrings[currentIdx].name);
                }

                if(prompter.text.Length < myStrings[currentIdx].text.Length)
                {
                    prompter.text = myStrings[currentIdx].text.Substring(0, prompter.text.Length + 1);
                }
                updateFrame = Time.time;
            }

            if (prompter.text == myStrings[currentIdx].text)
            {
                textUpdateDone = true;
                nextBtn.SetActive(true);
            }
        }
    }

    public void showChaImg(string name)
    {
        int target = -1;

        switch (name)
        {
            case "훈이":
                if (ChaIdx == 0)
                    target = 0;
                else
                    target = 7;
                break;
            case "미소녀":
                target = 1;
                break;
            case "점주":
                target = 2;
                break;
            case "열정맨":
                target = 3;
                break;
            case "비실이":
                target = 4;
                break;
            case "양아치":
                target = 5;
                break;
            case "초코롤빵":
                target = 6;
                break;
            case "딸기크림빵":
                target = 8;
                break;
            case "맛동산":
                target = 9;
                break;
            case "핫소스빵":
                target = 10;
                break;
            case "용돈":
                target = 11;
                break;
            case "바캉스":
                target = 12;
                break;
            case "야쿠르트":
                target = 13;
                break;
            case "박법학박사":
                target = 14;
                break;
            case "메이풀빵":
                target = 15;
                break;
            case "푸린글스빵":
                target = 16;
                break;
            //박종업원
            case "박종업원":
                target = 17;
                break;
            case "나이롱마스크":
                target = 18;
                break;
            case "왕형":
                target = 19;
                break;
            case "장왕":
                target = 20;
                break;
        }

        if (target == -1)
        {
            print("Image Set Name Error : " + name);
            return;
        }

        for (int i = 0; i< chaImgs.Length; i++)
        {
            if (i == target)
            {
                chaImgs[i].SetActive(true);
                chaImgs[i].GetComponent<Animator>().SetTrigger("show");
            }
            else chaImgs[i].GetComponent<Animator>().SetTrigger("hide");
        }
        
        
    }
    public void Reset()
    {
        LoadChaData();
        if (imageMode)
        {
            for (int i = 0; i < chaImgs.Length; i++)
            {
                chaImgs[i].GetComponent<Animator>().SetTrigger("hide");
            }
        }
        imageMode = false;
        started = true;
        updateFrame = Time.time;
        currentIdx = 0;
        myStrings = new List<Strings>();
        myOptions = new List<Options>();
        myNextActions = new NextActions();
        textUpdateDone = false;
        nextBtn.SetActive(false);
        hasOption = false;
        hasNextOption = false;

        prompter.text = "";

        nextBtns[0].SetActive(false);
        nextBtns[1].SetActive(false);
        nextBtns[2].SetActive(false);

        board.SetActive(true);
    }

    public void AddString(string name, string subject)
    {
        Strings newString = new Strings();
        newString.name = name;
        newString.text = subject;

        myStrings.Add(newString);
        return;
    }

    public void AddNextAction(string parent, string action)
    {
        if (myStrings.Count == 0)
        {
            print("AddNextAction : SHORTCUT");
            CallAction(parent, action);
            return;
        }
        NextActions newAction = new NextActions();
        newAction.parent = parent;
        newAction.action = action;

        myNextActions = newAction;
        hasNextOption = true;
        return;
    }

    public void AddOption(string option, string parent, string action)
    {
        Options newOption = new Options();
        newOption.text = option;
        newOption.parent = parent;
        newOption.action = action;

        myOptions.Add(newOption);
        hasOption = true;
        return;
    }


    private void UpdateOptionBtn()
    {
        board.SetActive(false);
        nextBtn.SetActive(false);
        //print("UpdateOptionBtn() called");
        nextBtns[0].SetActive(false);
        nextBtns[1].SetActive(false);
        nextBtns[2].SetActive(false);

        for (int i = 0; i<myOptions.Count; i++)
        {
            nextBtnTexts[i].GetComponent<Text>().text = myOptions[i].text;
            nextBtns[i].SetActive(true);
        }
    } 


    public void NextBtnClicked(int i)
    {
        switch(i) {
            case 0:
                if(currentIdx == myStrings.Count - 1)
                {
                    if (hasOption) UpdateOptionBtn();
                    else if (hasNextOption) CallAction(myNextActions.parent, myNextActions.action);
                    else CallAction(myNextActions.parent, myNextActions.action);
                    break;
                }
                currentIdx += 1;
                prompter.text = "";
                textUpdateDone = false;
                break;
            case 1:
                CallAction(myOptions[0].parent, myOptions[0].action);
                break;
            case 2:
                CallAction(myOptions[1].parent, myOptions[1].action);
                break;
            case 3:
                CallAction(myOptions[2].parent, myOptions[2].action);
                break;
        } 
    }

    [Button]
    public void CallAction(string parent, string action)
    {
        // balloon.ShowMsg(parent + " " + action);
        if(string.IsNullOrEmpty(parent)) return;
        
        switch (parent) {
            case "main":
                MainPanel.GetComponent<Main_control>().ActionHandler(action);
                break;
            case "store":
                MainPanel.GetComponent<Main_control>().StoreEvent(action);
                break;
            case "bbangBoy":
                MainPanel.GetComponent<Main_control>().BbangBoyAction(action);
                break;
            case "pmt":
                if(action == "end")
                {
                    GetComponent<Animator>().SetTrigger("hide");
                    Reset();
                } 
                break;
            case "dangunCha":
                dangunCha.BackToHome();
                break;
            case "OpenCard":
                if (int.TryParse(action, out int idx))
                {
                    print(cc.myCard[idx].count);
                    if(cc.myCard[idx].count == 1)
                    {
                        cc.OpenCard(idx, true);
                    }
                    else cc.OpenCard(idx);

                    CallAction("dangunCha", "BackToHome");
                }
                break;
            case "Nylon" :
                Main_control.Instance.Nylon(action);
                break;
            case "Nylon_f" :
                Main_control.Instance.Nylon_f(action);
                break;
            case "Tanghuru" :
                Main_control.Instance.Tanghuru(action);
                break;
            case "JangJokBal" :
                Main_control.Instance.JangJokBal(action);
                break;
            default :
                Debug.LogError("PMTCTRL : No Action Found for " + parent);
                break;
        } 
    }
}
