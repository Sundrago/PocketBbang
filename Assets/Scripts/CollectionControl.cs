using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Cards
{
    public Sprite image;
    public string name;
    public string description;
    public string reaction;
    public int count;
    public int idx;
    public char rank;
}

public class CollectionControl : MonoBehaviour
{
    [SerializeField] Sprite[] imgs = new Sprite[0];
    [SerializeField] Sprite[] imgsHighres = new Sprite[0];
    [SerializeField] AchievementCtrl achievement;
    [SerializeField] AudioControl myAydio;
    [SerializeField] GameObject CardPanel, card_image, card, fx1, fx2, title_ui, bbang, canvas, card_bg, text_board, pormpter_ui, pormpter_name_ui, trans_btn, new_ui;

    bool started = false;
    bool hidden = false;
    bool isnew = false;

    public List<Cards> myCard = new List<Cards>();
    public bool delay;

    public static CollectionControl Instance;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    private List<int> S_idx;
    private List<int> A_idx;
    private List<int> B_idx;

    // Start is called before the first frame update
    public void Start()
    {
        if (started) return;
        started = true;

        S_idx = new List<int>();
        A_idx = new List<int>();
        B_idx = new List<int>();

        Cards newCard = new Cards();
        newCard.image = imgs[0];
        newCard.name = "김상해씨";
        newCard.description = "양반 김씨 8대손 김상해씨다.";
        newCard.idx = 1;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[1];
        newCard.name = "피카소-Chu";
        newCard.description = "그림을 잘 그리는 노란 고양이 Chu.";
        newCard.idx = 25;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[2];
        newCard.name = "별가사리";
        newCard.description = "어디서 많이 본 것 같은 불가사리.";
        newCard.idx = 120;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[3];
        newCard.name = "파이-팅!";
        newCard.description = "따뜻한 파란색 불꽃의 따봉! 파이팅!";
        newCard.idx = 4;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[4];
        newCard.name = "꼽주기";
        newCard.description = "왠지 거북이가 나를 놀리는 것 같다.";
        newCard.idx = 7;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[5];
        newCard.name = "도도";
        newCard.description = "도도해 보이는 새. 머리가 두개라 더 도도해 보인다.";
        newCard.idx = 84;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[6];
        newCard.name = "피식";
        newCard.description = "나를 보고 웃는것 같아 기분이 좋지는 않지만 귀엽다.";
        newCard.idx = 36;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[7];
        newCard.name = "코(난도)일";
        newCard.description = "코난과 자석이 무슨 상관인지는 잘 모르겠다.";
        newCard.idx = 81;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[8];
        newCard.name = "질처기";
        newCard.description = "질처억-. 그만 좀 질척댔으면 좋겠다.";
        newCard.idx = 89;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[9];
        newCard.name = "자몽Lee";
        newCard.description = "이씨 집안 대대로 농사짓는 자몽이다. 신선함이 일품.";
        newCard.idx = 6;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[10];
        newCard.name = "k피죤";
        newCard.description = "요염하게 걷는 한국 둘기";
        newCard.idx = 17;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[11];
        newCard.name = "다그다";
        newCard.description = "다그닥 말이 땅에 파묻혔다.";
        newCard.idx = 50;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[12];
        newCard.name = "캐터피자";
        newCard.description = "브로콜리 맛 피자. 맛은 없을것 같다.";
        newCard.idx = 10;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[13];
        newCard.name = "고데기";
        newCard.description = "초록색 단단한 고데기. 튼튼해서 안 망가진다고 한다.";
        newCard.idx = 11;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[14];
        newCard.name = "버터풀러스";
        newCard.description = "국내산 1등급 A등급 나비로 만든 버터 플러플러스!";
        newCard.idx = 12;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[15];
        newCard.name = "구구구구";
        newCard.description = "비둘기야 먹자. 구구구구. 마이쪙.";
        newCard.idx = 16;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[16];
        newCard.name = "삐빅";
        newCard.description = "삐빅- 어린이입니다~! 삐빅- 잔액이 부족합니다~!";
        newCard.idx = 35;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[17];
        newCard.name = "냐옹";
        newCard.description = "귀여운 고양이다. 냐옹이냐옹~";
        newCard.idx = 52;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[18];
        newCard.name = "김상해씨(B급)";
        newCard.description = "양반 김씨 8대손 김상해씨다.";
        newCard.idx = 1;
        newCard.rank = 'B';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[19];
        newCard.name = "피카소-Chu(B급)";
        newCard.description = "그림을 잘 그리는 노란 고양이 Chu.";
        newCard.idx = 25;
        newCard.rank = 'B';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[20];
        newCard.name = "별가사리(B급)";
        newCard.description = "어디서 많이 본 것 같은 불가사리.";
        newCard.idx = 120;
        newCard.rank = 'B';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[21];
        newCard.name = "파이-팅!(B급)";
        newCard.description = "따뜻한 파란색 불꽃의 따봉! 파이팅!";
        newCard.idx = 4;
        newCard.rank = 'B';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[22];
        newCard.name = "꼽주기(B급)";
        newCard.description = "왠지 거북이가 나를 놀리는 것 같다.";
        newCard.idx = 7;
        newCard.rank = 'B';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[23];
        newCard.name = "도도(B급)";
        newCard.description = "도도해 보이는 새. 머리가 두개라 더 도도해 보인다.";
        newCard.idx = 84;
        newCard.rank = 'B';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[24];
        newCard.name = "피식(B급)";
        newCard.description = "나를 보고 웃는것 같아 기분이 좋지는 않지만 귀엽다.";
        newCard.idx = 36;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[25];
        newCard.name = "코(난도)일(B급)";
        newCard.description = "코난과 자석이 무슨 상관인지는 잘 모르겠다.";
        newCard.idx = 81;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[26];
        newCard.name = "질처기(B급)";
        newCard.description = "질처억-. 그만 좀 질척댔으면 좋겠다.";
        newCard.idx = 89;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[27];
        newCard.name = "자몽Lee(B급)";
        newCard.description = "이씨 집안 대대로 농사짓는 자몽이다. 신선함이 일품.";
        newCard.idx = 6;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[28];
        newCard.name = "k피죤(B급)";
        newCard.description = "요염하게 걷는 한국 둘기";
        newCard.idx = 17;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[29];
        newCard.name = "다그다(B급)";
        newCard.description = "다그닥 말이 땅에 파묻혔다.";
        newCard.idx = 50;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[30];
        newCard.name = "캐터피자(B급)";
        newCard.description = "브로콜리 맛 피자. 맛은 없을것 같다.";
        newCard.idx = 10;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[31];
        newCard.name = "고데기(B급)";
        newCard.description = "초록색 단단한 고데기. 튼튼해서 안 망가진다고 한다.";
        newCard.idx = 11;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[32];
        newCard.name = "버터풀러스(B급)";
        newCard.description = "국내산 1등급 A등급 나비로 만든 버터 플러플러스!";
        newCard.idx = 12;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[33];
        newCard.name = "구구구구(B급)";
        newCard.description = "비둘기야 먹자. 구구구구. 마이쪙.";
        newCard.idx = 16;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[34];
        newCard.name = "삐빅(B급)";
        newCard.description = "삐빅- 어린이입니다~! 삐빅- 잔액이 부족합니다~!";
        newCard.idx = 35;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[35];
        newCard.name = "냐옹(B급)";
        newCard.description = "귀여운 고양이다. 냐옹이냐옹~";
        newCard.idx = 52;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[36];
        newCard.name = "똔까스";
        newCard.description = "보라색 돈까스와 까스- 까스-!";
        newCard.idx = 110;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[37];
        newCard.name = "가오-나시";
        newCard.description = "멋쟁이 헤어스타일을 한 가오나시.";
        newCard.idx = 103;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[38];
        newCard.name = "푸린글스";
        newCard.description = "초록색 어니언 맛이 진리! 뭘 좀 아는 걸!";
        newCard.idx = 39;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[39];
        newCard.name = "얼라리?";
        newCard.description = "귀여운 알알 친구들 다섯마리. 얼라리~?";
        newCard.idx = 102;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[40];
        newCard.name = "카레-라이츄";
        newCard.description = "따끈한 카레라이스 오믈렛. 채소로 만든 귀여운 얼굴!";
        newCard.idx = 26;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[41];
        newCard.name = "고라파도";
        newCard.description = "러버덕 친구야 뒤에 파도를 조심해!";
        newCard.idx = 54;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[42];
        newCard.name = "푸크린토피아";
        newCard.description = "푸근한 인상의 세탁소 아줌마. 아주머니 운동화 빨러 왔어요!";
        newCard.idx = 40;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[43];
        newCard.name = "암나이트클럽";
        newCard.description = "아무나 와서 춤출 수 있는 클럽. 심지어 오징어도 둠칫둠칫~!";
        newCard.idx = 138;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[44];
        newCard.name = "꼬막돌";
        newCard.description = "꼬막 무침을 먹는데 무시무시한 돌을 씹었다. 아얏-!";
        newCard.idx = 74;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[45];
        newCard.name = "똔까스(B급)";
        newCard.description = "보라색 돈까스와 까스- 까스-!";
        newCard.idx = 110;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[46];
        newCard.name = "가오-나시(B급)";
        newCard.description = "멋쟁이 헤어스타일을 한 가오나시.";
        newCard.idx = 103;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[47];
        newCard.name = "푸린글스(B급)";
        newCard.description = "초록색 어니언 맛이 진리! 뭘 좀 아는 걸!";
        newCard.idx = 39;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[48];
        newCard.name = "얼라리?(B급)";
        newCard.description = "귀여운 알알 친구들 다섯마리. 얼라리~?";
        newCard.idx = 102;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[49];
        newCard.name = "카레-라이츄(B급)";
        newCard.description = "따끈한 카레라이스 오믈렛. 채소로 만든 귀여운 얼굴!";
        newCard.idx = 26;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[50];
        newCard.name = "고라파도(B급)";
        newCard.description = "러버덕 친구야 커다란 파도를 조심해!";
        newCard.idx = 54;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[51];
        newCard.name = "푸크린토피아(B급)";
        newCard.description = "푸근한 인상의 세탁소 아줌마. 아주머니 운동화 빨러 왔어요!";
        newCard.idx = 40;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[52];
        newCard.name = "암나이트클럽(B급)";
        newCard.description = "아무나 와서 춤출 수 있는 클럽. 심지어 오징어도 둠칫둠칫~!";
        newCard.idx = 138;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[53];
        newCard.name = "꼬막돌(B급)";
        newCard.description = "꼬막 무침을 먹는데 무시무시한 돌을 씹었다. 아얏-!";
        newCard.idx = 74;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[54];
        newCard.name = "감자탕구리";
        newCard.description = "구리야 지켜주지 못해서 미안해ㅠㅠ \n 근데 좀 맛있어 보인다..(츄릅)";
        newCard.idx = 104;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[55];
        newCard.name = "꼬릿";
        newCard.description = "으윽-! 꼬릿꼬릿한 냄새-! \n 발을 열심히 깨끗하게 닦자!!";
        newCard.idx = 19;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[56];
        newCard.name = "어머니거북이";
        newCard.description = "엄마 거북이 뚜루루뚜루- \n 아기 거북이 뚜루루뚜루-";
        newCard.idx = 8;
        newCard.rank = 'A';
        myCard.Add(newCard); 
        

        newCard = new Cards();
        newCard.image = imgs[57];
        newCard.name = "거북목왕";
        newCard.description = "이정도 거북목이면 그냥 \n바다에 들어가는 게 빠를 것 같다.";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[58];
        newCard.name = "감자탕구리(B급)";
        newCard.description = "구리야 지켜주지 못해서 미안해ㅠㅠ \n 근데 좀 맛있어 보인다..(츄릅)";
        newCard.idx = 104;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[59];
        newCard.name = "꼬릿(B급)";
        newCard.description = "으윽-! 꼬릿꼬릿한 냄새-! \n 발을 열심히 깨끗하게 닦자!!";
        newCard.idx = 19;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[60];
        newCard.name = "어머니거북이(B급)";
        newCard.description = "엄마 거북이 뚜루루뚜루- \n 아기 거북이 뚜루루뚜루-";
        newCard.idx = 8;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[61];
        newCard.name = "거북목왕(B급)";
        newCard.description = "이정도 거북목이면 그냥 \n바다에 들어가는 게 빠를 것 같다.";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[62];
        newCard.name = "묘";
        newCard.description = "레어카드를 묘를 획득했다! \n 끼야 너무 신난다 -!";
        newCard.idx = 9;
        newCard.rank = 'S';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[63];
        newCard.name = "캐터피";
        newCard.description = "레어카드를 캐터피를 획득했다! \n 세상에 너무 귀엽다 -!";
        newCard.idx = 9;
        newCard.rank = 'S';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[64];
        newCard.name = "탕구리";
        newCard.description = "레어카드 탕구리를 획득했다! \n 이럴수가 너무 귀엽잖아 -!";
        newCard.idx = 9;
        newCard.rank = 'S';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[65];
        newCard.name = "피카추돈까스";
        newCard.description = "레어카드 피카추돈까스를 획득했다! \n 너무 맛잇어보인다 -!";
        newCard.idx = 9;
        newCard.rank = 'S';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[66];
        newCard.name = "쏘드라이브";
        newCard.description = "나랑 같이 쏘카로 드라이브할래? \n 어서 올라타!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[67];
        newCard.name = "아보크도";
        newCard.description = "보아뱀이 아보카도를 \n 씨앗까지 꿀-꺽했대요!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[68];
        newCard.name = "틀딱충이";
        newCard.description = "잔소리는 기분 나쁘지만 \n 조언은 더 기분 나빠요.";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[69];
        newCard.name = "롹키";
        newCard.description = "다같이 롹! 앤! 롤! \n 우리 한번 신나게 놀아보자!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[70];
        newCard.name = "슬리퍼";
        newCard.description = "짜장면은 삼선 짜장면 \n 슬리퍼는 삼선 슬리퍼!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[71];
        newCard.name = "쏘드라이브(B급)";
        newCard.description = "나랑 같이 쏘카로 드라이브할래? \n 어서 올라타!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[72];
        newCard.name = "아보크도(B급)";
        newCard.description = "보아뱀이 아보카도를 \n 씨앗까지 꿀-꺽했대요!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[73];
        newCard.name = "틀딱충이(B급)";
        newCard.description = "잔소리는 기분 나쁘지만 \n 조언은 더 기분 나빠요.";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[74];
        newCard.name = "롹키(B급)";
        newCard.description = "다같이 롹! 앤! 롤! \n 우리 한번 신나게 놀아보자!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[75];
        newCard.name = "슬리퍼(B급)";
        newCard.description = "짜장면은 삼선 짜장면 \n 슬리퍼는 삼선 슬리퍼!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        


        newCard = new Cards();
        newCard.image = imgs[76];
        newCard.name = "근손실몬";
        newCard.description = "통장에서 돈 빠지는게 낫지 \n 근손실 보다는.. 영차..!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[77];
        newCard.name = "잠나무늘보";
        newCard.description = "저 잠보 아니예오. \n 하루에 18시간 밖에 안자오.";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[78];
        newCard.name = "파오리꼬치구이";
        newCard.description = "지켜주지 못해서 미안해 ㅜㅜ \n 그런데 꼬치구이.. 참 맛있겠다..";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[79];
        newCard.name = "GOD뇽";
        newCard.description = "하나, 둘, 셋! 안녕하세요~ \n god뇽 입니다!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[80];
        newCard.name = "근손실몬(B급)";
        newCard.description = "통장에서 돈 빠지는게 낫지 \n 근손실 보다는.. 영차..!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[81];
        newCard.name = "잠나무늘보(B급)";
        newCard.description = "저 잠보 아니예오. \n 하루에 18시간 밖에 안자오.";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[82];
        newCard.name = "파오리꼬치구이(B급)";
        newCard.description = "지켜주지 못해서 미안해 ㅜㅜ \n 그런데 꼬치구이.. 참 맛있겠다..";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[83];
        newCard.name = "GOD뇽(B급)";
        newCard.description = "하나, 둘, 셋! 안녕하세요~ \n god뇽 입니다!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        //0610
        newCard = new Cards();
        newCard.image = imgs[84];
        newCard.name = "야도랏맨";
        newCard.description = "빙글빙글 돌아가는 야도랏의 하루~\n 정신차려 이친구야!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[85];
        newCard.name = "아뵤";
        newCard.description = "태권도 노란띠를 받았어요!\n아뵤-! 돌려차기-!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[86];
        newCard.name = "가츠동";
        newCard.description = "이상하게 생긴 그릇이다!\n입 같기도 하고..?!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[87];
        newCard.name = "망나니뇽";
        newCard.description = "망나니처럼 먹고 놀기만 하면 어떡해!\n조금 부럽잖아!?";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[88];
        newCard.name = "아이고-오스";
        newCard.description = "아이고오 허리야..!\n어르신 허리 조심하세요!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[89];
        newCard.name = "식스팩테일";
        newCard.description = "복근운동을 열심히하자! \n꼬리가 불타오르도록!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        //0616
        newCard = new Cards();
        newCard.image = imgs[90];
        newCard.name = "냄새꼬릿";
        newCard.description = "뭐야 이 꼬릿꼬릿한 냄새는!\n샴푸를 잘 하라고 했지!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[91];
        newCard.name = "셀럽";
        newCard.description = "우와 요즘 핫한 셀럽이잖아!\n저랑 사진 찍어주세요!!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[92];
        newCard.name = "알바몬";
        newCard.description = "띵동-! 배달왔이요-!\n오토바이 안전히 운전하세요-!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[93];
        newCard.name = "이브이로그";
        newCard.description = "구독자 여러분 이브이하~\n오늘은 케이크 맛집을 투어할거예요!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[94];
        newCard.name = "독파리지앵";
        newCard.description = "어때요 해파리 원피스 이쁘죠!\n파리에 왔으면 이정도는 입어줘야죠!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[95];
        newCard.name = "닭트리오";
        newCard.description = "사이좋은 닭친구 삼형제!\n이름은 꼬- 끼- 오- 래요!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[96];
        newCard.name = "갸루도스";
        newCard.description = "요즘 유행하는 갸루피스-!\n인싸라면 갸루도스처럼 포즈해봐요!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[97];
        newCard.name = "잉어:킹받네";
        newCard.description = "슈크림이 좋아요 팥이 좋아요?\n어머! 잉어가 킹받았어요!";
        newCard.idx = 9;
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        //0616 -B
        newCard = new Cards();
        newCard.image = imgs[98];
        newCard.name = "야도랏맨(B급)";
        newCard.description = "빙글빙글 돌아가는 야도랏의 하루~\n 정신차려 이친구야!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[99];
        newCard.name = "아뵤(B급)";
        newCard.description = "태권도 노란띠를 받았어요!\n아뵤-! 돌려차기-!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[100];
        newCard.name = "가츠동(B급)";
        newCard.description = "이상하게 생긴 그릇이다!\n입 같기도 하고..?!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[101];
        newCard.name = "망나니뇽(B급)";
        newCard.description = "망나니처럼 먹고 놀기만 하면 어떡해!\n조금 부럽잖아!?";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[102];
        newCard.name = "아이고-오스(B급)";
        newCard.description = "아이고오 허리야..!\n어르신 허리 조심하세요!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[103];
        newCard.name = "식스팩테일(B급)";
        newCard.description = "복근운동을 열심히하자! \n꼬리가 불타오르도록!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[104];
        newCard.name = "냄새꼬릿(B급)";
        newCard.description = "뭐야 이 꼬릿꼬릿한 냄새는!\n샴푸를 잘 하라고 했지!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[105];
        newCard.name = "셀럽(B급)";
        newCard.description = "우와 요즘 핫한 셀럽이잖아!\n저랑 사진 찍어주세요!!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[106];
        newCard.name = "알바몬(B급)";
        newCard.description = "띵동-! 배달왔이요-!\n오토바이 안전히 운전하세요-!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[107];
        newCard.name = "이브이로그(B급)";
        newCard.description = "구독자 여러분 이브이하~\n오늘은 케이크 맛집을 투어할거예요!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[108];
        newCard.name = "독파리지앵(B급)";
        newCard.description = "어때요 해파리 원피스 이쁘죠!\n파리에 왔으면 이정도는 입어줘야죠!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[109];
        newCard.name = "닭트리오(B급)";
        newCard.description = "사이좋은 닭친구 삼형제!\n이름은 꼬- 끼- 오- 래요!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[110];
        newCard.name = "갸루도스(B급)";
        newCard.description = "요즘 유행하는 갸루피스-!\n인싸라면 갸루도스처럼 포즈해봐요!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[111];
        newCard.name = "잉어:킹받네(B급)";
        newCard.description = "슈크림이 좋아요 팥이 좋아요?\n어머! 잉어가 킹받았어요!";
        newCard.idx = 9;
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        //0617
        newCard = new Cards();
        newCard.image = imgs[112];
        newCard.name = "모래두지";
        newCard.description = "레어카드 모래두지를 획득했다!\n어머! 실물이 더 귀엽잖아!";
        newCard.idx = 9;
        newCard.rank = 'S';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[113];
        newCard.name = "단데기";
        newCard.description = "레어카드 단데기를 획득했다!\n시골에 가면 많이 보인다던데\n너무 신기하잖아!";
        newCard.idx = 9;
        newCard.rank = 'S';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[114];
        newCard.name = "우츠동";
        newCard.description = "레어카드 우츠동을 획득했다!\n너어 진짜 입이 크구나!";
        newCard.idx = 9;
        newCard.rank = 'S';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[115];
        newCard.name = "콘치";
        newCard.description = "레어카드 콘치를 획득했다!\n우와 뻐끔뻐끔 너무 귀여워!!";
        newCard.idx = 9;
        newCard.rank = 'S';
        myCard.Add(newCard);
        

        //0621
        newCard = new Cards();
        newCard.image = imgs[116];
        newCard.name = "탕수륙챙이";
        newCard.description = "홀에 탕수육 하나 나왔어요!\n요리사 올챙이 본 적 있어요?";
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[117];
        newCard.name = "고우스톱";
        newCard.description = "원고-! 투고-! 쓰리고-!\n도박문제 상담전화는 1336번이고-!";
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[118];
        newCard.name = "왕꼬깔콘치";
        newCard.description = "왕꼬깔콘을 머리에 쓴 금붕어!\n무슨 맛일까 궁금해!";
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[119];
        newCard.name = "시라소니몬";
        newCard.description = "바람처럼 스쳐가는-!\n나는 야인이 될거야-!";
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[120];
        newCard.name = "부스터샷";
        newCard.description = "백신주사 아파서 싫은데\n대체 몇 번 더 맞아야 해요..?";
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[121];
        newCard.name = "탕수륙챙이(B급)";
        newCard.description = "홀에 탕수육 하나 나왔어요!\n요리사 올챙이 본 적 있어요?";
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[122];
        newCard.name = "고우스톱(B급)";
        newCard.description = "원고-! 투고-! 쓰리고-!\n도박문제 상담전화는 1336번이고-!";
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[123];
        newCard.name = "왕꼬깔콘치(B급)";
        newCard.description = "왕꼬깔콘을 머리에 쓴 금붕어!\n무슨 맛일까 궁금해!";
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[124];
        newCard.name = "시라소니몬(B급)";
        newCard.description = "바람처럼 스쳐가는-!\n나는 야인이 될거야-!";
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[125];
        newCard.name = "부스터샷(B급)";
        newCard.description = "백신주사 아파서 싫은데\n대체 몇 번 더 맞아야 해요..?";
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        //0626
        newCard = new Cards();
        newCard.image = imgs[126];
        newCard.name = "시크릿쥬쥬";
        newCard.description = "나, 귀여워?\n치링치링 치리링-!";
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[127];
        newCard.name = "당기뇽";
        newCard.description = "미뇽? 당기뇽?\n너무 헷갈려요!";
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[128];
        newCard.name = "쥬레곤볼";
        newCard.description = "간다! 초사이언 바다물범-!\n배치기를 받아라-!";
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[129];
        newCard.name = "그랩";
        newCard.description = "오케이~ 그랩~!\n아주 좋은 생각이라구~!";
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[130];
        newCard.name = "야세뱃돈";
        newCard.description = "하핫 괜찮은데 이것 참!\n주시면 또 받아야죠!";
        newCard.rank = 'A';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[131];
        newCard.name = "시크릿쥬쥬(B급)";
        newCard.description = "나, 귀여워?\n치링치링 치리링-!";
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[132];
        newCard.name = "당기뇽(B급)";
        newCard.description = "미뇽? 당기뇽?\n너무 헷갈려요!";
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[133];
        newCard.name = "쥬레곤볼(B급)";
        newCard.description = "간다! 초사이언 바다물범-!\n배치기를 받아라-!";
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[134];
        newCard.name = "그랩(B급)";
        newCard.description = "오케이~ 그랩~!\n아주 좋은 생각이라구~!";
        newCard.rank = 'B';
        myCard.Add(newCard);
        

        newCard = new Cards();
        newCard.image = imgs[135];
        newCard.name = "야세뱃돈(B급)";
        newCard.description = "하핫 괜찮은데 이것 참!\n주시면 또 받아야죠!";
        newCard.rank = 'B';
        myCard.Add(newCard);

        //0703
        newCard = new Cards();
        newCard.image = imgs[136];
        newCard.name = "간디";
        newCard.description = "순순히 곶감을 넘기면\n유혈사태는 일어나지 않을 것입니다!";
        newCard.rank = 'A';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[137];
        newCard.name = "부추";
        newCard.description = "건강에 좋은 부추!\n국밥에 부추가 빠지면 섭섭하지!";
        newCard.rank = 'A';
        myCard.Add(newCard);


        newCard = new Cards();
        newCard.image = imgs[138];
        newCard.name = "후시딘";
        newCard.description = "상처엔 후~\n가계부채표 후시딘";
        newCard.rank = 'A';
        myCard.Add(newCard);


        newCard = new Cards();
        newCard.image = imgs[139];
        newCard.name = "폴리스";
        newCard.description = "딱딱한 경찰 로봇 폴리스.\n나쁜 녀석들 꼼짝마!";
        newCard.rank = 'A';
        myCard.Add(newCard);


        newCard = new Cards();
        newCard.image = imgs[140];
        newCard.name = "파라솔";
        newCard.description = "등에서 자라는 파라솔 두개!\n여름에 시원하겠다!";
        newCard.rank = 'A';
        myCard.Add(newCard);


        newCard = new Cards();
        newCard.image = imgs[141];
        newCard.name = "간디(B급)";
        newCard.description = "순순히 곶감을 넘기면\n유혈사태는 일어나지 않을 것입니다!";
        newCard.rank = 'B';
        myCard.Add(newCard); 


        newCard = new Cards();
        newCard.image = imgs[142];
        newCard.name = "부추(B급)";
        newCard.description = "건강에 좋은 부추!\n국밥에 부추가 빠지면 섭섭하지!";
        newCard.rank = 'B';
        myCard.Add(newCard);


        newCard = new Cards();
        newCard.image = imgs[143];
        newCard.name = "후시딘(B급)";
        newCard.description = "상처엔 후~\n가계부채표 후시딘";
        newCard.rank = 'B';
        myCard.Add(newCard);


        newCard = new Cards();
        newCard.image = imgs[144];
        newCard.name = "폴리스(B급)";
        newCard.description = "딱딱한 경찰 로봇 폴리스.\n나쁜 녀석들 꼼짝마!";
        newCard.rank = 'B';
        myCard.Add(newCard);


        newCard = new Cards();
        newCard.image = imgs[145];
        newCard.name = "파라솔 (B급)";
        newCard.description = "등에서 자라는 파라솔 두개!\n여름에 시원하겠다!";
        newCard.rank = 'B';
        myCard.Add(newCard);

        //0717 MAPLE
        newCard = new Cards();
        newCard.image = imgs[146];
        newCard.name = "야생 멧돼지";
        newCard.description = "야생의 멧돼지 와일드보어!\n무시무시한 뿔이 인상적이야!";
        newCard.rank = 'M';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[147];
        newCard.name = "초록 물풍선";
        newCard.description = "초록색 물풍선 슬라임!\n미끌미끌하고 건드리면 터질것 같아!";
        newCard.rank = 'M';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[148];
        newCard.name = "주니어 펭귄";
        newCard.description = "뒤뚱뒤뚱 귀여운 아기 펭귄!\n이름은 페페래요. 주니어 페페!?";
        newCard.rank = 'M';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[149];
        newCard.name = "어린이 네키";
        newCard.description = "네키 목도리를 닮은 아기 뱀이에요!\n귀여운 아기뱀 네키!";
        newCard.rank = 'M';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[150];
        newCard.name = "도끼 그루터기";
        newCard.description = "도끼가 찍힌 나무 그루터기에요!\n장작으로 쓰면 딱일것 같네요!";
        newCard.rank = 'M';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[151];
        newCard.name = "꽃매듭돼지";
        newCard.description = "분혹색 리본이 잘 어울리는 아기돼지!\n이렇게 귀여운 돼지를 어떻게 사냥해?!";
        newCard.rank = 'M';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[152];
        newCard.name = "주황색 버섯";
        newCard.description = "주황색 갓을 한 버섯이다!!\n마치 점프해서 뛰어오를 것 같아!";
        newCard.rank = 'M';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[153];
        newCard.name = "빨간색 달팽이";
        newCard.description = "빨간색 달팽이를 본 적 있어?\n세 형제 중에 제일 강하대!";
        newCard.rank = 'M';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[154];
        newCard.name = "팬티M";
        newCard.description = "M사이즈 팬티를 입은 세균!\n인텔 팬티M을 생각했다면 아재겠죠?";
        newCard.rank = 'A';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[155];
        newCard.name = "가디건";
        newCard.description = "초록색 가디건 어디 브랜드에요?\n머리랑 스타일이인 너무 잘 어울려요!";
        newCard.rank = 'A';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[156];
        newCard.name = "따라큐라";
        newCard.description = "어때요 내 드라큘라 코스튬?\n제가 직접 만들었어요!";
        newCard.rank = 'A';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[157];
        newCard.name = "피츄파츕스";
        newCard.description = "너무너무 귀여운 막대사탕!\n무슨 맛일지 궁금하다!";
        newCard.rank = 'A';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[158];
        newCard.name = "팬티M";
        newCard.description = "M사이즈 팬티를 입은 세균!\n인텔 팬티M을 생각했다면 아재겠죠?";
        newCard.rank = 'B';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[159];
        newCard.name = "가디건";
        newCard.description = "초록색 가디건 어디 브랜드에요?\n머리랑 스타일이인 너무 잘 어울려요!";
        newCard.rank = 'B';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[160];
        newCard.name = "따라큐라";
        newCard.description = "어때요 내 드라큘라 코스튬?\n제가 직접 만들었어요!";
        newCard.rank = 'B';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[161];
        newCard.name = "피츄파츕스";
        newCard.description = "너무너무 귀여운 막대사탕!\n무슨 맛일지 궁금하다!";
        newCard.rank = 'B';
        myCard.Add(newCard);


        newCard = new Cards();
        newCard.image = imgs[162];
        newCard.name = "샤오미드";
        newCard.description = "마치 핸드폰 광고 같지만\n협찬은 아니예요.";
        newCard.rank = 'A';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[163];
        newCard.name = "앱솔루트";
        newCard.description = "보도카 한 잔 어때요?\n윽 맛은 없을 것 같아요!";
        newCard.rank = 'A';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[164];
        newCard.name = "앱솔루트";
        newCard.description = "보도카 한 잔 어때요?\n윽 맛은 없을 것 같아요!";
        newCard.rank = 'B';
        myCard.Add(newCard);

        newCard = new Cards();
        newCard.image = imgs[165];
        newCard.name = "샤오미드";
        newCard.description = "마치 핸드폰 광고 같지만\n협찬은 아니예요.";
        newCard.rank = 'B';
        myCard.Add(newCard);

        foreach (var card in myCard)
        {
            if(card.rank == 'S') S_idx.Add(card.idx);
            else if(card.rank == 'A') A_idx.Add(card.idx);
            else if(card.rank == 'B') B_idx.Add(card.idx);
        }
        LoadData();
        PlayerPrefs.SetInt("TotalCards", myCard.Count);
        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        for(int i = 0; i<myCard.Count; i++)
        {
            if (!PlayerPrefs.HasKey("card_" + i))
            {
                PlayerPrefs.SetInt("card_" + i, 0);
                PlayerPrefs.Save();
            }
            myCard[i].count = PlayerPrefs.GetInt("card_" + i);
        }
    }

