using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TanghuruStick : MonoBehaviour
{
    [SerializeField] private List<Image> fruits = new();
    public List<int> ids = new();

    private int fruitCount;

    // private void Start()
    // {
    //     Init();
    // }

    public void Init()
    {
        foreach (var image in fruits)
        {
            image.sprite = null;
            image.gameObject.SetActive(false);
        }

        ids = new List<int>();
        fruitCount = 0;
    }

    public void AddFruits(Sprite sprite, int idx)
    {
        if (fruitCount >= fruits.Count) return;

        fruits[fruitCount].sprite = sprite;
        fruits[fruitCount].gameObject.SetActive(true);
        ids.Add(idx);
        fruitCount += 1;
    }

    public GameObject GetFruitsTargetTransform()
    {
        if (fruitCount >= fruits.Count) return null;
        return fruits[fruitCount].gameObject;
    }

    public string GetIds()
    {
        var output = "";
        foreach (var id in ids) output += id;
        return output;
    }

    public int GetFruitCount()
    {
        return fruitCount;
    }

    public void Icing()
    {
        for (var i = 0; i < 4; i++)
        {
            fruits[i].sprite = TanghuruGameManager.Instance.fruits_sprite[8 + ids[i]];
            if (i >= ids.Count) return;
            print(8 + i);
        }
    }
}