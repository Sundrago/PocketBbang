using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Bossam_StageManager : MonoBehaviour
{
    [SerializeField] private Transform single_pos;
    [SerializeField] private Transform[] char_pos;
    [SerializeField] private Bossam_character[] char_prefabs;
    
    [TableList, SerializeField] public List<Bossam_charData> chardata;
    [Button]
    public void AnimIn()
    {
        chardata = new List<Bossam_charData>();
        //init
        int rnd = Random.Range(0, 3);
        int[] charidx = new int[3];
        charidx[0] = Random.Range(0, 3);
        charidx[1] = Random.Range(0, 3);
        charidx[2] = Random.Range(0, 3);
        
        while (charidx[1] == charidx[0])
        {
            charidx[1] = Random.Range(0, 3);
        }
        while (charidx[2] == charidx[0] || charidx[2] == charidx[1] )
        {
            charidx[2] = Random.Range(0, 3);
        }
        
        switch (rnd)
        {
            case 0:
                //one char
                AddChild(0, charidx[0]);
                break;
            case 1:
                //two char
                AddChild(1, charidx[0]);
                AddChild(2, charidx[1]);
                break;
            case 2:
                AddChild(1, charidx[0]);
                AddChild(2, charidx[1]);
                AddChild(3, charidx[2]);
                break;
            
        }
        
        foreach (var character in chardata)
        {
            character.character.CloseMouth();
        }
        gameObject.transform.localPosition = new Vector3(1000, gameObject.transform.localPosition
            .y, 0);
        gameObject.transform.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            foreach (var character in chardata)
            {
                if (character.mouthOpen)
                    character.character.OpenMouth();
            }
        });

        void AddChild(int posIdx, int charIdx)
        {
            Bossam_character newChar = Instantiate(char_prefabs[charIdx], gameObject.transform);
            newChar.transform.position = char_pos[posIdx].position;
            chardata.Add(new Bossam_charData(true, newChar));
        }
    }
   
    [Button]
    public void AnimOut()
    {
        gameObject.transform.DOLocalMoveX(-1000, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    public void GotBossam(int idx)
    {
        chardata[idx].mouthOpen = false;
        chardata[idx].character.CloseMouth();
        
        bool finishedStage = true;
        foreach (var character in chardata)
        {
            if (character.mouthOpen)
            {
                finishedStage = false;
                break;
            }
        }
        
        if(finishedStage) StageCleaer();
    }

    private void StageCleaer()
    {
        AnimOut();
        Bossam_GameManager.Instance.StageClear();
    }

    [Serializable]
    public class Bossam_charData
    {
        public bool mouthOpen;
        public Bossam_character character;

        public Bossam_charData(bool _mouthOpen, Bossam_character _character)
        {
            mouthOpen = _mouthOpen;
            character = _character;
        }
    }
}