    public void AddData(int i)
    {
        if (!PlayerPrefs.HasKey("card_" + i))
        {
            PlayerPrefs.SetInt("card_" + i, 0);
        }
        PlayerPrefs.SetInt("myRealTotalCard", PlayerPrefs.GetInt("myRealTotalCard") + 1);
        PlayerPrefs.SetInt("card_"+i, PlayerPrefs.GetInt("card_" + i) + 1);
        PlayerPrefs.Save();
        LoadData();
    }

    public void OpenCard(int i, bool isnew = false)
    {
        if (i == -1) return;

        if (isnew)
        {
            new_ui.GetComponent<Animator>().SetTrigger("show");
            fx1.GetComponent<Animator>().SetTrigger("show");
            fx2.GetComponent<Animator>().SetTrigger("show");

            fx1.SetActive(true);
            fx2.SetActive(true);
            new_ui.SetActive(true);
            delay = true;
        }
        else
        {
            new_ui.GetComponent<Animator>().SetTrigger("hide");
            fx1.SetActive(false);
            fx2.SetActive(false);
            new_ui.SetActive(false);
            delay = false;
        }
        CardPanel.SetActive(true);
        StartCoroutine(OpenCardRoutine(i, isnew));
    }

    IEnumerator OpenCardRoutine(int i, bool isnew = false)
    {
        print("OPEN CARD : " + i);
        card_image.GetComponent<Image>().sprite = imgsHighres[i];
        title_ui.GetComponent<Text>().text = myCard[i].name;
        card.GetComponent<Animator>().SetTrigger("show");
        card_bg.GetComponent<Animator>().SetTrigger("show");
        text_board.GetComponent<Animator>().SetTrigger("show");

        pormpter_ui.GetComponent<Text>().text = myCard[i].description;
        pormpter_name_ui.GetComponent<Text>().text = myCard[i].name;
        if(hidden)
        {
            trans_btn.SetActive(true);
            hidden = false;
        }
        yield return null;
    }

