using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

public class MiniGameEndScoreManager : MonoBehaviour
{
    [SerializeField] private Text score_text, rank_text, money_text, score_title, rank_title, money_title, score_msg, replay_text;
    [SerializeField] private Button closeBtn, replayBtn;
    [SerializeField] private Slider replayBtnTimer;
    [SerializeField] private Image scoreMsgImage;
    [SerializeField] private GameObject moneyCount;
    
    private Color color_transparent = new Color(0, 0, 0, 0);
    public enum MiniGameType
    {
        Tanghuru, Bossam
    }

    [SerializeField] private MiniGameType type;
    
    [Button]
    public void ShowScore(int score)
    {
#if UNITY_IOS && !UNITY_EDITOR
        Firebase.Analytics.FirebaseAnalytics
            .LogEvent("GameScore", type.ToString(), score);
#endif
        
        // if(gameObject.activeSelf == true) return;
        moneyCount.gameObject.SetActive(true);
        
        char rank = 'A';
        int money_score = 0;

        switch (type)
        {
            case MiniGameType.Tanghuru:
                if (score < 1000)
                {
                    score_msg.text = "손님들 불만이 이만저만이 아니었소.";
                    rank = 'F';
                    money_score = 0;
                } else if (score < 2000)
                {
                    score_msg.text = "혼자하는 것보다는 나았던 것 같소.";
                    rank = 'D';
                    money_score = 1000;
                } else if (score < 3000)
                {
                    score_msg.text = "덕분에 그런데로 끝낼 수 있었소.";
                    rank = 'C';
                    money_score = 3000;
                } else if (score < 3500)
                {
                    score_msg.text = "덕분에 무난하게 일을 끝낼 수 있었소.";
                    rank = 'B';
                    money_score = 6000;
                } else if (score < 4000)
                {
                    score_msg.text = "덕분에 아무 문제 없이 일을 끝마칠 수 있었소.";
                    rank = 'A';
                    money_score = 8000;
                }
                else
                {
                    score_msg.text = "덕분에 손님들이 아주 만족했소.";
                    rank = 'S';
                    money_score = 12000;
                }
                PlayerPrefs.SetString("tanghuru_rank", rank.ToString());
                break;
            case MiniGameType.Bossam:
                if (score < 700)
                {
                    score_msg.text = "정신 안 차리실래요오??";
                    rank = 'F';
                    money_score = 0;
                } else if (score < 1700)
                {
                    score_msg.text = "백지장도 맞들면 낫긴 하네요오!!";
                    rank = 'D';
                    money_score = 1000;
                } else if (score < 2500)
                {
                    score_msg.text = "사고는 안 났네요오!!";
                    rank = 'C';
                    money_score = 3000;
                } else if (score < 3200)
                {
                    score_msg.text = "손님들이 나름 만족해주셨어요오!!";
                    rank = 'B';
                    money_score = 6000;
                } else if (score < 4000)
                {
                    score_msg.text = "손님들이 좋은 후기를 남겨주셨어요오!!";
                    rank = 'A';
                    money_score = 8000;
                }
                else
                {
                    score_msg.text = "손님들의 눈에서 황홀함의 눈물이 흐르고 있어요오!!";
                    rank = 'S';
                    money_score = 12000;
                }
                PlayerPrefs.SetString("bossam_rank", rank.ToString());
                break;
        }
        
        
        score_text.color = color_transparent;
        rank_text.color = color_transparent;
        money_text.color = color_transparent;
        score_title.color = color_transparent;
        rank_title.color = color_transparent;
        money_title.color = color_transparent;
        score_msg.color = color_transparent;
        scoreMsgImage.color = new Color(1, 1, 1, 0);

        closeBtn.transform.localScale = Vector3.zero;
        replayBtn.transform.localScale = Vector3.zero;

        score_title.DOFade(1, 0.3f).SetDelay(0f);
        score_text.DOFade(1, 0.3f).SetDelay(0.15f);
        DOVirtual.Int(0, score, 0.5f, (score_tween) =>
        {
            score_text.text = score_tween + "점";
        }).SetDelay(0.15f);;
        
        rank_title.DOFade(1, 0.3f).SetDelay(0.75f);
        rank_text.DOFade(1, 0.3f).SetDelay(0.9f);
        rank_text.text = rank + "";
        rank_text.transform.DOPunchScale(Vector3.one, 1f).SetDelay(0.9f);;
        
        money_title.DOFade(1, 0.3f).SetDelay(1.5f);
        money_text.DOFade(1, 0.3f).SetDelay(1.65f);
        DOVirtual.Int(0, money_score, 0.5f, (money_tween) =>
        {
            money_text.text = money_tween + "냥";
        }).SetDelay(1.65f)
            .OnComplete(() =>
            {
                Heart_Control.Instance.UpdateMoney(money_score);                
            });
        
        
        score_msg.DOFade(1, 0.3f).SetDelay(2f);
        scoreMsgImage.DOFade(1,0.3f).SetDelay(2f);
        closeBtn.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(2f);
        replayBtn.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetDelay(2.3f);

        int adCount = PlayerPrefs.GetInt("Minigame_adCount", 0);
        replay_text.text = "광고보고 다시하기 (" + adCount + "/2)";;
        if (adCount >= 2)
        {
            replayBtn.interactable = false;
        }
        else
        {
            replayBtn.interactable = true;
            DOVirtual.Float(1f, 0f, 6f, (float_tween) =>
            {
                replayBtnTimer.value = float_tween;
            }).OnComplete(() => { replayBtn.interactable = false;}).SetEase(Ease.OutQuad).SetDelay(2.3f);
        }
        gameObject.SetActive(true);
    }

    public void HideScore()
    {
        if(gameObject.activeSelf == false) return;
        gameObject.SetActive(false);
        moneyCount.gameObject.SetActive(false);
    }
}
