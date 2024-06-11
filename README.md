<br/>
<br/>

# <p align="center"> **GettingHarder**  </p>

##### <p align="center"> <b> 내일배움캠프 Unity 게임개발 숙련프로젝트 </b>

<br/>
<br/>

<br/>

---

### 📖 목차
+ [팀 소개](#팀-소개)
+ [프로젝트 소개](#프로젝트-소개)
+ [결과물 소개](#결과물-소개)
+ [기능 소개](#기능-소개)
+ [와이어 프레임](#와이어-프레임)
+ [기술 스택](#기술-스택)
+ [TroubleShooting](#TroubleShooting)
+ [UML 다이어그램](#UML-다이어그램)
---

### ✨팀 소개
| 이름   | 직책 | 직무 |
|--------|------|------|
| 권태하 | 팀장 | 적 생성 및 전투 AI 구축, 절차적 맵 생성 및 자원 리스폰, 저장 및 불러오기 |
| 김정석 | 팀원 | 플레이어 인풋 시스템, 생존 관리 시스템, 사물 인터랙션 및 장비 제작, BGM |
| 이강혁 | 팀원 | SingleTon 패턴 기초 구축, 건축 및 크래프팅 시스템 제작, 낮과 밤 시간 구현 |
| 이종민 | 팀원 | 게임 UI 및 씬 구현, 인벤토리 상호작용, 로그인 시스템 구현 |

---

### ✨프로젝트 소개

 `Info` **GettingHarder는 플레이어가 자원을 수집하고 생존을 위해 적과 싸우는 게임입니다. **

 `Stack` C#, Unity-2022.3.17f

 `Made by` **권태하, 김정석, 이강혁, 이종민** 

---

### ✨결과물 소개

### [🌾Team Notion](https://www.notion.so/teamsparta/efbfef6530864b86969e6a991f6b38c3)

### [🌕YouTube](https://www.youtube.com/watch?v=1O8rv1-oNSQ)

---

### ✨기능 소개

1. 스타트 씬 플로우

![1](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/429837d7-bfc0-4a0f-8c12-239bcee42af6)

2. 인벤토리

![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/3eddad95-17a9-45fa-8133-aa4548a5316b)

3. 건설

![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/d91a207b-bd57-4639-b4eb-b4f69a932e45)

4. 전투
   
![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/36077176-a2f8-46b1-8900-4b4d631b140a)

---

### ✨와이어 프레임

![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/aaae43f0-6d3d-4fde-bc73-cdac5e0f9b8e)

---

### ✨기술 스택
| 기술 스택   | 사용 이유 | 결 |
|:--------:|------|------|
| 절차적 맵 생성 | 생존 게임에서 자원 및 오브젝트가 무작위성을 가지면서도 일정한 생성 요건을 충족시키기 위해 | 게임 플레이의 예측 불가능성을 높이고 플레이어에게 새로운 도전과 탐험의 즐거움 제공 |
| 로그인 시스템 | 게임 개발에 필수적인 유저 정보 저장 시스템 구축을 위해 | 로그인을 통한 정보 관리 시스템 구현 |
| Cinemachine | TopDown View 구현 및 Camera Damping 기능 구현을 위해 | 플레이어를 부드럽게 따라가는 카메라 시스템 구축 |
| Json | 플레이어의 정보나 절차적 맵 생성을 통한 맵과 인벤토리의 정보를 받아오기 위 | 다양한 정보를 저장 및 불러오기 가능 |
| Raycast | 플레이어의 아이템 및 건물 상호작용을 위해 | Drag & Drop 및 건물 배치에 활 |
| Scriptable Object | 다양한 아이템을 관리하기 위해 | 다양한 아이템을 쉽게 생성하고 정리 가능 |
| EventSystem | Drag & Drop 기능과 UI 상호작용을 위해 | EventSystem을 통해 아이템 위치 변경 가능 |
| InputSystem | 플레이어의 여러가지 행동을 구현하기 위해 | 플레이어의 다양한 상호작용 및 행동 구현 |

---

### ✨TroubleShooting

1.
![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/46c05791-75d6-4e9e-9afb-b61085646536)

2.
![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/e46cc060-88be-4f98-b746-3169edda45b3)

3.
![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/baf4b374-4875-4dda-b746-6486151e7d10)

4.
![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/d87f6aaf-128b-4bc6-958f-bb4995a0e1d1)

---

### ✨UML 다이어그램

<전체 구조>
![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/0d456e50-8f0c-466c-bba4-46acd0e61c44)

<스타트 씬>
![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/a843d43e-ee14-4e15-95be-f1632df23a6a)

<싱글톤 패턴>
![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/498a0683-dcd2-4cd6-93c5-089da97053c6)

<환경>
![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/8b251974-97c6-4866-9c6c-0c03e16a4f15)

<인벤토리>
![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/7704c614-d78d-4b96-bd35-409dd6354f81)

<오디오 및 저장>
![image](https://github.com/DoOrNo33/GettingHarderPublic/assets/167051416/8b03963f-941f-4388-8b31-05d903fd2cc7)

---

