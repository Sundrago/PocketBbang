using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection_Panel_Control : MonoBehaviour
{
    const int maxRowCount = 36;

    [SerializeField] CollectionControl cc;
    [SerializeField] AudioControl myAudio;
    [SerializeField] BalloonControl balloon;

    [SerializeField] GameObject row, sampleImg, canvasHolder;
    [SerializeField] GameObject finishBtn_ui, exitBtn_ui, btmTabPanel;
    [SerializeField] GameObject card_panel;
    [SerializeField] GameObject Arank_ui, Brank_ui, Srank_ui, Null_ui, Mrank_ui;

    [SerializeField] public List<GameObject> rows = new List<GameObject>();
    [SerializeField] public List<GameObject> btmTab = new List<GameObject>();
    [SerializeField] public List<Text> btmTabName = new List<Text>();
    [SerializeField] public List<int> mySelectedIdx = new List<int>();

    public bool selectionMode = false;

    bool started = false;
    bool hasValue = false;

    int originalMusic;
    int selectionCardCount = 2;
    char selectionCardChar = 'A';

    public class CardData
    {
        public int IDX_code;
        public int idx_sort;
        public int count;
    }

    public List<CardData> Scards = new List<CardData>();
    public List<CardData> Acards = new List<CardData>();
    public List<CardData> Bcards = new List<CardData>();
    public List<CardData> Mcards = new List<CardData>();

    public List<GameObject> Srows = new List<GameObject>();
    public List<GameObject> Arows = new List<GameObject>();
    public List<GameObject> Brows = new List<GameObject>();
    public List<GameObject> Mrows = new List<GameObject>();

    public List<CardData> Androidcards = new List<CardData>();
    public List<GameObject> AndroidRows = new List<GameObject>();

    [SerializeField] List<Button> deckBtns = new List<Button>();


    public bool bookMode = true;
    public bool debugMode;

    public Sprite checkbox_0, checkbox_1;
    public Image checkbox_icon_ui;

    public void Start()
    {
        if(!PlayerPrefs.HasKey("CollectionTab")) PlayerPrefs.SetInt("CollectionTab", 0);

        if (started) return;

        started = true;
        gameObject.GetComponent<SetMyActive>().Start();
        row.SetActive(false);

        GameObject newRow = Instantiate(row, canvasHolder.transform);
        newRow.SetActive(true);
        rows.Add(newRow);

        cc = card_panel.GetComponent<CollectionControl>();
        cc.Start();

        CheckBoxClicked();
    }

    void ReadData()
    {
        Scards.Clear();
        Acards.Clear();
        Bcards.Clear();
        Mcards.Clear();

        for (int i = 0; i < cc.myCard.Count; i++)
        {
            if (selectionMode)
            {
                if (cc.myCard[i].count < selectionCardCount) continue;
                if (cc.myCard[i].rank != selectionCardChar) continue;
            }
            if(cc.myCard[i].count > 0 | bookMode == true | debugMode == true)
            {
                
                CardData myCardData = new CardData();
                myCardData.IDX_code = i;
                myCardData.idx_sort = cc.myCard[i].idx;
                myCardData.count = cc.myCard[i].count;

                switch (cc.myCard[i].rank)
                {
                    case 'A':
                        Acards.Add(myCardData);
                        break;
                    case 'B':
                        Bcards.Add(myCardData);
                        break;
                    case 'S':
                        Scards.Add(myCardData);
                        break;
                    case 'M':
                        Mcards.Add(myCardData);
                        break;
                    default:
                        print("Error loading card rank on idx : " + i);
                        break;
                }
            }
        }
    }

    public void GetCount()
    {
        Start();
        bookMode = false;
        debugMode = false;
        ReadData();
        int myTotalCards = Scards.Count + Acards.Count + Bcards.Count + Mcards.Count;
        PlayerPrefs.SetInt("myTotalCards", myTotalCards);
        print("My Total Cards : " + myTotalCards);
        PlayerPrefs.Save();
    }

    public async void UpdateData()
    {
        //SetCount(myCardList.Count);

        if(PlayerPrefs.GetInt("lowDataMode")==1)
        {
            SetCounts(0, 0, 0, 0);
            UpdateAndroidPanel();
            return;
        }

        //Reset Rows
        foreach (GameObject item in AndroidRows)
        {
            Destroy(item);
        }
        AndroidRows.Clear();

        Arank_ui.SetActive(true);
        Brank_ui.SetActive(true);
        Null_ui.SetActive(false);
        btmTabPanel.SetActive(false);
        Mrank_ui.SetActive(true);

        SetCounts(Acards.Count, Bcards.Count, Scards.Count, Mcards.Count);

        //Update S data
        for (int i = 0; i < Scards.Count; i++)
        {
            await Srows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().UpdateCard(i % 4, cc.myCard[Scards[i].IDX_code].image, cc.myCard[Scards[i].IDX_code].name, cc.myCard[Scards[i].IDX_code].count, debugMode);
            int idx = new int();
            idx = Scards[i].IDX_code;

            Card_control myCard = Srows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Card_control>();
            if (selectionMode)
            {
                myCard.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SelectionHandler(myCard, idx); });
                hasValue = true;
                continue;
            }
            else myCard.SetSelectedUI(false);

            if (Scards[i].count == 0 & !debugMode)
            {
                Srows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Button>().onClick.AddListener(delegate { cc.OpenCard(-1); });
            }
            else
            {
                Srows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Button>().onClick.AddListener(delegate { cc.OpenCard(idx); });
            }
        }

        //Update M data
        for (int i = 0; i < Mcards.Count; i++)
        {
            await Mrows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().UpdateCard(i % 4, cc.myCard[Mcards[i].IDX_code].image, cc.myCard[Mcards[i].IDX_code].name, cc.myCard[Mcards[i].IDX_code].count, debugMode);
            int idx = new int();
            idx = Mcards[i].IDX_code;

            Card_control myCard = Mrows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Card_control>();
            if (selectionMode)
            {
                myCard.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SelectionHandler(myCard, idx); });
                hasValue = true;
                continue;
            }
            else myCard.SetSelectedUI(false);

            if (Mcards[i].count == 0 & !debugMode)
            {
                Mrows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Button>().onClick.AddListener(delegate { cc.OpenCard(-1); });
            }
            else
            {
                Mrows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Button>().onClick.AddListener(delegate { cc.OpenCard(idx); });
            }
        }

        //Update A data
        for (int i = 0; i < Acards.Count; i++)
        {
            await Arows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().UpdateCard(i % 4, cc.myCard[Acards[i].IDX_code].image, cc.myCard[Acards[i].IDX_code].name, cc.myCard[Acards[i].IDX_code].count, debugMode);
            int idx = new int();
            idx = Acards[i].IDX_code;

            if (selectionMode)
            {
                Card_control myCard = Arows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Card_control>();
                myCard.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SelectionHandler(myCard, idx); });
                hasValue = true;
                continue;
            }

            if (Acards[i].count == 0 & !debugMode)
            {
                Arows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Button>().onClick.AddListener(delegate { cc.OpenCard(-1); });
            }
            else
            {
                Arows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Button>().onClick.AddListener(delegate { cc.OpenCard(idx); });
            }
        }

        //Update B data
        for (int i = 0; i < Bcards.Count; i++)
        {
            await Brows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().UpdateCard(i % 4, cc.myCard[Bcards[i].IDX_code].image, cc.myCard[Bcards[i].IDX_code].name, cc.myCard[Bcards[i].IDX_code].count, debugMode);
            int idx = new int();
            idx = Bcards[i].IDX_code;

            if (selectionMode)
            {
                Card_control myCard = Brows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Card_control>();
                myCard.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SelectionHandler(myCard, idx); });
                hasValue = true;
                continue;
            }

            if (Bcards[i].count == 0 & !debugMode)
            {
                Brows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Button>().onClick.AddListener(delegate { cc.OpenCard(-1); });
            }
            else
            {
                Brows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Button>().onClick.AddListener(delegate { cc.OpenCard(idx); });
            }
        }

        /*
        for (int i = 0; i<myCardList.Count; i++)
        {
            rows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().UpdateCard(i % 4, cc.myCard[myCardList[i]].image, cc.myCard[myCardList[i]].name, cc.myCard[myCardList[i]].count);
            int idx = new int();
            idx = myCardList[i];
            rows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i%4].GetComponent<Button>().onClick.AddListener(delegate { cc.OpenCard(idx); });
        }
        */
    }

    public void Read(int i)
    {
        //print(i);
    }

    public void SetCounts(int Acount, int Bcount, int Scount, int Mcount)
    {
        /*
        //Deactivate when empty
        if (Acount + Bcount == 0)
        {
            foreach (GameObject item in Arows)
            {
                item.SetActive(false);
            }
            foreach (GameObject item in Brows)
            {
                item.SetActive(false);
            }
            return;
        } else
        {
            foreach (GameObject item in Arows)
            {
                item.SetActive(true);
            }
            foreach (GameObject item in Brows)
            {
                item.SetActive(true);
            }
        }
        */

        foreach (GameObject item in Arows)
        {
            Destroy(item);
        }
        foreach (GameObject item in Brows)
        {
            Destroy(item);
        }
        foreach (GameObject item in Srows)
        {
            Destroy(item);
        }
        foreach (GameObject item in Mrows)
        {
            Destroy(item);
        }

        Srows.Clear();
        Arows.Clear();
        Brows.Clear();
        Mrows.Clear();

        //Get row Count
        int SrowCount = Mathf.FloorToInt(Scount / 4);
        if (Scount % 4 != 0) SrowCount += 1;

        int MrowCount = Mathf.FloorToInt(Mcount / 4);
        if (Mcount % 4 != 0) MrowCount += 1;

        int ArowCount = Mathf.FloorToInt(Acount / 4);
        if (Acount % 4 != 0) ArowCount += 1;

        int BrowCount = Mathf.FloorToInt(Bcount / 4);
        if (Bcount % 4 != 0) BrowCount += 1;

        //print("ACount = " + Acount + "  ArowCount = " + ArowCount);
        //print("BCount = " + Bcount + "  BrowCount = " + BrowCount);

        //Set S Row GameObject Sets
        if (Srows.Count < SrowCount)
        {
            int addCount = SrowCount - Srows.Count;
            //print("RowCount " + addCount);

            for (int i = 0; i < addCount; i++)
            {
                GameObject newRow = Instantiate(row, canvasHolder.transform);
                newRow.SetActive(true);
                Srows.Add(newRow);
            }
        }
        else if (Srows.Count > SrowCount)
        {
            int addCount = Srows.Count - SrowCount;

            for (int i = 0; i < addCount; i++)
            {
                Destroy(Srows[Srows.Count - 1]);
            }
            Srows.RemoveAll(s => s == null);
        }

        //Set M Row GameObject Sets
        if (Mrows.Count < MrowCount)
        {
            int addCount = MrowCount - Mrows.Count;
            //print("RowCount " + addCount);

            for (int i = 0; i < addCount; i++)
            {
                GameObject newRow = Instantiate(row, canvasHolder.transform);
                newRow.SetActive(true);
                Mrows.Add(newRow);
            }
        }
        else if (Mrows.Count > MrowCount)
        {
            int addCount = Mrows.Count - MrowCount;

            for (int i = 0; i < addCount; i++)
            {
                Destroy(Mrows[Mrows.Count - 1]);
            }
            Mrows.RemoveAll(s => s == null);
        }

        //Set A Row GameObject Sets
        if (Arows.Count < ArowCount)
        {
            int addCount = ArowCount - Arows.Count;
            //print("RowCount " + addCount);

            for (int i = 0; i < addCount; i++)
            {
                GameObject newRow = Instantiate(row, canvasHolder.transform);
                newRow.SetActive(true);
                Arows.Add(newRow);
            }
        }
        else if (Arows.Count > ArowCount)
        {
            int addCount = Arows.Count - ArowCount;

            for (int i = 0; i < addCount; i++)
            {
                Destroy(Arows[Arows.Count - 1]);
            }
            Arows.RemoveAll(s => s == null);
        }

        //Set B Row GameObject Sets
        if (Brows.Count < BrowCount)
        {
            int addCount = BrowCount - Brows.Count;
            //print("RowCount " + addCount);

            for (int i = 0; i < addCount; i++)
            {
                GameObject newRow = Instantiate(row, canvasHolder.transform);
                newRow.SetActive(true);
                Brows.Add(newRow);
            }
        }
        else if (Brows.Count > BrowCount)
        {
            int addCount = Brows.Count - BrowCount;

            for (int i = 0; i < addCount; i++)
            {
                Destroy(Brows[Brows.Count - 1]);
            }
            Brows.RemoveAll(s => s == null);
        }

        //Set S Position
        for (int i = 0; i < Srows.Count; i++)
        {
            if (i != Srows.Count - 1 | Scount % 4 == 0)
                Srows[i].GetComponent<Collection_Row_control>().SetCount(4);
            else
                Srows[i].GetComponent<Collection_Row_control>().SetCount(Scount % 4);

            if (i == 0)
            {
                Srows[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(Srows[i].GetComponent<RectTransform>().anchoredPosition.x, Srank_ui.GetComponent<RectTransform>().anchoredPosition.y - 220f);
            }
            if (i != 0)
            {
                Srows[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(Srows[i].GetComponent<RectTransform>().anchoredPosition.x, Srows[i - 1].GetComponent<RectTransform>().anchoredPosition.y - 310f);
            }
        }

        //Set M Position
        if (Scount > 0) Mrank_ui.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mrank_ui.GetComponent<RectTransform>().anchoredPosition.x, Srows[Srows.Count - 1].GetComponent<RectTransform>().anchoredPosition.y - 200f);
        else Mrank_ui.GetComponent<RectTransform>().anchoredPosition = new Vector2(Mrank_ui.GetComponent<RectTransform>().anchoredPosition.x, Srank_ui.GetComponent<RectTransform>().anchoredPosition.y - 225);
        for (int i = 0; i < Mrows.Count; i++)
        {
            if (i != Mrows.Count - 1 | Mcount % 4 == 0)
                Mrows[i].GetComponent<Collection_Row_control>().SetCount(4);
            else
                Mrows[i].GetComponent<Collection_Row_control>().SetCount(Mcount % 4);

            if (i == 0)
            {
                Mrows[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(Mrows[i].GetComponent<RectTransform>().anchoredPosition.x, Mrank_ui.GetComponent<RectTransform>().anchoredPosition.y - 220f);
            }
            if (i != 0)
            {
                Mrows[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(Mrows[i].GetComponent<RectTransform>().anchoredPosition.x, Mrows[i - 1].GetComponent<RectTransform>().anchoredPosition.y - 310f);
            }
        }

        //Set A Position
        if (Mcount > 0) Arank_ui.GetComponent<RectTransform>().anchoredPosition = new Vector2(Arank_ui.GetComponent<RectTransform>().anchoredPosition.x, Mrows[Mrows.Count - 1].GetComponent<RectTransform>().anchoredPosition.y - 200f);
        else Arank_ui.GetComponent<RectTransform>().anchoredPosition = new Vector2(Arank_ui.GetComponent<RectTransform>().anchoredPosition.x, Mrank_ui.GetComponent<RectTransform>().anchoredPosition.y - 225);
        for (int i = 0; i < Arows.Count; i++)
        {
            if (i != Arows.Count - 1 | Acount % 4 == 0)
                Arows[i].GetComponent<Collection_Row_control>().SetCount(4);
            else
                Arows[i].GetComponent<Collection_Row_control>().SetCount(Acount % 4);

            if(i == 0)
            {
                Arows[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(Arows[i].GetComponent<RectTransform>().anchoredPosition.x, Arank_ui.GetComponent<RectTransform>().anchoredPosition.y - 220f);
            }
            if (i != 0)
            {
                Arows[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(Arows[i].GetComponent<RectTransform>().anchoredPosition.x, Arows[i - 1].GetComponent<RectTransform>().anchoredPosition.y - 310f);
            }
        }


        //Set B Position
        if(Acount>0) Brank_ui.GetComponent<RectTransform>().anchoredPosition = new Vector2(Brank_ui.GetComponent<RectTransform>().anchoredPosition.x, Arows[Arows.Count - 1].GetComponent<RectTransform>().anchoredPosition.y - 200f);
        else Brank_ui.GetComponent<RectTransform>().anchoredPosition = new Vector2(Brank_ui.GetComponent<RectTransform>().anchoredPosition.x, Arank_ui.GetComponent<RectTransform>().anchoredPosition.y - 225);
        for (int i = 0; i < Brows.Count; i++)
        {
            if (i != Brows.Count - 1 | Bcount % 4 == 0)
                Brows[i].GetComponent<Collection_Row_control>().SetCount(4);
            else
                Brows[i].GetComponent<Collection_Row_control>().SetCount(Bcount % 4);

            if (i == 0)
            {
                Brows[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(Brows[i].GetComponent<RectTransform>().anchoredPosition.x, Brank_ui.GetComponent<RectTransform>().anchoredPosition.y - 220f);
            }
            if (i != 0)
            {
                Brows[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(Brows[i].GetComponent<RectTransform>().anchoredPosition.x, Brows[i - 1].GetComponent<RectTransform>().anchoredPosition.y - 310f);
            }
        }

        //Set Canvas Scale

        if(Bcount > 0) canvasHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasHolder.GetComponent<RectTransform>().sizeDelta.x, (Brows[Brows.Count - 1].GetComponent<RectTransform>().anchoredPosition.y - 200) * -1);
        else if(Acount > 0) canvasHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasHolder.GetComponent<RectTransform>().sizeDelta.x, (Arows[Arows.Count - 1].GetComponent<RectTransform>().anchoredPosition.y - 200) * -1);
    }

    public void ShowPanel()
    {
        finishBtn_ui.SetActive(false);
        exitBtn_ui.SetActive(true);
        gameObject.SetActive(true);
        selectionMode = false;

        cc.LoadData();
        ReadData();
        UpdateData();
        GetComponent<Animator>().SetTrigger("show");
        originalMusic = myAudio.currentPlaying;
        myAudio.PlayMusic(4);
    }

    public void HidePanel()
    {
        GetComponent<Animator>().SetTrigger("hide");
        debugMode = false;
        myAudio.PlayMusic(originalMusic);
    }

    public void BtmTabClicked(int idx)
    {
        int currentTab = PlayerPrefs.GetInt("CollectionTab");
        //if (currentTab == idx) return;

        PlayerPrefs.SetInt("CollectionTab", idx);
        
        UpdateAndroidPanel();
    }

    public async void UpdateAndroidPanel()
    {
        btmTabPanel.SetActive(true);
        foreach(Button btn in deckBtns) {
            btn.interactable = false;
        }

        //BottomBtnSetup Color
        int currentTab = PlayerPrefs.GetInt("CollectionTab");
        for(int i = 0; i<btmTab.Count; i++)
        {
            if(i != currentTab) btmTab[i].GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            else btmTab[i].GetComponent<Image>().color = new Color(1f, 0.9f, 0.5f);
        }

        //Set Btn UI Text name and SetActive
        int idx = 0;
        btmTabName[idx].text = "S등급";
        btmTab[idx].SetActive(true);
        idx += 1;
        btmTabName[idx].text = "A등급";
        btmTab[idx].SetActive(true);
        idx += 1;
        if (Acards.Count > maxRowCount)
        {
            btmTabName[idx].text = "A등급(2)";
            btmTab[idx].SetActive(true);
            idx += 1;
        }
        btmTabName[idx].text = "B등급";
        btmTab[idx].SetActive(true);
        idx += 1;
        if (Bcards.Count > maxRowCount)
        {
            btmTabName[idx].text = "B등급(2)";
            btmTab[idx].SetActive(true);
            idx += 1;
        }
        for(int i = idx; i<5; i++)
        {
            btmTab[i].SetActive(false);
        }

        //UI Title setting
        Srank_ui.GetComponent<Text>().text = btmTabName[currentTab].text;
        Arank_ui.SetActive(false);
        Brank_ui.SetActive(false);
        Mrank_ui.SetActive(false);

        //Reset Rows
        foreach (GameObject item in AndroidRows)
        {
            Destroy(item);
        }
        AndroidRows.Clear();
        Androidcards = new List<CardData>();

        //AssignData
        switch (btmTabName[currentTab].text)
        {
            case "S등급":
                for (int i = 0; i < Scards.Count; i++)
                {
                    Androidcards.Add(Scards[i]);
                }
                for (int i = 0; i<Mcards.Count; i++)
                {
                    Androidcards.Add(Mcards[i]);
                }
                break;
            case "A등급":
                if (Acards.Count > maxRowCount) Androidcards = Acards.GetRange(0, maxRowCount);
                else Androidcards = Acards;
                break;
            case "A등급(2)":
                Androidcards = Acards.GetRange(maxRowCount, Acards.Count - maxRowCount);
                break;
            case "B등급":
                if (Bcards.Count > maxRowCount) Androidcards = Bcards.GetRange(0, maxRowCount);
                else Androidcards = Bcards;
                break;
            case "B등급(2)":
                Androidcards = Bcards.GetRange(maxRowCount, Bcards.Count - maxRowCount);
                break;
        }

        int AndroidRowCount = Mathf.CeilToInt(Androidcards.Count/4);
        if (Androidcards.Count % 4 != 0) AndroidRowCount += 1;

        if (Androidcards.Count == 0)
        {
            AndroidRowCount = 0;
            Null_ui.SetActive(true);
        } else
        {
            Null_ui.SetActive(false);
        }
        int cardCount = Androidcards.Count;
        print("rowCount : " + AndroidRowCount);

        //Setup Rows
        for (int i = 0; i < AndroidRowCount; i++)
        {
            if (AndroidRowCount == 0) continue;
            GameObject newRow = Instantiate(row, canvasHolder.transform);
            newRow.SetActive(true);
            AndroidRows.Add(newRow);

            //AndroidRows[i].GetComponent<Collection_Row_control>().SetCount(4);

            if (i == AndroidRowCount-1 & cardCount % 4 != 0)
            {
                AndroidRows[i].GetComponent<Collection_Row_control>().SetCount(cardCount % 4);
            }
            else AndroidRows[i].GetComponent<Collection_Row_control>().SetCount(4);

            if (i == 0)
            {
                AndroidRows[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(AndroidRows[i].GetComponent<RectTransform>().anchoredPosition.x, Srank_ui.GetComponent<RectTransform>().anchoredPosition.y - 220f);
            }
            if (i != 0)
            {
                AndroidRows[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(AndroidRows[i].GetComponent<RectTransform>().anchoredPosition.x, AndroidRows[i - 1].GetComponent<RectTransform>().anchoredPosition.y - 310f);
            }
        }

        //Update Data
        for (int i = 0; i < Androidcards.Count; i++)
        {
            if (AndroidRowCount == 0) continue;
            await AndroidRows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().UpdateCard(i % 4, cc.myCard[Androidcards[i].IDX_code].image, cc.myCard[Androidcards[i].IDX_code].name, cc.myCard[Androidcards[i].IDX_code].count, debugMode);

            idx = new int();
            idx = Androidcards[i].IDX_code;

            //Add Click Event Handler
            Card_control myCard = AndroidRows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Card_control>();
            if (selectionMode)
            {
                int myIdx = idx;
                myCard.gameObject.GetComponent<Button>().onClick.AddListener(delegate { SelectionHandler(myCard, myIdx); });
                hasValue = true;

                if(mySelectedIdx.Contains(myIdx)) myCard.SetSelectedUI(true);
                continue;
            } else myCard.SetSelectedUI(false);

            if (Androidcards[i].count == 0 & !debugMode)
            {
                AndroidRows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Button>().onClick.AddListener(delegate { cc.OpenCard(-1); });
            }
            else
            {
                Button myBtn = AndroidRows[Mathf.FloorToInt(i / 4)].GetComponent<Collection_Row_control>().cards[i % 4].GetComponent<Button>();
                int myIdx  = idx;
                myBtn.onClick.AddListener(delegate { cc.OpenCard(myIdx); });
                //print(idx);
            }
        }

        //Setup Slider Panel Height
        if(Androidcards.Count > 0)
        canvasHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasHolder.GetComponent<RectTransform>().sizeDelta.x, (AndroidRows[AndroidRowCount-1].GetComponent<RectTransform>().anchoredPosition.y - 300) * -1);
        else canvasHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasHolder.GetComponent<RectTransform>().sizeDelta.x, (Srank_ui.GetComponent<RectTransform>().anchoredPosition.y - 300) * -1);

        foreach(Button btn in deckBtns) {
            btn.interactable = true;
        }
    }

    public void CheckBoxClicked()
    {
        if(bookMode)
        {
            bookMode = false;
            checkbox_icon_ui.sprite = checkbox_0;
        } else
        {
            bookMode = true;
            checkbox_icon_ui.sprite = checkbox_1;
        }

        ReadData();
        UpdateData();
        PlayerPrefs.SetInt("CollectionTab", 1);
    }

    void SelectionHandler(Card_control myCard, int idx)
    {
        myCard.SetSelectedUI(false);

        if (mySelectedIdx.Contains(idx))
        {
            myCard.SetSelectedUI(false);
            mySelectedIdx.Remove(idx);
        } else
        {
            if(mySelectedIdx.Count >= 3)
            {
                balloon.ShowMsg("최대 세 종류의 카드를\n선택할 수 있어요!");
                return;
            }
            myCard.SetSelectedUI(true);
            mySelectedIdx.Add(idx);
        }
    }

    public bool UpdateAndReturnSelectionAvailability(int count, char rank)
    {
        Start();
        selectionMode = true;
        selectionCardCount = count;
        hasValue = false;
        selectionCardChar = rank;
        mySelectedIdx.Clear();

        cc.LoadData();
        ReadData();
        UpdateData();
        return hasValue;
    }

    public void ShowSelectionPanel()
    {
        finishBtn_ui.SetActive(true);
        exitBtn_ui.SetActive(false);
        mySelectedIdx.Clear();
        gameObject.SetActive(true);
        GetComponent<Animator>().SetTrigger("show");
    }

    public List<int> ClosePanelAndReturnSelectionIdx()
    {
        selectionMode = false;
        GetComponent<Animator>().SetTrigger("hide");

        return mySelectedIdx;
    }
}
