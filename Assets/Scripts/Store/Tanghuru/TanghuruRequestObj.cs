using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;



public class TanghuruRequestObj : MonoBehaviour
{
    [SerializeField] private GameObject soldOut_ui;
    [SerializeField] private Text counter_text;
    [SerializeField] private TanghuruStick tanghuruStick;

    private int count;
    public bool finished = false;
    
    public string GetIds()
    {
        return tanghuruStick.GetIds();
    }

    public void MakeNewOrder(int level)
    {
        tanghuruStick.Init();
        // int type = Random.Range(0, 4);
        //
        // if (type == 0 || type == 1)
        // {
        //     int rnd = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
        //     for (int i = 0; i < 4; i++)
        //     {
        //         tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
        //     }
        //     count = Random.Range(1, 4);
        // }
        // else if (type == 2)
        // {
        //     int rnd = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
        //     for (int i = 0; i < 2; i++)
        //     {
        //         tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
        //     }
        //     rnd = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
        //     for (int i = 0; i < 2; i++)
        //     {
        //         tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
        //     }
        //     count = Random.Range(1, 3);
        // }
        // else
        // {
        //     for (int i = 0; i < 4; i++)
        //     {
        //         int rnd = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
        //         tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
        //     }
        //     count = Random.Range(1, 2);
        // }
        // count = Random.Range(1, 4);
        print("lv:" + level);
        switch (level)
        {
            
            case 0 :
                SetDifficulty(1f,0f,0f);
                break;
            case 1 :
                SetDifficulty(0.7f,0.3f,0f);
                break;
            case 2 :
                SetDifficulty(0.6f,0.3f,0.1f);
                break;
            case 3 :
                SetDifficulty(0.5f,0.4f,0.1f);
                break;
            case 4 :
                SetDifficulty(0.4f,0.5f,0.1f);
                break;
            case 5 :
                SetDifficulty(0.3f,0.5f,0.2f);
                break;
            case 6 :
                SetDifficulty(0.2f,0.5f,0.3f);
                break;
            default: 
                SetDifficulty(0.1f,0.3f,0.6f);
                break;
            
        }

        count = 1;
        UpdateUI();
        finished = false;
        soldOut_ui.SetActive(false);

        void SetDifficulty(float easy, float mid, float hard)
        {
            float total = easy + mid + hard;
            float rnd = Random.Range(0, total);
            
            if(rnd < easy) Easy();
            else if(rnd<easy+mid) Mid();
            else Hard();
        }
        
        void Easy()
        {
            int rnd = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
            for (int i = 0; i < 4; i++)
            {
                tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
            }
            count = 1;
        }

        void Mid()
        {
            int type = Random.Range(0, 3);
            int rnd, rnd2;
            switch (type)
            {
                case 0 : 
                    rnd = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
                    for (int i = 0; i < 4; i++)
                    {
                        tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
                    }
                    count = Random.Range(1,3);
                    break;
                case 1 : 
                    rnd = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
                    rnd2 = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
                    tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
                    tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd2], rnd2);
                    tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
                    tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd2], rnd2);
                    count = Random.Range(1,3);
                    break;
                default :
                    rnd = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
                    for (int i = 0; i < 2; i++)
                    {
                        tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
                    }
                    rnd = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
                    for (int i = 0; i < 2; i++)
                    {
                        tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
                    }
                    count = Random.Range(1,3);
                    break;
            }
        }

        void Hard()
        {
            int type = Random.Range(0, 2);
            int rnd;
            switch (type)
            {
                case 0 :
                    for (int i = 0; i < 4; i++)
                    {
                        rnd = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
                        tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
                    }
                    count = Random.Range(1, 3);
                    break;
                case 1 : 
                    rnd = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
                    tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
                    rnd = Random.Range(0, TanghuruGameManager.Instance.fruits_sprite.Count/2);
                    for (int i = 0; i < 3; i++)
                    {
                        tanghuruStick.AddFruits(TanghuruGameManager.Instance.fruits_sprite[rnd], rnd);
                    }
                    count = Random.Range(1, 3);
                    break;
            }
        }
    }
    

    public void CompleteOrder()
    {
        count -= 1;
        UpdateUI();
    }
    
    public void UpdateUI()
    {
        if (count <= 0)
        {
            finished = true;
            soldOut_ui.SetActive(true);
        }
        counter_text.text = count.ToString();
    }
}
