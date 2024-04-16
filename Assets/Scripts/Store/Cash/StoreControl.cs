using UnityEngine;
using UnityEngine.UI;

public class StoreControl : MonoBehaviour
{
    [SerializeField] private Sprite[] storeImgs;
    [SerializeField] private GameObject storeImg_ui;
    [SerializeField] private GameObject me;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject cloud;
    [SerializeField] private GameObject bbobgiMachine;
    [SerializeField] public BbobgiPanelController bbobgiPanel;
    [SerializeField] private GameObject scrumb;

    public void UpdateStore(int i)
    {
        storeImg_ui.GetComponent<Image>().sprite = storeImgs[i];

        if (i == 10) cloud.SetActive(true);
        else cloud.SetActive(false);

        if (i == 5)
        {
            bbobgiMachine.SetActive(true);
            bbobgiPanel.Reload = true;
        }
        else
        {
            bbobgiMachine.SetActive(false);
        }

        float rnd;
        if (PlayerPrefs.GetInt("totalScrubCount", 0) < 10) rnd = 0.6f;
        else if (PlayerPrefs.GetInt("totalScrubCount", 0) < 30) rnd = 0.5f;
        else if (PlayerPrefs.GetInt("totalScrubCount", 0) < 100) rnd = 0.4f;
        else rnd = 0.3f;
        scrumb.gameObject.SetActive(Random.value < rnd);
    }

    public void DestroySelf()
    {
        Destroy(me);
    }

    public void WentToStore()
    {
        mainPanel.GetComponent<GameManager>().WentToStore();
    }

    public void WentToPark()
    {
        if (mainPanel.GetComponent<GameManager>().gointToPark)
            mainPanel.GetComponent<GameManager>().WentToPark();
    }

    public void StartBbobgi()
    {
        bbobgiPanel.GetComponent<BbobgiPanelController>().OpenGame();
    }

    public void ScrumbBtnClicked()
    {
        int rnd;
        if (PlayerPrefs.GetInt("totalScrubCount", 0) < 100) rnd = Random.Range(1, 3);
        else rnd = 1;

        RewardItemManager.Instance.Init(new int[1] { 1001 }, new int[1] { rnd }, "scrumb", "반짝이는 빵 부스러기를 주웠다!");
        scrumb.gameObject.SetActive(false);
    }
}