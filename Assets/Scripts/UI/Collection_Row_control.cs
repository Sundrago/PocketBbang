using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Collection_Row_control : MonoBehaviour
{
    [SerializeField] public GameObject[] cards = new GameObject[4];
    [SerializeField] private GameObject[] imgs = new GameObject[4];
    [SerializeField] private GameObject[] names = new GameObject[4];
    [SerializeField] private GameObject[] counts = new GameObject[4];

    private Color alpha;
    private bool started;

    private void Start()
    {
        started = true;
        alpha = Color.white;
        alpha.a = 0.025f;
    }

    public void SetCount(int count)
    {
        for (var i = 0; i < count; i++) cards[i].SetActive(true);
        for (var i = 3; i >= count; i--) cards[i].SetActive(false);
    }

    public async Task UpdateCard(int idx, Sprite img, string name, int count, bool debugMode)
    {
        if (!started) Start();

        imgs[idx].GetComponent<Image>().sprite = img;

        counts[idx].GetComponent<Text>().text = "" + count;

        if ((count == 0) & !debugMode)
        {
            imgs[idx].GetComponent<Image>().color = alpha;
            names[idx].GetComponent<Text>().text = "?";
        }
        else
        {
            names[idx].GetComponent<Text>().text = name;
            imgs[idx].GetComponent<Image>().color = Color.white;
        }

        await Task.Delay(5);
        await Task.Yield();
    }
}