    public void TestBtn()
    {
        /*
        int rnd = Random.Range(0, 5);
        AddData(rnd);
        print(myCard[rnd].count);
        OpenCard(rnd);
        */

        GameObject myBang = new GameObject();
        myBang = Instantiate(bbang, canvas.transform);
        myBang.SetActive(true);
    }

    //2, 35, 65
    public void OpenNew()
    {
        int rnd;
        if (PlayerPrefs.GetInt("myRealTotalCard") < 30)
        {
            int random = Random.Range(0, 100);
            if (random < 1)
            {
                rnd = GetCardIdxByRank('S');
            }
            else if (random < 60)
            {
                rnd = GetCardIdxByRank('A');
            }
            else
            {
                rnd = GetCardIdxByRank('B');
            }
        }
        else
        {
            int random = Random.Range(0, 100);
            if (random < 2)
            {
                rnd = GetCardIdxByRank('S');
            }
            else if (random < 33)
            {
                rnd = GetCardIdxByRank('A');
            }
            else
            {
                rnd = GetCardIdxByRank('B');
            }
        }

        isnew = IsNew(rnd);
        
        if (isnew)
        {
            myAydio.PlaySoundFx(0);
        }
        print("OPEN NEW : " + isnew);

        AddData(rnd);
        OpenCard(rnd, isnew);
    }

