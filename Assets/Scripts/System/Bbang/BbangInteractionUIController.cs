using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BbangInteractionUIController : MonoBehaviour
{
    private const string format = "yyyy/MM/dd";

    [Header("Managers and Controllers")] [SerializeField]
    private BalloonUIManager balloonUIManager;

    [SerializeField] private BbangShowroomManager showroomManager;
    [SerializeField] private PlayerHealthManager playerHealthManager;
    [SerializeField] private PhoneMessageController phoneMessageController;

    [Header("UI Elements")] [SerializeField]
    private Text title;

    private int bbangEatCount, addHeartCount = 1;
    private string eatTime, currentTime, myType = "";
    private bool hiding = true;

    private Vector2 myTargetPosition;

    private IFormatProvider provider;
    private GameObject targetObj;

    public void ReadData()
    {
        if (!PlayerPrefs.HasKey("bbangEatTime"))
        {
            PlayerPrefs.SetString("bbangEatTime", DateTime.Now.ToString(format));
            PlayerPrefs.SetInt("bbangEatCount", 0);
            PlayerPrefs.SetInt("totalBbangEatCount", 0);
            PlayerPrefs.Save();
        }

        eatTime = PlayerPrefs.GetString("bbangEatTime");
        currentTime = DateTime.Now.ToString(format);

        if (eatTime != currentTime)
        {
            eatTime = currentTime;
            bbangEatCount = 0;

            PlayerPrefs.SetString("bbangEatTime", DateTime.Now.ToString(format));
            PlayerPrefs.SetInt("bbangEatCount", 0);
            PlayerPrefs.Save();
            print("BbangEatDate Past!");
        }

        bbangEatCount = PlayerPrefs.GetInt("bbangEatCount");
    }


    public void ShowBbangBtn(Vector2 myTarget, string type, GameObject target)
    {
        myType = type;
        targetObj = target;
        if (myType == "choco")
            title.text = "로켓트의 초코롤빵";
        else if (myType == "mat")
            title.text = "맛동산 한 상자";
        else if (myType == "strawberry")
            title.text = "푸딩의 딸기크림빵";
        else if (myType == "hot")
            title.text = "파이팅의 핫소스빵";
        else if (myType == "vacance")
            title.text = "피로회복제 바캉스";
        else if (myType == "yogurt")
            title.text = "시원달콤 야쿠르트";
        else if (myType == "bingle")
            title.text = "빙글빙글 요거트빵";
        else if (myType == "maple")
            title.text = "메이풀빵";
        else if (myType == "purin") title.text = "푸린글스빵";

        myTargetPosition = myTarget;
        gameObject.GetComponent<Animator>().SetTrigger("show");
        gameObject.transform.position = myTarget;
        gameObject.SetActive(true);
        hiding = false;
    }

    public void SetMeDeactive()
    {
        gameObject.SetActive(false);
        hiding = true;
    }

    public void Hiding()
    {
        hiding = true;
    }

    public void EatCheck()
    {
        ReadData();

        if (myType == "mat")
            addHeartCount = 2;
        else
            addHeartCount = 1;

        if (myType == "vacance")
        {
            phoneMessageController.SetMsg(title.text + "를 마시고\n체력을 모두 회복하시겠습니까? \n\n (꼭 하트 풀충전이\n필요할 때 드세요!)", 2,
                "eat");
            return;
        }

        if (myType == "yogurt")
        {
            phoneMessageController.SetMsg(title.text + "를 마시고\n체력을 3개 회복하시겠습니까? \n\n (꼭 하트 3개 충전이\n필요할 때 드세요!)", 2,
                "eat");
            return;
        }

        if (bbangEatCount <= 2)
            phoneMessageController.SetMsg(
                title.text + "을 먹고\n체력을 " + addHeartCount + "개 회복하시겠습니까? \n\n" + (bbangEatCount + 1) + "/3개", 2, "eat");
        else
            phoneMessageController.SetMsg("빵과 과자는 물리기 때문에 하루에 3개씩만 먹을 수 있습니다.", 1, "eat");
    }

    public void Eat()
    {
        if (myType == "yogurt")
        {
            showroomManager.RemoveBbangAt(myType, targetObj);
            playerHealthManager.SetHeart(1);
            playerHealthManager.SetHeart(1);
            playerHealthManager.SetHeart(1);
            var rnd = Random.Range(0, 3);
            switch (rnd)
            {
                case 0:
                    balloonUIManager.ShowMsg("힘이난다! 하나 더 먹고싶다!!");
                    break;
                case 1:
                    balloonUIManager.ShowMsg("힘이난다! 얼려먹어도 맛있겠다!");
                    break;
                case 2:
                    balloonUIManager.ShowMsg("좋아 힘이 난다! 맛있다!");
                    break;
            }

            return;
        }

        if (myType == "vacance")
        {
            showroomManager.RemoveBbangAt(myType, targetObj);
            playerHealthManager.SetHeart(1);
            playerHealthManager.SetHeart(1);
            playerHealthManager.SetHeart(1);
            playerHealthManager.SetHeart(1);
            playerHealthManager.SetHeart(1);
            playerHealthManager.SetHeart(1);
            var rnd = Random.Range(0, 3);
            switch (rnd)
            {
                case 0:
                    balloonUIManager.ShowMsg("우와앗! 힘이 솟아난다!!");
                    break;
                case 1:
                    balloonUIManager.ShowMsg("피로가 확 풀린다!");
                    break;
                case 2:
                    balloonUIManager.ShowMsg("진짜 피로회복제는 여기있었구나!!");
                    break;
            }

            return;
        }

        ReadData();
        if (bbangEatCount <= 2)
        {
            showroomManager.RemoveBbangAt(myType, targetObj);
            if (myType == "mat")
            {
                playerHealthManager.SetHeart(1);
                playerHealthManager.SetHeart(1);
            }
            else
            {
                playerHealthManager.SetHeart(1);
            }

            bbangEatCount += 1;
            PlayerPrefs.SetInt("bbangEatCount", bbangEatCount);
            PlayerPrefs.SetInt("totalBbangEatCount", PlayerPrefs.GetInt("totalBbangEatCount") + 1);
            PlayerPrefs.Save();


            var rnd = Random.Range(0, 8);

            if (myType == "choco")
                switch (rnd)
                {
                    case 0:
                        balloonUIManager.ShowMsg("음. 달달한게 아주 맛있다!");
                        break;
                    case 1:
                        balloonUIManager.ShowMsg("촉촉한 초코맛. 맛있다!");
                        break;
                    case 2:
                        balloonUIManager.ShowMsg("냠냠. 맛있는걸?");
                        break;
                    case 3:
                        balloonUIManager.ShowMsg("찐득한 초코맛. 맛잇다!");
                        break;
                    case 4:
                        balloonUIManager.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        balloonUIManager.ShowMsg("오옷 이 맛은!!!");
                        break;
                    case 6:
                        balloonUIManager.ShowMsg("맛있는데 금방 물릴 것 같다.");
                        break;
                    case 7:
                        balloonUIManager.ShowMsg("우유랑 먹으면 맛있을 것 같다!");
                        break;
                }
            else if (myType == "hot")
                switch (rnd)
                {
                    case 0:
                        balloonUIManager.ShowMsg("음. 매콤 달달한게 아주 맛있다!");
                        break;
                    case 1:
                        balloonUIManager.ShowMsg("핫소스맛이라니. 맛있다!");
                        break;
                    case 2:
                        balloonUIManager.ShowMsg("오오. 맛있는걸?");
                        break;
                    case 3:
                        balloonUIManager.ShowMsg("신ㄱ한 맛이다. 맛잇다!");
                        break;
                    case 4:
                        balloonUIManager.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        balloonUIManager.ShowMsg("오옷 이 맛은!!!");
                        break;
                    case 6:
                        balloonUIManager.ShowMsg("맛있는데 금방 물릴 것 같다.");
                        break;
                    case 7:
                        balloonUIManager.ShowMsg("뭐랑 같이 먹어야 하지?");
                        break;
                }
            else if (myType == "bingle")
                switch (rnd)
                {
                    case 0:
                        balloonUIManager.ShowMsg("음. 달달한게 아주 맛있다!");
                        break;
                    case 1:
                        balloonUIManager.ShowMsg("요거트맛이라니. 맛있다!");
                        break;
                    case 2:
                        balloonUIManager.ShowMsg("오오. 맛있는걸?");
                        break;
                    case 3:
                        balloonUIManager.ShowMsg("신비한 맛이다. 빙글빙글맛!");
                        break;
                    case 4:
                        balloonUIManager.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        balloonUIManager.ShowMsg("오옷 이 맛은!!!");
                        break;
                    case 6:
                        balloonUIManager.ShowMsg("맛있는데 금방 물릴 것 같다.");
                        break;
                    case 7:
                        balloonUIManager.ShowMsg("뭐랑 같이 먹어야 하지?");
                        break;
                }
            else if (myType == "strawberry")
                switch (rnd)
                {
                    case 0:
                        balloonUIManager.ShowMsg("음. 달달한 딸기맛 아주 맛있다!");
                        break;
                    case 1:
                        balloonUIManager.ShowMsg("촉촉한 딸기맛. 맛있다!");
                        break;
                    case 2:
                        balloonUIManager.ShowMsg("냠냠. 의외로 맛있는걸?");
                        break;
                    case 3:
                        balloonUIManager.ShowMsg("딸기쨈맛. 맛있다!");
                        break;
                    case 4:
                        balloonUIManager.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        balloonUIManager.ShowMsg("오옷 이 맛은!!!");
                        break;
                    case 6:
                        balloonUIManager.ShowMsg("맛있는데 금방 물릴 것 같다.");
                        break;
                    case 7:
                        balloonUIManager.ShowMsg("우유랑 먹으면 맛있을 것 같다!");
                        break;
                }
            else if (myType == "mat")
                switch (rnd)
                {
                    case 0:
                        balloonUIManager.ShowMsg("맛있는데, 양이 너무 많아..");
                        break;
                    case 1:
                        balloonUIManager.ShowMsg("먹어도 먹어도 줄지가 않는다!");
                        break;
                    case 2:
                        balloonUIManager.ShowMsg("다먹기엔 너무나도 배부르다!");
                        break;
                    case 3:
                        balloonUIManager.ShowMsg("딱딱해서 이빨이 아파!");
                        break;
                    case 4:
                        balloonUIManager.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        balloonUIManager.ShowMsg("할아버지가 좋아할 맛!");
                        break;
                    case 6:
                        balloonUIManager.ShowMsg("음.. 금방 물릴 것 같은 맛.");
                        break;
                    case 7:
                        balloonUIManager.ShowMsg("우리 할아버지가 좋아하는 과자다!");
                        break;
                }
            else if (myType == "maple")
                switch (rnd)
                {
                    case 0:
                        balloonUIManager.ShowMsg("추억의 풀빵이다!");
                        break;
                    case 1:
                        balloonUIManager.ShowMsg("풀빵은 이런 맛이구나 음!!");
                        break;
                    case 2:
                        balloonUIManager.ShowMsg("팟이 가득 들었다 맛있다!");
                        break;
                    case 3:
                        balloonUIManager.ShowMsg("겉바속촉 풀빵이다. 맛있다!");
                        break;
                    case 4:
                        balloonUIManager.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        balloonUIManager.ShowMsg("할아버지가 좋아할 맛!");
                        break;
                    case 6:
                        balloonUIManager.ShowMsg("보기보다 맛있는데?!");
                        break;
                    case 7:
                        balloonUIManager.ShowMsg("팥맛 붕어빵 맛이다. 맛있다!");
                        break;
                }
            else if (myType == "purin")
                switch (rnd)
                {
                    case 0:
                        balloonUIManager.ShowMsg("오 이것은 피치피치맛!!");
                        break;
                    case 1:
                        balloonUIManager.ShowMsg("복숭아맛 감자칩이다!");
                        break;
                    case 2:
                        balloonUIManager.ShowMsg("겉바속촉인게 맛있다!");
                        break;
                    case 3:
                        balloonUIManager.ShowMsg("푸린글스는 역시 피치맛이지!");
                        break;
                    case 4:
                        balloonUIManager.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        balloonUIManager.ShowMsg("달달한 복숭아맛!");
                        break;
                    case 6:
                        balloonUIManager.ShowMsg("복숭아맛이라니, 맛있어!");
                        break;
                    case 7:
                        balloonUIManager.ShowMsg("복숭아맛 슈크림빵!");
                        break;
                }
        }
        else
        {
            var rnd = Random.Range(0, 8);
            switch (rnd)
            {
                case 0:
                    balloonUIManager.ShowMsg("지금은 배부르다.");
                    break;
                case 1:
                    balloonUIManager.ShowMsg("맛있는데 이제 그만 먹고싶다.");
                    break;
                case 2:
                    balloonUIManager.ShowMsg("물린다. \n 오늘은 이제 그만 먹고싶다.");
                    break;
                case 3:
                    balloonUIManager.ShowMsg("... 꼭 먹어야만 할까..?");
                    break;
                case 4:
                    balloonUIManager.ShowMsg("너무 달다. \n 이제 그만 먹고 싶어졌다.");
                    break;
                case 5:
                    balloonUIManager.ShowMsg("내일 더 먹을까? \n (오늘은 물린다)");
                    break;
                case 6:
                    balloonUIManager.ShowMsg("너무 달아서 그만 먹고 싶다.");
                    break;
                case 7:
                    balloonUIManager.ShowMsg("오늘은 그만 먹고싶다. \n 내일 더 먹어야 겠다.");
                    break;
            }
        }
    }

    public void ThrowAway()
    {
        var rnd = Random.Range(0, 8);
        switch (rnd)
        {
            case 0:
                balloonUIManager.ShowMsg("버리기엔 아깝다..");
                break;
            case 1:
                balloonUIManager.ShowMsg("아까운데 누구 줄 수 없을까?");
                break;
            case 2:
                balloonUIManager.ShowMsg("단군마켓에 팔아볼까..?");
                break;
            case 3:
                balloonUIManager.ShowMsg("빵을 먹고싶은 사람도 있을텐데.. \n 버리기엔 아깝다.");
                break;
            case 4:
                balloonUIManager.ShowMsg("방을 좀 치우고 싶긴 한데.. \n 이걸 버리기엔 아까워.");
                break;
            case 5:
                balloonUIManager.ShowMsg("버리기엔 아깝다. \n 단군마켓에서 필요한 사람을 찾아볼까?");
                break;
            case 6:
                balloonUIManager.ShowMsg("남은 빵을 어떻게 처리하는 게 좋을까?");
                break;
            case 7:
                balloonUIManager.ShowMsg("먹을거를 소중히 하라고 했어.");
                break;
        }
    }
}