using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BbangBtnControl : MonoBehaviour
{
    [SerializeField] BalloonControl ballon;
    [SerializeField] Bbang_showroom showroom;
    [SerializeField] GameObject Hoonie;
    [SerializeField] Heart_Control heart;
    [SerializeField] Text title;
    [SerializeField] PhoneMsgCtrl Msg;

    const string format = "yyyy/MM/dd";
    System.IFormatProvider provider;

    string eatTime, currentTime;
    string myType = "";
    int bbangEatCount;
    int addHeartCount = 1;
    bool hiding = true;

    Vector2 myTargetPosition;
    GameObject targetObj;

    public void ReadData()
    {
        if (!PlayerPrefs.HasKey("bbangEatTime"))
        {
            PlayerPrefs.SetString("bbangEatTime", System.DateTime.Now.ToString(format));
            PlayerPrefs.SetInt("bbangEatCount", 0);
            PlayerPrefs.SetInt("totalBbangEatCount", 0);
            PlayerPrefs.Save();
        }

        eatTime = PlayerPrefs.GetString("bbangEatTime");
        currentTime = System.DateTime.Now.ToString(format);
        //print("eatTime : " + eatTime);
        //print("currentTime : " + currentTime);


        if (eatTime != currentTime)
        {
            eatTime = currentTime;
            bbangEatCount = 0;

            PlayerPrefs.SetString("bbangEatTime", System.DateTime.Now.ToString(format));
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
        if(myType == "choco")
        {
            title.text = "로켓트의 초코롤빵";
        } else if (myType == "mat")
        {
            title.text = "맛동산 한 상자";
        }
        else if (myType == "strawberry")
        {
            title.text = "푸딩의 딸기크림빵";
        }
        else if (myType == "hot")
        {
            title.text = "파이팅의 핫소스빵";
        }
        else if (myType == "vacance")
        {
            title.text = "피로회복제 바캉스";
        }
        else if (myType == "yogurt")
        {
            title.text = "시원달콤 야쿠르트";
        }
        else if (myType == "bingle")
        {
            title.text = "빙글빙글 요거트빵";
        }
        else if (myType == "maple")
        {
            title.text = "메이풀빵";
        }
        else if (myType == "purin")
        {
            title.text = "푸린글스빵";
        }
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
        {
            addHeartCount = 2;
        } else
        {
            addHeartCount = 1;
        }

        if(myType == "vacance")
        {
            Msg.SetMsg(title.text + "를 마시고\n체력을 모두 회복하시겠습니까? \n\n (꼭 하트 풀충전이\n필요할 때 드세요!)", 2, "eat");
            return;
        }
        if (myType == "yogurt")
        {
            Msg.SetMsg(title.text + "를 마시고\n체력을 3개 회복하시겠습니까? \n\n (꼭 하트 3개 충전이\n필요할 때 드세요!)", 2, "eat");
            return;
        }

        if (bbangEatCount <= 2)
        {
            Msg.SetMsg(title.text + "을 먹고\n체력을 " + addHeartCount + "개 회복하시겠습니까? \n\n" + (bbangEatCount + 1) + "/3개", 2, "eat");
        } else
        {
            Msg.SetMsg("빵과 과자는 물리기 때문에 하루에 3개씩만 먹을 수 있습니다.", 1, "eat");
        }
    }

    public void Eat()
    {
        if (myType == "yogurt")
        {
            showroom.RemoveBbangAt(myType, targetObj);
            heart.SetHeart(1);
            heart.SetHeart(1);
            heart.SetHeart(1);
            int rnd = Random.Range(0, 3);
            switch (rnd)
            {
                case 0:
                    ballon.ShowMsg("힘이난다! 하나 더 먹고싶다!!");
                    break;
                case 1:
                    ballon.ShowMsg("힘이난다! 얼려먹어도 맛있겠다!");
                    break;
                case 2:
                    ballon.ShowMsg("좋아 힘이 난다! 맛있다!");
                    break;
            }
            return;
        }

        if (myType == "vacance")
        {
            showroom.RemoveBbangAt(myType, targetObj);
            heart.SetHeart(1);
            heart.SetHeart(1);
            heart.SetHeart(1);
            heart.SetHeart(1);
            heart.SetHeart(1);
            heart.SetHeart(1);
            int rnd = Random.Range(0, 3);
            switch (rnd)
            {
                case 0:
                    ballon.ShowMsg("우와앗! 힘이 솟아난다!!");
                    break;
                case 1:
                    ballon.ShowMsg("피로가 확 풀린다!");
                    break;
                case 2:
                    ballon.ShowMsg("진짜 피로회복제는 여기있었구나!!");
                    break;
            }
            return;
        }



        //print(bbangEatCount);
        ReadData();
        if (bbangEatCount <= 2)
        {
            showroom.RemoveBbangAt(myType, targetObj);
            if (myType == "mat")
            {
                heart.SetHeart(1);
                heart.SetHeart(1);
            }
            else
            {
                heart.SetHeart(1);
            }
            bbangEatCount += 1;
            PlayerPrefs.SetInt("bbangEatCount", bbangEatCount);
            PlayerPrefs.SetInt("totalBbangEatCount", PlayerPrefs.GetInt("totalBbangEatCount") + 1);
            PlayerPrefs.Save();


            int rnd = Random.Range(0, 8);

            if (myType == "choco")
            {
                switch (rnd)
                {
                    case 0:
                        ballon.ShowMsg("음. 달달한게 아주 맛있다!");
                        break;
                    case 1:
                        ballon.ShowMsg("촉촉한 초코맛. 맛있다!");
                        break;
                    case 2:
                        ballon.ShowMsg("냠냠. 맛있는걸?");
                        break;
                    case 3:
                        ballon.ShowMsg("찐득한 초코맛. 맛잇다!");
                        break;
                    case 4:
                        ballon.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        ballon.ShowMsg("오옷 이 맛은!!!");
                        break;
                    case 6:
                        ballon.ShowMsg("맛있는데 금방 물릴 것 같다.");
                        break;
                    case 7:
                        ballon.ShowMsg("우유랑 먹으면 맛있을 것 같다!");
                        break;
                }
            } else if (myType == "hot")
            {
                switch (rnd)
                {
                    case 0:
                        ballon.ShowMsg("음. 매콤 달달한게 아주 맛있다!");
                        break;
                    case 1:
                        ballon.ShowMsg("핫소스맛이라니. 맛있다!");
                        break;
                    case 2:
                        ballon.ShowMsg("오오. 맛있는걸?");
                        break;
                    case 3:
                        ballon.ShowMsg("신ㄱ한 맛이다. 맛잇다!");
                        break;
                    case 4:
                        ballon.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        ballon.ShowMsg("오옷 이 맛은!!!");
                        break;
                    case 6:
                        ballon.ShowMsg("맛있는데 금방 물릴 것 같다.");
                        break;
                    case 7:
                        ballon.ShowMsg("뭐랑 같이 먹어야 하지?");
                        break;
                }
            }
            else if (myType == "bingle")
            {
                switch (rnd)
                {
                    case 0:
                        ballon.ShowMsg("음. 달달한게 아주 맛있다!");
                        break;
                    case 1:
                        ballon.ShowMsg("요거트맛이라니. 맛있다!");
                        break;
                    case 2:
                        ballon.ShowMsg("오오. 맛있는걸?");
                        break;
                    case 3:
                        ballon.ShowMsg("신비한 맛이다. 빙글빙글맛!");
                        break;
                    case 4:
                        ballon.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        ballon.ShowMsg("오옷 이 맛은!!!");
                        break;
                    case 6:
                        ballon.ShowMsg("맛있는데 금방 물릴 것 같다.");
                        break;
                    case 7:
                        ballon.ShowMsg("뭐랑 같이 먹어야 하지?");
                        break;
                }
            }
            else if (myType == "strawberry")
            {
                switch (rnd)
                {
                    case 0:
                        ballon.ShowMsg("음. 달달한 딸기맛 아주 맛있다!");
                        break;
                    case 1:
                        ballon.ShowMsg("촉촉한 딸기맛. 맛있다!");
                        break;
                    case 2:
                        ballon.ShowMsg("냠냠. 의외로 맛있는걸?");
                        break;
                    case 3:
                        ballon.ShowMsg("딸기쨈맛. 맛있다!");
                        break;
                    case 4:
                        ballon.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        ballon.ShowMsg("오옷 이 맛은!!!");
                        break;
                    case 6:
                        ballon.ShowMsg("맛있는데 금방 물릴 것 같다.");
                        break;
                    case 7:
                        ballon.ShowMsg("우유랑 먹으면 맛있을 것 같다!");
                        break;
                }
            }
            else if (myType == "mat")
            {
                switch (rnd)
                {
                    case 0:
                        ballon.ShowMsg("맛있는데, 양이 너무 많아..");
                        break;
                    case 1:
                        ballon.ShowMsg("먹어도 먹어도 줄지가 않는다!");
                        break;
                    case 2:
                        ballon.ShowMsg("다먹기엔 너무나도 배부르다!");
                        break;
                    case 3:
                        ballon.ShowMsg("딱딱해서 이빨이 아파!");
                        break;
                    case 4:
                        ballon.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        ballon.ShowMsg("할아버지가 좋아할 맛!");
                        break;
                    case 6:
                        ballon.ShowMsg("음.. 금방 물릴 것 같은 맛.");
                        break;
                    case 7:
                        ballon.ShowMsg("우리 할아버지가 좋아하는 과자다!");
                        break;
                }
            }
            else if (myType == "maple")
            {
                switch (rnd)
                {
                    case 0:
                        ballon.ShowMsg("추억의 풀빵이다!");
                        break;
                    case 1:
                        ballon.ShowMsg("풀빵은 이런 맛이구나 음!!");
                        break;
                    case 2:
                        ballon.ShowMsg("팟이 가득 들었다 맛있다!");
                        break;
                    case 3:
                        ballon.ShowMsg("겉바속촉 풀빵이다. 맛있다!");
                        break;
                    case 4:
                        ballon.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        ballon.ShowMsg("할아버지가 좋아할 맛!");
                        break;
                    case 6:
                        ballon.ShowMsg("보기보다 맛있는데?!");
                        break;
                    case 7:
                        ballon.ShowMsg("팥맛 붕어빵 맛이다. 맛있다!");
                        break;
                }
            }
            else if (myType == "purin")
            {
                switch (rnd)
                {
                    case 0:
                        ballon.ShowMsg("오 이것은 피치피치맛!!");
                        break;
                    case 1:
                        ballon.ShowMsg("복숭아맛 감자칩이다!");
                        break;
                    case 2:
                        ballon.ShowMsg("겉바속촉인게 맛있다!");
                        break;
                    case 3:
                        ballon.ShowMsg("푸린글스는 역시 피치맛이지!");
                        break;
                    case 4:
                        ballon.ShowMsg("맛있다! 힘이 조금 나는 것 같아!");
                        break;
                    case 5:
                        ballon.ShowMsg("달달한 복숭아맛!");
                        break;
                    case 6:
                        ballon.ShowMsg("복숭아맛이라니, 맛있어!");
                        break;
                    case 7:
                        ballon.ShowMsg("복숭아맛 슈크림빵!");
                        break;
                }
            }
        }
        else
        {
            int rnd = Random.Range(0, 8);
            switch (rnd)
            {
                case 0:
                    ballon.ShowMsg("지금은 배부르다.");
                    break;
                case 1:
                    ballon.ShowMsg("맛있는데 이제 그만 먹고싶다.");
                    break;
                case 2:
                    ballon.ShowMsg("물린다. \n 오늘은 이제 그만 먹고싶다.");
                    break;
                case 3:
                    ballon.ShowMsg("... 꼭 먹어야만 할까..?");
                    break;
                case 4:
                    ballon.ShowMsg("너무 달다. \n 이제 그만 먹고 싶어졌다.");
                    break;
                case 5:
                    ballon.ShowMsg("내일 더 먹을까? \n (오늘은 물린다)");
                    break;
                case 6:
                    ballon.ShowMsg("너무 달아서 그만 먹고 싶다.");
                    break;
                case 7:
                    ballon.ShowMsg("오늘은 그만 먹고싶다. \n 내일 더 먹어야 겠다.");
                    break;
            }
        }
    }
    public void ThrowAway()
    {
        int rnd = Random.Range(0, 8);
        switch (rnd)
        {
            case 0:
                ballon.ShowMsg("버리기엔 아깝다..");
                break;
            case 1:
                ballon.ShowMsg("아까운데 누구 줄 수 없을까?");
                break;
            case 2:
                ballon.ShowMsg("단군마켓에 팔아볼까..?");
                break;
            case 3:
                ballon.ShowMsg("빵을 먹고싶은 사람도 있을텐데.. \n 버리기엔 아깝다.");
                break;
            case 4:
                ballon.ShowMsg("방을 좀 치우고 싶긴 한데.. \n 이걸 버리기엔 아까워.");
                break;
            case 5:
                ballon.ShowMsg("버리기엔 아깝다. \n 단군마켓에서 필요한 사람을 찾아볼까?");
                break;
            case 6:
                ballon.ShowMsg("남은 빵을 어떻게 처리하는 게 좋을까?");
                break;
            case 7:
                ballon.ShowMsg("먹을거를 소중히 하라고 했어.");
                break;
        }
    }
}
