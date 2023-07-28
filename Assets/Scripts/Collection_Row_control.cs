using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class Collection_Row_control : MonoBehaviour
{
    public GameObject[] cards = new GameObject[4];
    public GameObject[] imgs = new GameObject[4];
    public GameObject[] names = new GameObject[4];
    public GameObject[] counts = new GameObject[4];

    Color alpha;
    bool started = false;

    private void Start()
    {
        started = true;
        alpha = Color.white;
        alpha.a = 0.025f;
    }

    public void SetCount(int count)
    {
        for(int i = 0; i<count; i++)
        {
            cards[i].SetActive(true);
        }
        for (int i = 3; i >= count; i--)
        {
            cards[i].SetActive(false);
        }
    }

    public async Task UpdateCard(int idx, Sprite img, string name, int count, bool debugMode)
    {
        if(!started) Start();

        imgs[idx].GetComponent<Image>().sprite = img;
        
        counts[idx].GetComponent<Text>().text = "" + count;

        if(count == 0 & !debugMode)
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
