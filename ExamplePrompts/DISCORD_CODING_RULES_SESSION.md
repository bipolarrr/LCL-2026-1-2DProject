## 코딩 규약 설명 방법
---

## 이 프로젝트가 뭔가

- **Unity 6** 기반
- **2D 플랫포머**
- 여러 사람이 같이 만들어도 덜 망가지게 짜는 프로젝트

> 한 사람이 모든 걸 다 하는 게임이 아님.
> 작은 역할을 나눠서 만드는 팀 프로젝트형 게임임.

---

## 제일 중요한 규칙

### 한 파일은 한 가지 큰 일만 맡음.

- 입력 처리면 입력 처리
- 이동이면 이동
- 체력이면 체력
- UI면 UI

이걸 섞기 시작하면 나중에 고치기 어려워짐.

---

## `god class`가 뭔가

- 입력도 읽고
- 이동도 하고
- 체력도 깎고
- 점수도 올리고
- UI도 바꾸는

혼자 너무 많은 일을 하는 큰 클래스.

이 프로젝트는 그걸 피하려고 만든 구조임.

---

## `interface`가 뭔가

> `interface`는 "나는 이런 말을 알아들을 수 있음" 이라는 약속서.

예시:

- `IDamageable`: 데미지를 받을 수 있음
- `ICollector`: 점수를 받을 수 있음
- `ICollectible`: 수집될 수 있음
- `IKnockbackReceiver`: 밀려날 수 있음
- `IGameStateReader`: 게임 상태를 읽을 수 있음
- `IGameStateChanger`: 게임 상태 변경을 요청할 수 있음

이 구조를 쓰면:

- 적은 상대 내부를 몰라도 데미지를 줄 수 있음
- 코인은 플레이어 내부를 몰라도 점수를 줄 수 있음
- 버튼은 게임 규칙을 몰라도 상태 변경 요청을 보낼 수 있음

> 서로 내부를 다 몰라도 같이 일할 수 있게 만드는 장치임.

---

## 폴더 구조

```text
Assets/
  Scripts/
    Core/
      Interfaces/
      State/
    Player/
    Enemy/
    Items/
    MapGimmicks/
    UI/
    Editor/
  Scenes/
    SampleScene.unity
```

### 각 폴더가 하는 일

- `Core`: 공통 규칙
- `Interfaces`: 약속서 모음
- `State`: 게임 상태 관리
- `Player`: 플레이어 관련 코드
- `Enemy`: 적 관련 코드
- `Items`: 아이템 관련 코드
- `MapGimmicks`: 맵 장치
- `UI`: 버튼, 패널 같은 인터페이스 코드
- `Editor`: 씬 자동 생성 같은 에디터 도구

---

## 지금 씬에 있는 것

현재 `SampleScene`에는 대충 이런 오브젝트가 있음.

- `Main Camera`
- `Global Light 2D`
- `Ground`
- `Platform`
- `BouncePad`
- `Player`
- `Enemy`
- `Coin`
- `GameManager`

지금은 작은 2D 데모 씬이 이미 있는 상태.

---

## 플레이어 코드가 왜 나뉘어 있나

플레이어를 일부러 여러 조각으로 나눠 둠.

### `PlayerInputReader`

- 입력만 읽음
- 왼쪽, 오른쪽, 점프 확인
- 직접 움직이지는 않음

손가락이 뭘 눌렀는지만 적는 메모장 역할.

### `PlayerMotor`

- 실제 이동 담당
- 걷기, 점프, 넉백 처리
- Rigidbody2D 물리 담당

캐릭터의 다리와 몸 역할.

### `PlayerHealth`

- 체력
- 데미지
- 무적 시간
- 사망 판정

HP 담당.

### `PlayerController`

- 위 조각들을 연결함
- 입력을 보고 움직임을 요청함
- 점수도 관리함
- 하지만 세부 구현을 다 들고 있지는 않음

각 담당에게 일을 넘기는 반장 역할.

---

## 적, 아이템, 기믹은 어떻게 붙나

### `EnemyPatrol`

- 적이 좌우 이동
- 순찰만 담당

### `EnemyContactDamage`

- 닿은 대상에게 데미지와 넉백 전달
- 플레이어 내부값을 직접 건드리지 않음
- `IDamageable`, `IKnockbackReceiver`를 통해 처리

### `Coin`

- 닿으면 점수 전달
- `ICollector` 사용

### `BouncePad`

- 밟으면 위로 튕김
- `IKnockbackReceiver` 사용

> 코인, 적, 점프 발판이 플레이어 속을 직접 뜯어고치지 않게 만든 구조임.

---

## 게임 상태는 누가 관리하나

`GameStateController`가 담당.

상태 종류:

- `Boot`
- `Playing`
- `Paused`
- `GameOver`
- `StageClear`

이 스크립트가 하는 일:

- 현재 상태 저장
- 상태 전환 가능 여부 검사
- 일시정지면 시간 멈춤
- 플레이어가 죽으면 `GameOver`로 전환

> 게임 전체 흐름을 잡는 규칙판 역할.

---

## 지금 UI는 어디까지 되어 있나

현재 씬 기준으로는 아직:

- `Canvas`
- `Button`
- `Text`
- 실제 팝업 패널

같은 눈에 보이는 UI 오브젝트는 없음.

즉:

> 화면 UI는 아직 크게 붙지 않았고,
> UI를 붙일 준비용 코드가 먼저 있는 상태.

---

## UI 관련 스크립트

### `GameStateButtonActions`

- 버튼 클릭을 상태 변경 요청으로 바꿔 줌
- `Play`, `Pause`, `Resume`, `Restart` 요청을 보냄
- 버튼이 게임 규칙을 계산하지는 않음

### `GameStatePanelBinder`

- 특정 상태일 때 패널을 켜고 끔
- 예: `Paused`면 `PausePanel`, `GameOver`면 `GameOverPanel`

> 버튼은 요청만 보내고,
> 패널은 상태를 보고 켜지고 꺼짐.

---

## UI 규약

### UI는 보여주기와 버튼 전달만 함.

UI가 해도 되는 것:

- 값 표시
- 버튼 입력 전달
- 패널 켜기/끄기

UI가 하면 안 되는 것:

- 체력 계산
- 데미지 계산
- 이동 규칙 계산
- 게임 규칙의 핵심 판단

UI가 규칙까지 들고 있으면 코드가 꼬이기 쉬움.

---

## 이 구조가 좋은 이유

- 여러 명이 동시에 작업하기 쉬움
- 한 파일이 망가져도 피해가 덜 퍼짐
- 새 기능 추가가 쉬움
- 디버깅이 쉬움
- UI가 게임 로직을 망치지 않음

> 핵심은 "서로를 직접 꽉 잡지 말고, 작은 역할과 약속으로 협업하자"임.
