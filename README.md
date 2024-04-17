# 포켓볼빵 : 더 게임
![](https://github.com/Sundrago/PocketBbang/blob/3ca8a986dc5c8030e6c1d570f1222285bb448a04/Docs/00_0.jpeg) | ![](https://github.com/Sundrago/PocketBbang/blob/3ca8a986dc5c8030e6c1d570f1222285bb448a04/Docs/00_1.jpeg) | ![](https://github.com/Sundrago/PocketBbang/blob/3ca8a986dc5c8030e6c1d570f1222285bb448a04/Docs/00_2.jpeg) | ![](https://github.com/Sundrago/PocketBbang/blob/3ca8a986dc5c8030e6c1d570f1222285bb448a04/Docs/00_3.jpeg) | ![](https://github.com/Sundrago/PocketBbang/blob/3ca8a986dc5c8030e6c1d570f1222285bb448a04/Docs/00_4.jpeg) | ![](https://github.com/Sundrago/PocketBbang/blob/3ca8a986dc5c8030e6c1d570f1222285bb448a04/Docs/00_5.jpeg) |
---:|:---:|:---:|:---:|:---:|:---

<br />

<p align="center"><a href="https://apple.co/3wlfK6V">
<img src="https://github.com/Sundrago/Cocktailor/blob/15a867f85fc3829dc0730dccfe3db2180e624dce/Docs/appstore.png" width="200"">
</a></p>

<p align="center"><a href="https://play.google.com/store/apps/details?id=net.sundragon.bbang">
<img src="https://github.com/Sundrago/Cocktailor/blob/15a867f85fc3829dc0730dccfe3db2180e624dce/Docs/playstore.png" width="200"">
</a></p>

<br />
<hr>
<br />

### 개요
- 개발기간: 2022.07 ~ 2023.12 (LiveOps 12개월)
- 개발 환경 : Unity 2021.3 LTS
- 플랫폼: Android/ iOS
- 장르: 캐주얼/ 수집형 시뮬레이션
- 개발 인원: 1명 (개인 프로젝트)

### 성과
- 누적 다운로드: 100k+ (iOS)
- D1 리텐션: 45%+ (iOS)

### 한줄소개
-  ‘포켓볼빵 있어요?’ 편의점을 돌며 빵을 구하자, 포켓볼빵 시뮬레이션 게임!

### 개발배경
- 포켓볼빵이 처음 출시되었을 때 품절 대란으로 인해 큰 이슈가 되었습니다. ‘편의점을 돌아다니면서 빵을 구하는’ 간단한 게임 프로토타입을 만들었는데, 주변 지인들에게 의외로 큰 호응을 얻게 되어  정식 게임으로 발전하게 되었습니다.
- 게임이 출시된 후에는 많은 유저들로부터 다양한 피드백을 받았고, 그 피드백들이 게임을 재미있게 발전시키는 데 중요한 역할을 했습니다. 유저들의 의견을 반영하여 게임의 밸런스를 조정하고, 새로운 기능들을 추가하며, 게임의 스토리를 더 풍부하게 발전시켰습니다. 출시 이후 6개월이 넘도록 라이브옵스를 진행하면서, 밤잠을 줄이고 게임 개발에 몰두했지만, 그 과정이 무척 즐거웠습니다.

<br />
<hr>
<br />

## 게임 플레이
<!-- 01 -->
![](https://github.com/Sundrago/PocketBbang/blob/3ca8a986dc5c8030e6c1d570f1222285bb448a04/Docs/01.jpg) | <div> ① 랭킹(버튼) : 현제 플레이어의 랭킹 아이콘 출력<br>❷ 현재 체력 상태 : 완충 상태(6칸)가 아닐 경우, 회복할 때 까지 남는 시간 출력.<br>❸ 미개봉 빵(버튼) : 안 뜯은 빵 갯수 출력<br>④ 뜯은 빵 수 : 메인화면에 있는 뜯은 빵 수 출력<br>⑤ 붕어빵(버튼) : 붕어빵 아이템 사용 및 구매 팝업 출력<br>⑥ 다이아몬드 : 소지한 다이아몬드 수량 출력<br>⑦ 빵조각 : 소지한 빵 조각 출력<br>⑧ 용돈 : 소지한 돈 수량 출력<br>⑨ 캐시상점(버튼) : 클릭 시 캐시 상점화면 출력<br>⑩ 성실함의 보상(버튼) : 클릭 시 출석 보상화면 출력<br>⑪ 뜯은 빵 : 개별 빵/아이템 오브젝트, 드래그 가능, 클릭 시 먹기 버튼 팝업<br>⑫ 비둘기 공원(버튼) : 클릭 시 비둘기 공원으로 이동<br>⑬ UI최소화(토글) : 클릭 시 UI 버튼 숨기기/보이기<br>⓮콜렉션(버튼) : 클릭 시 UI 콜렉션 화면 출력<br>⓯편의점 이동(버튼) : 클릭 시 편의점으로 이동<br>⓰핸드폰 확인(버튼) : 클릭 시 핸드폰 화면 출력</div> |
---:|:---
> ### 메인화면 구성
> ‘편의점 가기’버튼을 누르면 편의점 투어를 시작합니다.  
> ‘비둘기 공원’으로 이동하면 유저의 주간/전체 랭킹을 확인하거나, 비둘기에가 밥주기(뽑기) 이벤트를 진행할 수 있습니다.  
> ‘핸드폰’에서는 단군마켓(당근마켓), 알밥천국(알바천국)등 의 기능을 이용할 수 있습니다.
<br />

<!-- 02 -->
![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/02_0.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/02_1.gif) | ![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/02_2.jpg)
---:|:---:|:---
> ### 빵과 스티커
> 플레이어는 여러 편의점을 방문하여 포켓볼빵을 획득하고 그 안에 들어있는 스티커를 모아야 합니다. 가능한 많은 종류의 스티커를 모아 자신의 콜렉션을 완성하는 것이 플레이어의 주요 게임 목표입니다.
<br />

<!-- 03 -->
![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/03_0.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/03_1.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/03_2.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/03_3.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/03_4.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/03_5.jpg) |
---:|:---:|:---:|:---:|:---:|:---
> ### 160종의 스티커와 콜렉션
>  게임에서 빵을 뜯을 때마다 스티커를 획득할 수 있습니다. 스티커는 S/A/B 세 가지 등급으로 나누어져 있으며, 콜렉션의 다양성과 재미를 높이기 위해 노력하였습니다. (B급 스티커는 그림판으로 그렸습니다.) 업데이트를 통해 계속해서 스티커를 추가하였으며 현재 160개 이상의 스티커를 수집할 수 있습니다.  
> 플레이어는 콜렉션 메뉴에서 자신이 모은 모든 스티커와 아직 모으지 못한 스티커를 확인할 수 있으며, 소셜 셰어 기능을 통해 스티커 콜렉션을 친구와 공유할 수도 있습니다.
<br />

<!-- 04 -->
![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/04_0.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/04_1.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/04_2.jpg)
---:|:---:|:---
> ### 편의점
> 플레이어는 포켓볼빵을 얻기 위해 여러 편의점을 방문합니다. 잼미니스톱, 포도씨유, 회미리마트 등 총 7개의 편의점이 있으며, 각 편의점에서 만나는 NPC와의 친밀도를 쌓아 빵을 획득할 확률을 높일 수 있으며, 일부 편의점에서는 미니게임을 통해 새로운 빵을 얻거나 체력을 회복할 수 있습니다.
<br />

<!-- 05 -->
![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/05_0.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/05_1.jpg) |
---:|:---
> ### 체력 시스템
> 새로운 편의점을 방문할 때마다 플레이어는 체력을 한 칸씩 소모하게 됩니다. 체력은 5분에 한 칸씩 회복되며, 아이템을 사용하거나 광고 시청을 통해 체력을 즉시 회복할 수도 있습니다.
<br />

<!-- 06 -->
![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/06_0.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/06_1.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/06_2.jpg)
---:|:---:|:---
> ### NPC 시스템
> 각 편의점에는 특정한 NPC가 있습니다. 플레이어는 총 5명의 NPC와 친구가 될 수 있으며 도전과제(Achievement)와 연동되어 있어 진척도를 확인할 수 있습니다. NPC와의 대화 선택지에서 적절한 답변을 선택하는 것이 중요합니다. 성공적으로 NPC와 친구가 되면, 빵이 들어오는 시간을 알려주거나, 가게 알바를 도와주는 특별한 이벤트가 발생합니다.
<br />

<!-- 06-B -->
![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/06_b.jpg)  
> ### NPC 대사 작성 툴 개발
> 구글 시트 기반의 대사 작성 툴을 개발했습니다. App Script를 활용해 구글 시트에서 작성한 대사를 자동으로 C#으로 변환하여 개발 시간을 단축하고 오류를 줄일 수 있었습니다. 이를 통해 플레이어 선택에 따른 NPC와의 친밀도 변화와 특별한 이벤트, 대화 발생 등 게임 재미를 높일 수 있었습니다.
<br />

<!-- 07 -->
![](https://github.com/Sundrago/PocketBbang/blob/5c56b34fb28c5ced0f370dd28c982406a94a2010/Docs/07_0.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/07_1.jpg) | 
---:|:---:
> ### 구글시트 연동 확률형 가챠, 인앱결제
> 구글 시트 기반의 확률형 가챠 시스템을 구축하여 게임 내 아이템 확률 관리 및 업데이트 시간을 축소했습니다. App Script를 통해 구글 시트 데이터를 기반으로 Unity에서 활용 가능한 JSON 데이터를 자동 생성함으로써 코드 수정 없이 빠르게 업데이트할 수 있게 되었습니다. 상품 정보, 가격, 설명 등의 UI 및 시스템을 자동 생성함으로써 개발 시간을 단축하고 오류 가능성을 감소시킬 수 있었습니다.
<br />

<!-- 08 -->
![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/08_0.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/08_1.png)
---:|:---:
> ### 푸시알림 
> 미소녀 알바생과 친구가 된 플레이어에게 '빵 입고 알림'을 제공하여 리텐션을 높이고자 했습니다. 게임 내 상황과 연계된 자연스러운 푸시 알림을 통해 플레이어에게 편한 경험을 제공했으며, 알림 확인 속도에 따른 1-3개의 빵 보상으로 긴장감을 유발하고 빠른 접속을 유도했습니다. 또한 매일 오전 9시부터 저녁 21시 사이 랜덤으로 알림을 발송하여 예상 불가능성으로 플레이어의 흥미를 유발하고 지속적인 관심을 유지할 수 있었습니다.
<br />

<!-- 09 -->
![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/09_0.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/09_1.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/09_2.jpg)
---:|:---:|:---
> ### 미니게임 
> 반복하여 편의점을 방문하는 과정이 지루하지 않도록 미니 게임 형식의 컨텐츠 업데이트를 계속 진행했습니다.  
> * 편의점 알바 미니게임: 편의점 알바를 하며 포켓볼빵을 찾는 손님들을 돌려보내는 게임  
> * 인형뽑기 미니게임: 인형 뽑기 기계에서 포켓볼빵을 뽑는 게임  
> * 떡볶이 미니게임: 떡볶이 가게에서 떡볶이를 획득한 만큼 체력을 회복하는 게임  
> * 왕족발 미니게임: 왕충동 장족발 가게에서 쌈을 싸서 손님 입에 던지는 게임  
> * 탕후루 가게 알바 미니게임: 탕후루 가게에서 알바로 탕후루를 만드는 게임
<br />

<!-- 10 -->
![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/10_0.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/10_1.jpg) | ![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/10_2.jpg)
---:|:---:|:---
> ### 단군마켓을 통한 밸런스 조절
> 게임을 플레이하다 보면 같은 종류의 스티커가 반복해서 뽑히게 되고, 배경화면 속에 빵이 쌓여 이를 소비하는 기능이 필요했습니다. 이러한 밸런스를 조절하기 위해 ’단군마켓’을 통해 여러 장의 스티커를 판매하거나 교환하는 기능들을 추가하였습니다.  
> 추가로 플레이어가 게임에 주기적으로 접속할 수 있도록 ‘빵셔틀’기능을 추가해 30분마다 빵을 하나씩 가져오도록 만들었습니다.
<br />

<!-- 11 -->
![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/11.jpg)
> ### Firebase 연동
> Firebase와 연동하여 플레이어의 데이터를 수집하고 있습니다. 유저 스코어, 플레이시간, 이탈구간, 등 데이터를 수집하고 있으며, 미니게임의 난이도(보상 구간)을 정하는데 도움이 되었습니다.

<br />

<!-- 12 -->
![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/12.jpg)
> ### 유저 리뷰
> 누적다운로드 10만을 기록하고 ‘개발자 놀고 있냐, 왜 업데이트 안하냐'와 같은 잔소리를 들을 정도로 유저 리뷰가 활발하게 이뤄졌습니다. 한참 인기가 있을 때에는 매일 같이 리뷰가 쏟아져서 여자친구가 CS를 도와주기도 할 정도였습니다. 실제로 게임의 개선 방향을 고민하고 결정하는 데 유저의 리뷰가 굉장히 큰 도움이 되었습니다. 적극적인 피드백 문화에 감탄하기도 했고, 더 열심히 콘텐츠를 추가하고 업데이트하는 데 큰 원동력이 되었습니다. 20종으로 시작한 스티커도 현재 160종이 넘도록 컨텐츠를 추가하였습니다. 유저와 소통하며 게임을 개선해 나아가는 과정은 지금까지 경험해보지 못한 재미있는 경험이었습니다.
<br />

<!-- 13 -->
![](https://github.com/Sundrago/PocketBbang/blob/0c181bec26d392b183b472213ffc51e53d9ec5b6/Docs/13.jpg)
> ### 한계점 및 개선 사항
> '포켓몬 빵 품귀 현상'이라는 사회적 이슈에 편승해 큰 주목을 받았지만, 포켓몬 빵에 대한 관심이 줄어들면서 자연스럽게 다운로드 수 역시 줄어들었습니다. 또한 이슈 자체가 국내에 한정되어 해외 유저의 공감을 이끌어내기 어렵다는 점도 한계점으로 평가됩니다.  
> 그럼에도 불구하고 지속적으로 게임을 플레이하는 유저가 있어서 2달에 한 번 정도 업데이트를 진행하였고, 최근까지도 주간 평균 200-300명의 신규 유저가 유입되고 있습니다.
<br />
