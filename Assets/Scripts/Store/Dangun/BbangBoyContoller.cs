using UnityEngine;
using UnityEngine.UI;

public class BbangBoyContoller : MonoBehaviour
{
    [Header("Managers and Controllers")] [SerializeField]
    private GameManager gameManager;

    [SerializeField] private BbangShowroomManager bbangShowroomManager;
    [SerializeField] private PrompterController prompterController;

    [Header("UI Elements")] [SerializeField]
    private Animator bbangBoyAnimator;

    [SerializeField] private GameObject prompter;
    [SerializeField] private GameObject bbangboyBalloon;
    [SerializeField] private Text bbangboyBalloon_ui;
    private float oldTime;
    private bool showText;
    private int showTextIdx;

    public bool ShouldStopEating { get; set; }

    private void Update()
    {
        if ((Time.frameCount % 30 == 0) & showText)
            switch (showTextIdx)
            {
                case 0:
                    if (oldTime < Time.time - 1f)
                    {
                        Msg("안녕하세오 당근이에오!");
                        oldTime = Time.time;
                        showTextIdx = 1;
                    }

                    break;
                case 1:
                    if (oldTime < Time.time - 1.5f)
                    {
                        Msg("빵 먹고 갈게오!");
                        oldTime = Time.time;
                        showTextIdx = 2;
                    }

                    break;
                case 2:
                    if (oldTime < Time.time - 1.5f)
                    {
                        Boy_Eat();
                        Msg("냠냠 맛있다!");
                        showTextIdx = 3;
                    }

                    break;
                case 3:
                    if (ShouldStopEating)
                    {
                        Msg("감사함미다!");
                        Boy_Wait();
                        showTextIdx = 4;
                        oldTime = Time.time;
                    }

                    break;
                case 4:
                    if (oldTime < Time.time - 1f)
                    {
                        oldTime = Time.time;
                        showTextIdx = 5;
                        prompterController.Reset();
                        gameManager.AddBBangType(1, "단군_빵먹기", "choco");
                        prompterController.AddString("훈이", "집이 좀 깨끗해진 것 같아서 좋다.");
                        prompterController.AddNextAction("bbangBoy", "bbangBoyOut");
                        prompter.SetActive(true);
                        prompter.GetComponent<Animator>().SetTrigger("show");
                    }

                    break;
                case 5:
                    if (oldTime < Time.time - 1.5f)
                    {
                        Boy_Out();
                        showText = false;
                    }

                    break;
            }
    }

    public void Boy_Wait()
    {
        bbangBoyAnimator.SetTrigger("wait");
    }

    public void Boy_Eat()
    {
        bbangBoyAnimator.SetTrigger("eat");
        bbangShowroomManager.removeBbangMode = true;
        ShouldStopEating = false;
    }

    public void Boy_In()
    {
        prompterController.CallAction("bbangBoy", "bbangBoyIn");
        bbangBoyAnimator.SetTrigger("in");
        bbangBoyAnimator.gameObject.SetActive(true);
        showText = true;
        showTextIdx = 0;
        oldTime = Time.time;
    }

    public void Boy_Out()
    {
        bbangBoyAnimator.SetTrigger("out");
    }

    public void StopEating()
    {
        Boy_Out();
    }

    public void Msg(string output)
    {
        bbangboyBalloon_ui.text = output;
        bbangboyBalloon.SetActive(true);
        bbangboyBalloon.GetComponent<Animator>().SetTrigger("show");
    }

    public void SetBbang80()
    {
        bbangShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_choco, 80);
    }

    public void Debug_mat()
    {
        PlayerPrefs.SetInt("Matdongsan", PlayerPrefs.GetInt("Matdongsan") + 1);
        bbangShowroomManager.AddBbang(BbangShowroomManager.BbangType.bbang_mat, 1);
    }
}