    public Sprite GetSpriteAt(int idx)
    {
        return imgs[idx];
    }
    
    public void OpenCardAt(int rnd)
    {
        isnew = IsNew(rnd);
        
        if (isnew)
        {
            myAydio.PlaySoundFx(0);
        }
        print("OPEN NEW : " + isnew);
        AddData(rnd);
        OpenCard(rnd, isnew);
    }

    public int GetCardIdxByRank(char rank)
    {
        int random;
        switch (rank)
        {
            case 'S' :
                random = Random.Range(0, S_idx.Count);
                return S_idx[random];
                break;
            case 'A' :
                random = Random.Range(0, A_idx.Count);
                return A_idx[random];
                break;
            case 'B' :
                random = Random.Range(0, B_idx.Count);
                return B_idx[random];
                break;
        }
        
        Debug.LogWarning("No rank found for GetCardIdxByRank : " + rank);
        return -1;
    }

    public void OpenMaple()
    {
        int rnd = Random.Range(0, myCard.Count);
        if (myCard[rnd].rank != 'M')
        {
            OpenMaple();
            return;
        }
        isnew = false;
        if (PlayerPrefs.GetInt("card_" + rnd) == 0)
        {
            isnew = true;
            myAydio.PlaySoundFx(0);
        }
        print("OPEN NEW : " + isnew);

        AddData(rnd);
        OpenCard(rnd, isnew);
    }

    public void HideCard()
    {
        if (delay) return;
        if (!card.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("sticker_show")) return;

        card.GetComponent<Animator>().SetTrigger("hide");
        card_bg.GetComponent<Animator>().SetTrigger("hide");
        text_board.GetComponent<Animator>().SetTrigger("hide");
        if (!hidden)
        {
            trans_btn.SetActive(false);
            hidden = true;
        }

        if(isnew)
        {
            
        }
        achievement.ReceiveRank();
    }

    public bool IsNew(int idx)
    {
        return PlayerPrefs.GetInt("card_" + idx) == 0;
    }
}
