# 2D Platformer Technical Design Document

## Table of Contents
- [1. Overview](#section-1)
- [2. Goals](#section-2)
- [3. Non-Goals](#section-3)
- [4. Unity Version and Packages](#section-4)
- [5. Architecture Principles](#section-5)
  - [5.1 Core와 게임 흐름](#section-5-1)
  - [5.2 Player의 책임](#section-5-2)
  - [5.3 Enemy, Item, MapGimmick의 책임](#section-5-3)
  - [5.4 요청 메서드와 구조체 사용 규칙](#section-5-4)
  - [5.5 요청 구조체의 범위](#section-5-5)
  - [5.6 물리와 이동 처리](#section-5-6)
  - [5.7 PlayerMovement와 PlayerMotor의 역할](#section-5-7)
  - [5.8 컴포넌트 분리 기준](#section-5-8)
  - [5.9 모듈 간 경계](#section-5-9)
  - [5.10 씬 구성과 에디터 코드](#section-5-10)
  - [5.11 초보자 수정 가능 영역](#section-5-11)
  - [5.12 AI 코드 에이전트 작성 규칙](#section-5-12)
  - [5.13 핵심 요약](#section-5-13)
- [6. Folder Structure](#section-6)
- [7. Ownership and Contributor Boundaries](#section-7)
- [8. Scene Strategy](#section-8)
- [9. Game Flow](#section-9)
- [10. Stage System](#section-10)
- [11. Cutscene System](#section-11)
- [12. Player System](#section-12)
- [13. Combat System](#section-13)
- [14. Damage / Heal / Buff Request Contracts](#section-14)
- [15. Enemy System](#section-15)
- [16. Boss System](#section-16)
- [17. Item System](#section-17)
- [18. MapGimmick System](#section-18)
- [19. UI System](#section-19)
- [20. Save / Stage Result / Leaderboard Extension Point](#section-20)
- [21. Input System](#section-21)
- [22. Animation Contract](#section-22)
- [23. Physics Layers and Tags](#section-23)
- [24. SceneBuilder Tools](#section-24)
- [25. Coding Style](#section-25)
- [26. Forbidden Practices](#section-26)
- [27. Expected Deliverables](#section-27)
- [28. Agent Implementation Order](#section-28)
- [29. Acceptance Criteria](#section-29)
- [30. Definition of Done](#section-30)

<a id="section-1"></a>
## 1. Overview
- Unity 6000.3.11 기반 2D 플랫포머 게임의 기술 설계 문서임

- 본 프로젝트는 6인 협업으로 진행된다. 역할분배는 아래와 같음
    - 리드 개발자가 Core와 Player의 핵심 책임을 통제
    - 초보 개발자는 Enemy, Item, MapGimmick, UI 등 제한된 영역에서 작업
        - 외부 시스템은 Player 내부 상태를 직접 수정하지 않고 Request 계약을 통해 상호작용

- 기본 정신은 아래와 같음
    - 핵심 씬은 SceneBuilder를 통해 재생성 가능해야 함
        - Script화 된 Scene 생성 절차를 통해 모두가 *.meta 파일 지옥 / 서로 다른 화면을 보는 사태를 방지함
    - LLM 에이전트가 초벌 구현을 수행할 수 있도록 파일 구조, 책임 경계, 금지사항, 완료 기준을 명확히 해야 함

<a id="section-2"></a>

## 2. Goals

### Functional Goals
- Main Menu에서 게임을 시작할 수 있음
    - Main Menu는 Game Start, Settings, Credit 버튼과 그 기능을 가짐
- Cutscene 시스템을 통해 코드를 재사용하여 컷씬 표시가 가능함
    - 코드를 재사용, 자막의 내용과 표시할 스프라이트만 바꿔주면 되는 구조임
- Dialogue 시스템을 통해 화면 하단에 일러스트와 자막칸을 표시하는 구조로 하는 대화창을 표시할 수 있음
    - 코드를 재사용, 자막의 내용과 표시할 스프라이트만 바꿔주면 되는 구조임
- 일반 스테이지, 중간 보스, 최종 보스 구조를 표현해야 함
    - SceneBuilder를 통한 맵 / 에셋 생성임
- Player
    - 이동
    - 점프
    - 대시
    - 피격
    - 사망
    - 원거리 공격; 총
    - 근접 공격; 총검술
    - 활공
    상태를 가짐
- Enemy는 {Placeholder, 일단은 플레이어와 유사한 구조의 모듈화된 토대, 향후 FSM 기반의 모델을 개발}를 가짐
- 스테이지 클리어 후 결과창을 표시함
    - 플레이어가 직접 본인의 [학과] [이름]등을 작성해서 기록을 등록할 수 있어야 함
- 향후 클리어 기록과 리더보드 기능을 추가할 수 있는 구조를 가짐

### 반드시 구현할 것
    - MainMenuScene 생성
    - N개의 StageScene 생성
    - Player
    - Enemy 기본 구조 N종
    - Item 기본 구조 N종
    - MapGimmick 기본 구조 N종
    - Cutscene 재사용 구조
    - Dialogue 재사용 구조
    - Stage clear 결과창
    - SceneBuilder 기반 씬 구성

<a id="section-3"></a>
## 3. Non-Goals
데모 구현에서는 다음을 목표로 하지 않는다.

- 상용 수준의 완성된 보스 AI
- 완성된 스토리 연출
- 복잡한 Timeline 기반 컷씬
- 온라인 리더보드
- 클라우드 저장
- 세이브 파일 암호화
- 모든 스테이지의 완성
- 완성된 아트/사운드 연동
- 복잡한 인벤토리 시스템
- 복잡한 장비 시스템
- 대규모 스킬 트리
- Addressables 기반 리소스 관리
- DOTween, UniTask 등 외부 라이브러리 의존

<a id="section-4"></a>
## 4. Unity Version and Packages

### Unity Version
- Unity 6000.3.11

### Required Packages
- Unity Input System
- TextMeshPro
- Unity Test Framework

### Optional Packages
- {필요 시 추가}

### Not Allowed Without Approval
- DOTween
- UniTask
- Cinemachine
- Timeline
- Addressables
- 외부 Asset Store 코드 패키지

<a id="section-5"></a>
## 5. Architecture Principles

본 섹션은 사람과 AI 코드 에이전트가 모두 읽는 기준 문서임  
따라서 원칙은 어렵게 쓰지 않고, “누가 무엇을 책임지는지”, “무엇을 직접 건드리면 안 되는지”, “어떤 방식으로 연결해야 하는지”를 명확히 적음

---

<a id="section-5-1"></a>
### 5.1 Core와 게임 흐름

- Core는 게임 전체의 흐름을 관리함
  - 예: 게임 시작, 게임 오버, 일시정지, 씬 전환, 상태 전환
- 런타임에서 Core의 공개 진입점 클래스 이름은 `GameFlowController`로 통일함
- 씬 전환, 게임 상태 변경, 전체 진행 흐름은 Core를 통해 처리함
- UI는 게임 상태를 직접 바꾸지 않음
  - 예: UI 버튼이 직접 씬을 바꾸지 않고, Core에 “다음 씬으로 이동해달라”고 요청함
- 버튼, 메뉴, 일시정지, 결과창처럼 게임 흐름을 바꾸는 UI는 Core에 요청함
- 체력바, 점수 표시, 상태 표시처럼 값을 보여주는 UI는 직접 상태를 바꾸지 않고, 필요한 값을 읽거나 이벤트를 받아 화면만 갱신함

```cs
// 나쁜 예시
public void OnClickStartButton()
{
    SceneManager.LoadScene("GameScene");
}

// 좋은 예시
public void OnClickStartButton()
{
    gameFlowController.RequestSceneChange("GameScene");
}
```

```cs
// 상태 표시 UI 예시
public class HpBar : MonoBehaviour
{
    public void UpdateHp(int currentHp, int maxHp)
    {
        // 화면 표시만 갱신함
    }
}
```

---

<a id="section-5-2"></a>
### 5.2 Player의 책임

- Player는 자기 자신의 상태를 스스로 관리함
  - 예: 체력, 이동, 점프, 공격, 피격, 회복, 버프, 무적 시간, 경직 상태, 사망 상태
- 외부 시스템은 Player의 내부 값을 직접 수정하지 않음
  - 나쁜 예: Enemy가 Player의 체력을 직접 깎음
  - 나쁜 예: Item이 Player의 내부 변수를 직접 변경함
  - 나쁜 예: MapGimmick이 Player의 Rigidbody2D 속도를 직접 바꿈
- 외부 시스템은 Player에게 “요청”만 보냄
- Player는 요청을 받은 뒤, 자신의 현재 상태를 확인하고 실제 적용 여부를 결정함
  - 예: 무적 상태라면 데미지 요청을 무시할 수 있음
  - 예: 사망 상태라면 이동, 공격, 회복 요청을 무시할 수 있음
  - 예: 경직 상태라면 이동 요청을 제한할 수 있음

```cs
// 나쁜 예시
player.currentHp -= 10;

// 좋은 예시
player.ApplyDamage(10);
```

```cs
// 좋은 예시
DamageRequest request = new DamageRequest(
    amount: 10,
    damageKind: DamageKind.EnemyContact,
    knockbackDirection: knockbackDirection,
    knockbackForce: 8f,
    source: gameObject
);

player.ApplyDamage(request);
```

```cs
public void ApplyDamage(DamageRequest request)
{
    if (isDead)
    {
        return;
    }

    if (isInvincible)
    {
        return;
    }

    currentHp -= request.amount;

    if (request.knockbackForce > 0f)
    {
        movement.ApplyKnockback(request.knockbackDirection, request.knockbackForce);
    }
}
```

---

<a id="section-5-3"></a>
### 5.3 Enemy, Item, MapGimmick의 책임

- Enemy, Item, MapGimmick은 Player를 직접 조작하지 않음
- Enemy, Item, MapGimmick은 Player에게 요청만 보냄
  - 예: 데미지를 입혀달라
  - 예: 회복해달라
  - 예: 위로 튕겨달라
  - 예: 넉백을 적용해달라
- 실제 체력 변경, 속도 변경, 상태 변경은 Player 내부에서 처리함
- 초보자 담당 영역에서는 Player 내부 구조를 몰라도 요청 메서드만 호출할 수 있게 만듦

```cs
// Enemy 쪽 좋은 예시
private void OnHitPlayer(PlayerController player)
{
    player.ApplyDamage(10);
}
```

```cs
// Item 쪽 좋은 예시
private void OnCollected(PlayerController player)
{
    player.ApplyHeal(20);
}
```

```cs
// MapGimmick 쪽 좋은 예시
private void OnPlayerEnter(PlayerController player)
{
    player.ApplyBounce(15f);
}
```

---

<a id="section-5-4"></a>
### 5.4 요청 메서드와 구조체 사용 규칙

- 간단한 요청은 쉬운 메서드로 호출할 수 있게 함
  - 예: `ApplyDamage(10)`
  - 예: `ApplyHeal(20)`
  - 예: `ApplyBounce(15f)`
- 복잡한 요청은 구조체에 값을 담아 전달함
  - 예: `DamageRequest`
  - 예: `HealRequest`
  - 예: `BounceRequest`
  - 예: `KnockbackRequest`
- 여기서 구조체는 어려운 개념이 아니라, 요청에 필요한 값을 담는 단순한 데이터 묶음임
- 모든 실제 처리 로직은 구조체를 받는 메서드에 모음
- 쉬운 메서드는 내부에서 구조체를 만들어 구조체 버전 메서드를 호출함
- `Request(...)` 하나로 모든 요청을 받는 방식은 사용하지 않음
  - 이유: 호출부만 보고 데미지인지, 회복인지, 튕김인지 알기 어려움
- 메서드 이름은 행동이 드러나게 작성함
  - 좋은 예: `ApplyDamage`
  - 좋은 예: `ApplyHeal`
  - 좋은 예: `ApplyBounce`
  - 좋은 예: `ApplyKnockback`
  - 피할 예: `Request` `MyApply`

```cs
public readonly struct DamageRequest
{
    public readonly int amount;
    public readonly DamageKind damageKind;
    public readonly Vector2 knockbackDirection;
    public readonly float knockbackForce;
    public readonly GameObject source;

    public DamageRequest(
        int amount,
        DamageKind damageKind,
        Vector2 knockbackDirection,
        float knockbackForce,
        GameObject source)
    {
        this.amount = amount;
        this.damageKind = damageKind;
        this.knockbackDirection = knockbackDirection;
        this.knockbackForce = knockbackForce;
        this.source = source;
    }
}
```

```cs
public void ApplyDamage(int amount)
{
    ApplyDamage(new DamageRequest(
        amount: amount,
        damageKind: DamageKind.Unknown,
        knockbackDirection: Vector2.zero,
        knockbackForce: 0f,
        source: null
    ));
}

public void ApplyDamage(int amount, DamageKind damageKind, Vector2 knockbackDirection, float knockbackForce, GameObject source)
{
    ApplyDamage(new DamageRequest(
        amount: amount,
        damageKind: damageKind,
        knockbackDirection: knockbackDirection,
        knockbackForce: knockbackForce,
        source: source
    ));
}

public void ApplyDamage(DamageRequest request)
{
    if (isDead)
    {
        return;
    }

    if (isInvincible)
    {
        return;
    }

    currentHp -= request.amount;

    if (request.knockbackForce > 0f)
    {
        movement.ApplyKnockback(request.knockbackDirection, request.knockbackForce);
    }
}
```

```cs
// 비추천 예시
public void Request(DamageRequest request)
{
    ApplyDamage(request);
}

public void Request(BounceRequest request)
{
    ApplyBounce(request);
}

// 이유:
// player.Request(request)만 보고는 어떤 요청인지 바로 알기 어려움
```

---

<a id="section-5-5"></a>
### 5.5 요청 구조체의 범위

- 요청 구조체는 처음부터 많이 만들지 않음
- 필요한 요청부터 최소한으로 시작함
- 기본적으로 다음 정도만 우선 사용함
  - `DamageRequest`
  - `HealRequest`
  - `BounceRequest`
  - `KnockbackRequest`
- `MoveRequest`는 신중하게 사용함
  - 이유: 외부 시스템이 Player의 일반 이동까지 요청하기 시작하면 Player의 이동 주도권이 흐려질 수 있음
- 컨베이어벨트, 바람, 밀어내기 같은 특수 이동은 `MoveRequest`보다 더 구체적인 이름을 사용함
  - 예: `ExternalForceRequest`
  - 예: `PushRequest`
  - 예: `ConveyorRequest`
- 단순한 프로젝트라면 구조체를 새로 만들기보다 `ApplyBounce`, `ApplyKnockback` 같은 공개 메서드만으로 처리해도 됨

```cs
// 권장
player.ApplyDamage(10);
player.ApplyHeal(20);
player.ApplyBounce(15f);
player.ApplyKnockback(Vector2.left, 8f);
```

```cs
// 복잡한 경우에만 구조체 사용
KnockbackRequest request = new KnockbackRequest(
    direction: Vector2.left,
    force: 8f
);

player.ApplyKnockback(request);
```

---

<a id="section-5-6"></a>
### 5.6 물리와 이동 처리

- Player의 이동, 충돌, 넉백, 중력 처리는 프레임레이트에 따라 달라지지 않게 만듦
  - 예: 컴퓨터 성능에 따라 점프 높이, 이동 거리, 넉백 거리가 달라지면 안 됨
- 입력 확인과 물리 적용은 분리함
  - 예: `Update`에서는 입력을 확인함
  - 예: `FixedUpdate`에서는 Rigidbody2D 기반 이동과 물리를 적용함
- Player 이동 로직은 한 가지 방식으로 통일함
  - Rigidbody2D에 힘을 주는 방식과 위치를 직접 덮어쓰는 방식을 함부로 섞지 않음
- Player의 위치, 속도, Rigidbody2D 값은 외부 시스템이 직접 수정하지 않음
- 벽 비비기, 끼임, 튕김, 비정상 가속 같은 문제를 줄이기 위해 Rigidbody2D 조작 지점을 제한함
- Player의 실제 Rigidbody2D 조작은 Player 내부의 전담 컴포넌트에서만 처리함
  - 예: `PlayerMotor`
  - 예: `PlayerMovement`

```cs
private float moveInput;

private void Update()
{
    moveInput = Input.GetAxisRaw("Horizontal");
}

private void FixedUpdate()
{
    movement.Move(moveInput);
}
```

```cs
// 피해야 하는 예시
rigidbody2D.AddForce(Vector2.right * moveForce);
transform.position += Vector3.right * moveSpeed * Time.deltaTime;

// 한 가지 방식으로 통일한 예시
Vector2 velocity = rigidbody2D.linearVelocity;
velocity.x = moveInput * moveSpeed;
rigidbody2D.linearVelocity = velocity;
```

```cs
// 나쁜 예시
playerRigidbody.linearVelocity = new Vector2(0f, 15f);

// 좋은 예시
player.ApplyBounce(15f);
```

---

<a id="section-5-7"></a>
### 5.7 PlayerMovement와 PlayerMotor의 역할

- Player 이동 관련 코드는 너무 큰 하나의 클래스에 몰아넣지 않음
- 가능하면 판단과 실제 물리 적용을 나눔
- `PlayerMovement`는 이동 규칙과 이동 가능 여부를 관리함
  - 예: 이동 가능한가
  - 예: 점프 가능한가
  - 예: 경직 중인가
  - 예: 넉백 중인가
- `PlayerMotor`는 Rigidbody2D에 실제 값을 적용함
  - 예: 가로 속도 설정
  - 예: 세로 속도 설정
  - 예: 넉백 속도 적용
  - 예: 튕김 속도 적용
- Rigidbody2D를 직접 만지는 코드는 가능하면 `PlayerMotor` 안에 모음
- 초보자 담당 코드는 `PlayerMotor`를 직접 호출하지 않음
- 외부 시스템은 `PlayerController` 또는 Player의 공개 메서드까지만 호출함

```cs
public class PlayerMovement : MonoBehaviour
{
    private PlayerMotor motor;

    public void Move(float input)
    {
        if (!CanMove())
        {
            return;
        }

        motor.SetHorizontalVelocity(input);
    }

    public void ApplyBounce(Vector2 direction, float power)
    {
        motor.SetVelocity(direction * power);
    }

    private bool CanMove()
    {
        // 사망, 경직, 컷신 상태 등을 확인함
        return true;
    }
}
```

```cs
public class PlayerMotor : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    public void SetHorizontalVelocity(float x)
    {
        Vector2 velocity = rigidbody2D.linearVelocity;
        velocity.x = x;
        rigidbody2D.linearVelocity = velocity;
    }

    public void SetVelocity(Vector2 velocity)
    {
        rigidbody2D.linearVelocity = velocity;
    }
}
```

---

<a id="section-5-8"></a>
### 5.8 컴포넌트 분리 기준

- 상속보다 작은 컴포넌트 조합을 우선함
- 큰 부모 클래스를 만들고 모든 기능을 상속으로 해결하지 않음
- Player는 역할별 컴포넌트로 나눔
  - 예: `PlayerController`
  - 예: `PlayerMovement`
  - 예: `PlayerMotor`
  - 예: `PlayerHealth`
  - 예: `PlayerAttack`
  - 예: `PlayerStatus`
- 각 컴포넌트는 자기 역할만 담당함
- 컴포넌트끼리 연결할 때는 public 메서드, 이벤트, 요청 구조체를 사용함

```cs
// 좋은 예시
public class PlayerController : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerHealth health;
    private PlayerAttack attack;
    private PlayerStatus status;
}
```

```cs
// 피해야 하는 방향
public class PlayerBase : MonoBehaviour
{
    // 이동, 체력, 공격, 상태, 버프, UI 연동, 씬 전환을 모두 처리함
}
```

---

<a id="section-5-9"></a>
### 5.9 모듈 간 경계

- 모듈 사이에는 명확한 경계를 둠
- 다른 파트의 내부 구현을 직접 건드리지 않음
- 필요한 연결은 public 메서드, 인터페이스, 이벤트, 요청 구조체를 통해 처리함
- 필드는 가능하면 외부에서 직접 수정하지 못하게 함
- 외부에서 수정해야 하는 값은 메서드로 감쌈
- AI 코드 에이전트는 다른 모듈의 private 필드, 내부 상태, Rigidbody2D 값을 직접 수정하는 코드를 생성하지 않아야 함

```cs
public interface IDamageReceiver
{
    void ReceiveDamage(DamageRequest request);
}
```

```cs
// 나쁜 예시
player.currentHp += 10;

// 좋은 예시
player.ApplyHeal(10);
```

```cs
public class PlayerHealth : MonoBehaviour
{
    private int currentHp;
    private int maxHp;

    public void ApplyHeal(int amount)
    {
        currentHp = Mathf.Min(currentHp + amount, maxHp);
    }
}
```

---

<a id="section-5-10"></a>
### 5.10 씬 구성과 에디터 코드

- 씬 구성은 SceneBuilder를 통해 다시 만들 수 있어야 함
  - 예: 버튼, UI, 플레이어 시작 위치, 기본 오브젝트 배치를 코드로 재현할 수 있어야 함
- SceneBuilder는 개발 편의를 위한 코드임
- Runtime 코드는 UnityEditor API에 의존하지 않음
  - Runtime 코드는 실제 게임 실행 중에도 동작해야 하는 코드임
  - UnityEditor API는 에디터에서만 사용해야 함
- Editor 전용 코드는 `Editor` 폴더 아래에 둠
  - 예: SceneBuilder
  - 예: 자동 배치 도구
  - 예: 개발용 생성 스크립트

```cs
// Editor 폴더 아래에 두는 코드 예시
public class MainMenuSceneBuilder
{
    public static void Build()
    {
        CreateCanvas();
        CreateStartButton();
        CreateExitButton();
    }
}
```

```cs
// 나쁜 예시: Runtime 코드에서 UnityEditor 사용
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    private void Start()
    {
        EditorUtility.DisplayDialog("Debug", "Player Started", "OK");
    }
}
```

```cs
// 좋은 위치 예시
Assets/
  Scripts/
    Runtime/
      Core/
      Player/
      Enemy/
      Items/
      MapGimmicks/
      UI/
    Editor/
      SceneBuilders/
        MainMenuSceneBuilder.cs
        StageSceneBuilder.cs
```

---

<a id="section-5-11"></a>
### 5.11 초보자 수정 가능 영역

- 초보자가 수정 가능한 코드는 제한된 폴더에 둠
- 초보자 담당자는 Core와 Player 내부 핵심 로직을 직접 수정하지 않음
- 초보자 담당자는 정해진 공개 메서드와 요청 구조체를 사용해 기능을 연결함
- Enemy, Items, MapGimmicks는 비교적 수정하기 쉽게 작성함
- Core, Player, PlayerMotor, 공통 인터페이스는 리드 개발자가 관리함
- 필요한 기능이 없으면 내부 필드를 직접 건드리지 말고 리드 개발자에게 공개 메서드나 요청 구조체 추가를 요청함

```cs
Assets/
  Scripts/
    Runtime/
      Core/          // 송지한
      Player/        // 송지한
      Enemy/         // Enemy 담당자 수정 가능
      Items/         // Item 담당자 수정 가능
      MapGimmicks/   // MapGimmick 담당자 수정 가능
      UI/            // UI 담당자 수정 가능
```

```cs
// 작성해도 되는 형태
public class SpikeTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player == null)
        {
            return;
        }

        player.ApplyDamage(10);
    }
}
```

```cs
// 작성하면 안 되는 형태
public class SpikeTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        Rigidbody2D playerRigidbody = other.GetComponent<Rigidbody2D>();

        player.currentHp -= 10;
        playerRigidbody.linearVelocity = new Vector2(0f, 20f);
    }
}
```

---

<a id="section-5-12"></a>
### 5.12 AI 코드 에이전트 작성 규칙

- AI 코드 에이전트는 기존 구조를 우선 따라야 함
- 새 기능을 추가할 때 Core, Player, Enemy, Item, MapGimmick의 책임을 섞지 않음
- Player 내부 필드, Rigidbody2D, Transform을 외부 코드에서 직접 수정하지 않음
- Enemy, Item, MapGimmick에서는 Player의 공개 메서드만 호출함
- 복잡한 값 전달이 필요하면 구조체를 추가하거나 기존 구조체를 확장함
- 단, 요청 구조체를 불필요하게 많이 만들지 않음
- 메서드 이름은 행동이 드러나게 작성함
  - 예: `ApplyDamage`
  - 예: `ApplyHeal`
  - 예: `ApplyBounce`
  - 예: `ApplyKnockback`
- `Request(...)` 하나로 모든 요청을 처리하는 만능 메서드는 만들지 않음
- Rigidbody2D 조작 코드는 Player 내부 전담 컴포넌트에 모음
- Runtime 코드에서 UnityEditor API를 사용하지 않음
- 초보자 수정 가능 폴더에서 Core와 Player의 내부 구현을 직접 참조하지 않음

```cs
// AI가 생성하면 안 되는 방향
player.currentHp -= damage;
player.transform.position = respawnPosition;
playerRigidbody.linearVelocity = knockbackVelocity;

// AI가 생성해야 하는 방향
player.ApplyDamage(damage);
player.RequestRespawn();
player.ApplyKnockback(direction, force);
```

---

<a id="section-5-13"></a>
### 5.13 핵심 요약

- Core는 게임 흐름을 담당함
- Player는 자기 상태와 물리를 스스로 관리함
- UI는 게임 상태를 직접 바꾸지 않고 GameFlowController 또는 표시 대상에 요청함
- Enemy, Item, MapGimmick은 Player를 직접 수정하지 않고 공개 메서드로 요청함
- 간단한 요청은 `ApplyDamage(10)`처럼 쉽게 호출함
- 복잡한 요청은 구조체에 값을 담아 전달함
- `Request(...)` 만능 메서드는 사용하지 않음
- Player의 Rigidbody2D, 위치, 속도는 외부에서 직접 수정하지 않음
- 입력 확인과 물리 적용은 분리함
- Rigidbody2D 조작은 Player 내부 전담 컴포넌트에서 처리함
- SceneBuilder로 씬 구성을 재현 가능하게 만듦
- Runtime 코드는 UnityEditor API에 의존하지 않음
- 초보자가 수정 가능한 코드는 제한된 폴더에 둠
- 리드 개발자는 Core, Player, 공통 인터페이스, 물리 적용 구조를 관리함

<a id="section-6"></a>
## 6. Folder Structure

Assets/
  Scripts/
    Runtime/
      Core/
      Player/
      Enemy/
      Items/
      MapGimmicks/
      Stages/
      Cutscenes/
      UI/
      Save/
      Shared/
    Editor/
      SceneBuilders/
      Tools/
  ScriptableObjects/
    Player/
    Enemy/
    Items/
    Stages/
    Cutscenes/
  Prefabs/
    Player/
    Enemy/
    Items/
    MapGimmicks/
    UI/
  Scenes/
  Art/
    Sprites/
    Animations/
    Tilesets/
  Tests/
    EditMode/
    PlayMode/

> Assets/Scripts/Runtime 폴더에는 빌드에 포함되는 게임 실행 코드를 둔다.
> Assets/Scripts/Editor 폴더에는 Unity Editor에서만 실행되는 SceneBuilder와 개발 도구를 둔다.
> Assets/Scripts/Runtime/Shared 폴더에는 여러 시스템이 공유하는 Request, Interface, Enum을 둔다.
<a id="section-7"></a>
## 7. Ownership and Contributor Boundaries

### 송지한
수정 가능:
- Assets/Scripts/Runtime/Core/
- Assets/Scripts/Runtime/Player/
- Assets/Scripts/Runtime/Shared/
- Assets/Scripts/Editor/SceneBuilders/

책임:
- 게임 흐름
- Player 핵심 상태
- Request 계약
- SceneBuilder 유지보수
- 다른 담당자의 코드 리뷰 및 수정

### 조성민
수정 가능:
- Assets/Scripts/Runtime/UI/
- Assets/Prefabs/UI/

### 이경수
수정 가능:
- Assets/Scripts/Runtime/Enemy/
- Assets/ScriptableObjects/Enemy/
- Assets/Prefabs/Enemy/

### 안지혁 / 박진성
수정 가능:
- Assets/Scripts/Runtime/Items/
- Assets/Scripts/Runtime/MapGimmicks/
- Assets/Prefabs/Items/
- Assets/Prefabs/MapGimmicks/

### 주의사항
웬만해서는 송지한 제외 직접 수정 금지:
- Assets/Scripts/Runtime/Core/
- Assets/Scripts/Runtime/Player/
- Assets/Scripts/Runtime/Shared/

<a id="section-8"></a>
## 8. Scene Strategy

### Scene List
- MainMenu
- CutsceneTest
- Stage01
- TemporaryStage01
    - 개발 / 검증용

### Optional Scene Candidates
- Mid/FinalBoss01
    - 보스전 규모가 일반 스테이지와 명확히 다를 경우에만 분리
    - 보스전 배경 / 카메라 / 맵 크기 / 연출이 일반 스테이지와 완전히 다름
    - 보스전 진입 전에 별도 로딩 또는 컷신이 필요함
    - 보스전 실패 시 별도 재시작 지점이 필요함
    - 보스전 하나가 독립된 콘텐츠 단위임
    - 최종보스처럼 엔딩 / 크레딧 / 특수 연출과 강하게 연결됨
    - 일반 StageController로 처리하기엔 예외 로직이 너무 많음
- StageResult
    - 별도 결과 화면이 필요한 경우에만 분리 (필요 없을 것으로 추정됨)

### Scene Creation Policy
- 핵심 씬은 SceneBuilder로 생성한다.
- SceneBuilder는 Tools 메뉴에서 실행 가능해야 한다.
- SceneBuilder는 카메라, Canvas, EventSystem, StageController, PlayerSpawn 등을 생성한다.
- 핵심 참조는 SceneBuilder에서 연결한다.
- 수동 Inspector 연결을 개발 단계에서 사용할 수 있지만, 기능 완성시에는 SceneBuilder를 기반으로 수정한다.

<a id="section-9"></a>
## 9. Game Flow

### Main Flow
Boot
→ MainMenu
→ IntroCutscene
→ InGame(향후 정확한 플로우를 정해야)
→ StageResult
→ MainMenu or NextStage

### Temporary Stage Flow
Stage
→ TemporaryStage
→ ReturnToPreviousStage or SelectOtherStage

### Game Over Flow
Stage
→ PlayerDead
→ GameOverUI
→ RetryStage or MainMenu

### 실제 구현
- GameFlow는 GameFlowController가 관리한다.
- UI와 StageController는 GameFlowController에 요청만 보낸다.

<a id="section-10"></a>
## 10. Stage System

각 스테이지는 StageDefinition으로 식별한다

StageDefinition은 스테이지의 기본 정보만 가진다  
스테이지 진행 순서, 클리어 처리, 나가기 처리는 StageDefinition에 넣지 않는다

### StageDefinition Fields

- stageId
    - 스테이지 내부 식별자
    - 예: Stage01, MidBoss01, FinalBoss01, TemporaryStage01

- displayName
    - UI에 표시할 이름
    - 예: 1스테이지, 중간 보스, 최종 보스

- sceneName
    - Unity에서 로드할 씬 이름
    - Build Settings에 등록된 씬 이름과 일치해야 함

- stageType
    - 스테이지 종류
    - Normal, Boss, Cutscene, Temporary 정도만 사용

### Stage Clear Policy

스테이지 클리어는 StageController에 요청하는 방식으로 처리한다

GoalTrigger, BossEnemy, MapEvent 등은 직접 씬을 전환하지 않는다  
대신 StageController.RequestStageClear()를 호출한다

StageController는 클리어 상태를 기록하고 GameFlowController에 다음 흐름을 요청한다

```cs
public class GoalTrigger : MonoBehaviour
{
    private StageController stageController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        stageController.RequestStageClear();
    }
}
```

```cs
public class BossEnemy : MonoBehaviour
{
    private StageController stageController;

    public void Die()
    {
        stageController.RequestStageClear();
    }
}
```

### Stage Flow Policy

다음 스테이지 이동은 StageDefinition에 저장하지 않는다  
GameFlowController에서 명시적으로 처리한다

```cs
public void HandleStageCleared(string stageId)
{
    switch (stageId)
    {
        case "Stage01":
            LoadStage("Stage02");
            break;

        case "Stage02":
            LoadStage("MidBoss01");
            break;

        case "MidBoss01":
            LoadStage("FinalBoss01");
            break;

        default:
            LoadMainMenu();
            break;
    }
}
```

이 방식은 데이터 주도형 구조보다 덜 유연하지만, 현재 프로젝트에서는 흐름이 명확하고 디버깅하기 쉽다  
협업자가 실수로 StageDefinition의 ID를 잘못 연결해서 진행 흐름이 깨지는 문제도 줄일 수 있다

### Recommended Structure

```cs
public enum StageType
{
    Normal,
    Boss,
    Cutscene,
    Temporary
}
```

```cs
[Serializable]
public class StageDefinition
{
    public string stageId;
    public string displayName;
    public string sceneName;
    public StageType stageType;
}
```

### Runtime Flow

```text
GoalTrigger / BossEnemy / EventTrigger
→ StageController.RequestStageClear()
→ GameFlowController.HandleStageCleared(stageId)
→ 다음 씬 로드
```

<a id="section-11"></a>
## 11. Cutscene System

### Initial Scope
- 컷씬은 CutsceneDefinition으로 정의한다.
- CutsceneDefinition은 여러 CutsceneStep을 가진다.
- 각 Step은 speakerName, dialogueText, portraitSprite, backgroundSprite를 가진다.
- Submit 입력으로 다음 Step으로 이동한다.
- Skip 입력으로 컷씬을 종료한다.
- 컷씬 종료 후 GameFlowController에 완료를 알린다.

### Not Supported Initially (얼마든지 여지는 남겨야 함)
- 분기형 대화
- 선택지
- 복잡한 Timeline 연출
- 음성 재생

<a id="section-12"></a>
## 12. Player System

### Player Components
- PlayerController
- PlayerMotor
- PlayerHealth
- PlayerCombat
- PlayerInputReader
- PlayerBuffController
- PlayerAnimationController

### Player States
- Idle
- Run
- Jump
- Fall
- Dash
- AttackMelee
- AttackRanged
- Guard
- Hurt
- Dead
- Glide

### Rules
- Dead 상태에서는 입력을 무시한다.
- Jump count는 Grounded 상태에서 초기화된다.
- ExtraJumpBuff는 PlayerBuffController를 통해 적용된다.
- Guard 상태에서는 피해 감소 또는 무시 여부를 Player 내부에서 결정한다.
- 외부 시스템은 Player 내부 필드를 직접 수정하지 않는다.

<a id="section-13"></a>
## 13. Combat System

### Combat Overview
- Player는 하나의 Attack 입력을 가짐
- Attack 입력이 들어오면 PlayerAttackController가 현재 상황을 판정함
- Player 앞쪽 근접 범위 안에 Enemy가 있으면 Melee Attack 실행
- 근접 대상이 없으면 Ranged Attack 실행
- Player, Enemy, Trap, Projectile은 서로의 hp를 직접 수정하지 않음
- 피해는 DamageRequest를 통해 전달함
- 피해 적용 여부는 DamageRequest를 받은 대상이 스스로 판단함

### Attack Selection
- PlayerAttackController는 공격 입력과 공격 타입 선택만 담당함
- 공격 입력 시 MeleeTargetDetector를 통해 근접 공격 가능 대상을 확인함
- 근접 대상이 존재하면 근접 공격을 실행함
- 근접 대상이 존재하지 않으면 원거리 공격을 실행함
- 공격 타입 선택 기준은 마우스 위치나 입력 방식이 아니라 Player와 Enemy의 위치 관계를 기준으로 함

### Player Melee Attack
- Melee Attack은 별도의 MeleeHitbox GameObject를 활성화하지 않음
- 공격 시점에 Player 앞쪽의 근접 공격 범위를 검사함
- 검사 방식은 OverlapBox 또는 OverlapCircle을 사용함
- 감지된 Enemy에게 DamageRequest를 전달함
- 같은 공격 1회 안에서 같은 Enemy를 중복 타격하지 않음
- 중복 타격 방지는 공격 실행 내부의 hitTargets 목록을 사용함

### Player Ranged Attack
- Ranged Attack은 PlayerProjectile을 생성함
- PlayerProjectile은 direction, speed, amount, lifetime을 가짐
- Projectile은 Player의 현재 바라보는 방향을 기준으로 발사됨
- Projectile은 Enemy와 충돌하면 DamageRequest를 전달하고 사라짐
- Projectile은 lifetime이 끝나면 사라짐

### DamageRequest
- DamageRequest는 공격, 충돌, 함정 등으로 인해 대상에게 전달되는 피해 요청임
- DamageRequest는 class가 아니라 struct로 정의함
- DamageRequest 생성 자체는 가벼운 값 생성으로 취급함
- Melee Attack, Ranged Attack, Enemy Contact Damage는 서로 다른 DamageRequest를 생성할 수 있음
- 데미지 차이는 피격자 내부 분기가 아니라 공격자가 생성하는 DamageRequest 값으로 구분함
- 피격자는 DamageRequest를 받은 뒤 자신의 상태와 규칙에 따라 피해 적용 여부를 결정함

### DamageRequest Fields
- amount
- damageKind
- knockbackForce
- knockbackDirection
- source

### DamageKind
- Unknown
- PlayerMelee
- PlayerProjectile
- EnemyContact
- Trap
- Item

### Damage Receiver Policy
- Player와 Enemy는 IDamageReceiver를 통해 DamageRequest를 받음
- 외부 시스템은 Player 또는 Enemy의 hp를 직접 수정하지 않음
- 외부 시스템은 Player의 invincible, guarding, dead 같은 내부 상태를 직접 확인하지 않음
- DamageRequest를 받은 대상이 자신의 상태를 기준으로 피해 적용 여부를 결정함
- Player가 invincible 상태이면 DamageRequest를 무시함
- Player가 guarding 상태이면 방어 규칙에 따라 피해를 감소시키거나 무시함
- Player가 dead 상태이면 DamageRequest를 무시함
- Enemy도 필요한 경우 armor, shield, invincible 같은 내부 규칙으로 DamageRequest를 처리할 수 있음

### Enemy Contact Damage
- Enemy는 Player와 접촉 중일 때 DamageRequest를 생성함
- Enemy는 Player에게 직접 체력 감소를 적용하지 않음
- Enemy는 Player의 invincible 상태를 직접 확인하지 않음
- Enemy는 자신의 contactDamageCooldown만 확인함
- Contact Damage는 매 프레임 발생하지 않음
- Enemy는 contactDamageCooldown이 지났을 때만 DamageRequest를 생성함
- Player는 DamageRequest를 받은 뒤 자신의 상태를 기준으로 피해 적용 여부를 결정함
- Player가 invincible 상태이면 DamageRequest를 무시함

### Performance Policy
- DamageRequest는 struct로 정의하여 불필요한 heap allocation을 피함
- Contact Damage는 OnCollisionStay2D 또는 OnTriggerStay2D에서 매 프레임 처리하지 않음
- Contact Damage는 접촉 상태를 기록한 뒤 쿨타임이 되었을 때만 처리함
- GetComponent 반복 호출을 줄이기 위해 접촉 시작 시 IDamageReceiver를 캐싱할 수 있음
- 공격자와 충돌자는 피격자의 내부 상태를 읽지 않음
- 최적화는 Player 상태를 외부에서 확인하는 방식이 아니라 Request 발생 빈도를 줄이는 방식으로 처리함

### Implementation Policy
- PlayerAttackController는 공격 입력과 공격 타입 선택만 담당함
- MeleeTargetDetector는 근접 공격 가능 대상 확인만 담당함
- PlayerMeleeAttack은 근접 공격 판정과 DamageRequest 생성을 담당함
- PlayerRangedAttack은 Projectile 생성을 담당함
- PlayerProjectile은 이동, 수명, 충돌 시 DamageRequest 전달을 담당함
- EnemyContactDamage는 접촉 대상 관리와 contactDamageCooldown 관리를 담당함
- PlayerDamageReceiver는 Player 상태에 따라 DamageRequest 처리 여부를 결정함
- EnemyDamageReceiver는 Enemy 상태에 따라 DamageRequest 처리 여부를 결정함

### Example Code

```cs
public sealed class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private MeleeTargetDetector meleeTargetDetector;
    [SerializeField] private PlayerMeleeAttack meleeAttack;
    [SerializeField] private PlayerRangedAttack rangedAttack;

    public void RequestAttack()
    {
        if (meleeTargetDetector.HasTarget())
        {
            meleeAttack.Execute();
            return;
        }

        rangedAttack.Execute();
    }
}
```

```cs
public sealed class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] private Transform attackOrigin;
    [SerializeField] private Vector2 attackSize;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int damage = 3;
    [SerializeField] private float knockbackForce = 4f;

    private readonly Collider2D[] hitResults = new Collider2D[16];

    public void Execute()
    {
        int hitCount = Physics2D.OverlapBoxNonAlloc(
            attackOrigin.position,
            attackSize,
            0f,
            hitResults,
            enemyLayer
        );

        HashSet<IDamageReceiver> hitTargets = new();

        for (int i = 0; i < hitCount; i++)
        {
            if (!hitResults[i].TryGetComponent(out IDamageReceiver damageReceiver))
            {
                continue;
            }

            if (!hitTargets.Add(damageReceiver))
            {
                continue;
            }

            DamageRequest request = new DamageRequest
            {
                amount = damage,
                damageKind = DamageKind.PlayerMelee,
                knockbackForce = knockbackForce,
                knockbackDirection = transform.right,
                source = gameObject
            };

            damageReceiver.ReceiveDamage(request);
        }
    }
}
```

```cs
public sealed class PlayerRangedAttack : MonoBehaviour
{
    [SerializeField] private PlayerProjectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private int damage = 1;

    public void Execute()
    {
        PlayerProjectile projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation
        );

        projectile.Initialize(new ProjectileInitData
        {
            direction = firePoint.right,
            speed = projectileSpeed,
            amount = damage,
            source = gameObject
        });
    }
}
```

```cs
public sealed class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackForce = 4f;
    [SerializeField] private float contactDamageCooldown = 0.75f;

    private readonly HashSet<IDamageReceiver> damageReceivers = new();
    private float nextDamageTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageReceiver damageReceiver))
        {
            damageReceivers.Add(damageReceiver);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageReceiver damageReceiver))
        {
            damageReceivers.Remove(damageReceiver);
        }
    }

    private void Update()
    {
        if (Time.time < nextDamageTime)
        {
            return;
        }

        if (damageReceivers.Count == 0)
        {
            return;
        }

        foreach (IDamageReceiver damageReceiver in damageReceivers)
        {
            DamageRequest request = new DamageRequest
            {
                amount = damage,
                damageKind = DamageKind.EnemyContact,
                knockbackForce = knockbackForce,
                knockbackDirection = Vector2.zero,
                source = gameObject
            };

            damageReceiver.ReceiveDamage(request);
        }

        nextDamageTime = Time.time + contactDamageCooldown;
    }
}
```

```cs
public sealed class PlayerDamageReceiver : MonoBehaviour, IDamageReceiver
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerStateController stateController;
    [SerializeField] private PlayerKnockbackController knockbackController;

    public void ReceiveDamage(DamageRequest request)
    {
        if (stateController.IsDead)
        {
            return;
        }

        if (stateController.IsInvincible)
        {
            return;
        }

        if (stateController.IsGuarding)
        {
            ApplyGuardDamage(request);
            return;
        }

        playerHealth.ApplyDamage(request.amount);
        knockbackController.ApplyKnockback(
            request.knockbackDirection,
            request.knockbackForce
        );

        stateController.StartInvincibility();
    }

    private void ApplyGuardDamage(DamageRequest request)
    {
        int reducedDamage = Mathf.Max(0, request.amount - 1);

        if (reducedDamage > 0)
        {
            playerHealth.ApplyDamage(reducedDamage);
        }

        knockbackController.ApplyKnockback(
            request.knockbackDirection,
            request.knockbackForce * 0.5f
        );
    }
}
```

```cs
public interface IDamageReceiver
{
    void ReceiveDamage(DamageRequest request);
}
```

```cs
public struct DamageRequest
{
    public int amount;
    public DamageKind damageKind;
    public float knockbackForce;
    public Vector2 knockbackDirection;
    public GameObject source;
}
```

```cs
public enum DamageKind
{
    Unknown,
    PlayerMelee,
    PlayerProjectile,
    EnemyContact,
    Trap,
    Item
}
```
<a id="section-14"></a>
## 14. Damage / Heal / Buff Request Contracts

외부 시스템은 Player 내부 상태를 직접 변경하지 않는다.
외부 시스템은 Request 객체를 생성해 Player에게 전달한다.
Player는 Request를 해석하고 실제 적용 여부를 결정한다.

### DamageRequest
- DamageRequest는 13장의 canonical 정의를 사용한다.
- 필드는 amount, damageKind, knockbackDirection, knockbackForce, source로 통일한다.
- 피해를 받는 대상은 IDamageReceiver.ReceiveDamage(DamageRequest request)를 통해 DamageRequest를 받는다.

### 예시 코드
```cs
public struct HealRequest
{
    public int amount;
    public GameObject source;
}
```

```cs
public struct BuffRequest
{
    public BuffKind buffKind;
    public float value;
    public float durationSeconds;
    public GameObject source;
}
```

<a id="section-15"></a>
## 15. Enemy System

### Required Components
- EnemyController
- EnemyHealth
- EnemyMovement
- EnemyAttack
- EnemyContactDamage
- EnemyDeathHandler

### Rules
- Enemy는 PlayerHealth를 직접 참조하지 않는다.
- Enemy는 DamageRequest를 통해 Player에게 피해를 요청한다.
- Enemy가 사망하면 StageController에 EnemyDefeated 이벤트를 보낸다.
- Enemy 수치는 EnemyDefinition ScriptableObject에 둔다.

<a id="section-16"></a>
## 16. Boss System

### Initial Scope
- Boss는 Enemy 구조를 확장한 특수 Enemy로 취급한다.
- BossHealth가 0이 되면 StageController에 BossDefeated 이벤트를 보낸다.
- DefeatBoss 클리어 조건은 BossDefeated 이벤트를 기준으로 처리한다.

### Not Supported Initially (여지는 남겨야 함)
- 복잡한 페이즈 전환
- 다중 보스전

---
> 이하 아이템, 맵 시스템 내용을 실제 구현해볼 것.
> 지혁 / 진성 두 명의 아이디어가 적용된 실제 에셋 이전에, 기본적인 형태를 구현해볼 것.

<a id="section-17"></a>
## 17. Item System

### Initial Items
- HealItem
- MoveSpeedBuffItem
- ExtraJumpBuffItem

### Rules
- Item은 Player 내부 필드를 직접 수정하지 않는다.
- HealItem은 HealRequest를 생성한다.
- MoveSpeedBuffItem은 BuffRequest를 생성한다.
- ExtraJumpBuffItem은 BuffRequest를 생성한다.
- Item은 적용 후 제거되거나 비활성화된다.

<a id="section-18"></a>
## 18. MapGimmick System

### Initial Gimmicks
- FallingTrap
- CollapsePlatform

### FallingTrap
- Player가 감지 영역에 들어오면 delaySeconds 후 낙하한다.
- Player와 충돌하면 DamageRequest를 전달한다.

### CollapsePlatform
- Player가 밟으면 delaySeconds 후 비활성화된다.
- restoreSeconds가 0보다 크면 일정 시간 후 복구된다.

### Rules
- MapGimmick은 Player 내부 필드를 직접 수정하지 않는다.
- MapGimmick은 StageController를 직접 조작하지 않는다.

<a id="section-19"></a>
## 19. UI System

### Initial UI
- MainMenuView
- HudView
- CutsceneView
- StageResultView
- GameOverView

### Rules
- UI는 게임 상태를 직접 변경하지 않는다.
- UI는 GameFlowController에 요청만 보낸다.
- UI는 Player 내부 필드를 직접 읽지 않는다.
- HUD는 PlayerStatusModel 또는 이벤트를 통해 데이터를 받는다.

<a id="section-20"></a>
## 20. Save / Stage Result / Leaderboard Extension Point

### Initial Scope
- 온라인 리더보드는 구현하지 않는다.
- StageResult UI는 StageClearResult 데이터를 표시한다.
- 로컬 저장은 단순 JSON 또는 PlayerPrefs 기반으로 확장 가능하게 둔다.

### StageClearResult Fields
- stageId
- clearTimeSeconds
- deathCount
- damageTaken
- collectedItemCount
- clearedAt

<a id="section-21"></a>
## 21. Input System

### Backend
- Unity Input System

### Actions
- Move
- Jump
- Dash
- Attack
- Glide
- Submit
- Cancel
- Pause

### Rules
- Player는 PlayerInputReader를 통해 입력을 받는다.
- Attack 입력은 하나만 사용하며, PlayerAttackController가 상황에 따라 근접 공격 또는 원거리 공격을 선택한다.
- Cutscene과 UI는 Submit/Cancel을 사용한다.
- Dead 상태에서는 gameplay input을 무시한다.

<a id="section-22"></a>
## 22. Animation Contract

Animator를 가지는 모든 런타임 객체는 Animation Contract를 따른다.

Animation Contract는 애니메이션 에셋, Animator Controller, Animator Parameter가 일부 누락되어도 게임 로직이 중단되지 않도록 하기 위한 규칙이다.

애니메이션은 상태를 표현하기 위한 출력 계층이며, 게임 로직의 필수 실행 조건이 아니다.

---

### Global Animation Rules

- Animator 컴포넌트는 선택 사항이다.
- Animator가 없어도 객체는 정상적으로 동작해야 한다.
- Animator Controller가 없어도 객체는 정상적으로 동작해야 한다.
- 특정 Animator Parameter가 없어도 NullReferenceException 또는 Animator Parameter 관련 런타임 오류가 발생하지 않아야 한다.
- Runtime 코드는 Animator.StringToHash를 사용해 파라미터를 관리한다.
- Animator 파라미터 설정은 직접 호출하지 않고 SafeAnimator 같은 래퍼를 통해 수행한다.
- 애니메이션 이벤트는 핵심 게임 로직의 필수 트리거로 사용하지 않는다.
- 공격 판정, 데미지 적용, 사망 처리, 씬 전환 같은 핵심 로직은 애니메이션 재생 여부와 분리한다.

---

### Common Animator Parameters

Animator를 가지는 객체는 가능한 경우 아래 공통 파라미터 이름을 사용한다.

- moveX: float
- verticalVelocity: float
- isGrounded: bool
- isDead: bool
- hurt: trigger
- attack: trigger

공통 파라미터는 Player, Enemy, Boss에서 동일한 의미로 사용한다.

- moveX는 수평 이동 방향 또는 이동 의도를 의미한다.
- verticalVelocity는 현재 수직 속도를 의미한다.
- isGrounded는 지면 접촉 여부를 의미한다.
- isDead는 사망 상태를 의미한다.
- hurt는 피격 반응을 의미한다.
- attack은 일반 공격 시작을 의미한다.

---

### Player Animator Parameters

Player는 아래 파라미터를 사용할 수 있다.

- moveX: float
- verticalVelocity: float
- isGrounded: bool
- isDashing: bool
- isGliding: bool
- isDead: bool
- hurt: trigger
- meleeAttack: trigger
- rangedAttack: trigger

Player의 공격 애니메이션은 attack 하나로 통합하지 않고 meleeAttack, rangedAttack으로 분리한다.

근거리 공격과 원거리 공격은 입력 조건과 판정 방식이 다르므로, 애니메이션 파라미터도 분리한다.

---

### Player Required Animation States

Player Animator Controller를 구성하는 경우 아래 상태를 권장한다.

- Idle
- Run
- Jump
- Fall
- Dash
- Hurt
- Dead
- MeleeAttack
- RangedAttack
- Glide

단, 위 상태가 없어도 Player 로직은 정상적으로 실행되어야 한다.

---

### Enemy Animator Parameters

Enemy는 아래 파라미터를 사용할 수 있다.

- moveX: float
- verticalVelocity: float
- isGrounded: bool
- isDead: bool
- hurt: trigger
- attack: trigger
- isAlert: bool
- isMoving: bool

Enemy는 일반적으로 공격 종류가 많지 않으므로 attack 파라미터 하나를 기본으로 사용한다.

특정 Enemy가 여러 공격 패턴을 가진다면 아래처럼 전용 파라미터를 추가할 수 있다.

- attackA: trigger
- attackB: trigger
- specialAttack: trigger

단, Enemy 공통 코드가 특정 Enemy 전용 파라미터에 직접 의존해서는 안 된다.

---

### Enemy Recommended Animation States

Enemy Animator Controller를 구성하는 경우 아래 상태를 권장한다.

- Idle
- Move
- Alert
- Attack
- Hurt
- Dead

Enemy 종류에 따라 필요한 상태만 구현해도 된다.

---

### Boss Animator Parameters

Boss는 아래 파라미터를 사용할 수 있다.

- moveX: float
- verticalVelocity: float
- isGrounded: bool
- isDead: bool
- hurt: trigger
- phase: int
- attackIndex: int
- attack: trigger
- specialAttack: trigger
- intro: trigger
- enraged: bool

Boss는 일반 Enemy보다 상태와 패턴이 많으므로 phase, attackIndex 같은 파라미터를 사용할 수 있다.

- phase는 보스의 현재 페이즈를 의미한다.
- attackIndex는 선택된 공격 패턴 번호를 의미한다.
- intro는 보스전 시작 연출을 의미한다.
- enraged는 강화 상태 또는 폭주 상태를 의미한다.

단, Boss의 실제 패턴 실행은 Animator가 아니라 Boss AI 또는 Boss Controller가 담당한다.

Animator는 현재 Boss 상태를 표현할 뿐이다.

---

### Boss Recommended Animation States

Boss Animator Controller를 구성하는 경우 아래 상태를 권장한다.

- Idle
- Intro
- Move
- Attack
- SpecialAttack
- Hurt
- PhaseTransition
- Enraged
- Dead

Boss의 페이즈 전환, 공격 판정, 사망 처리는 애니메이션 상태 전환에 의존하지 않는다.

---

### Safe Animator Policy

Animator 접근은 SafeAnimator를 통해 수행한다.

SafeAnimator는 아래 상황에서 아무 작업도 하지 않고 조용히 반환해야 한다.

- Animator가 null인 경우
- Animator Controller가 없는 경우
- 요청한 파라미터가 Animator Controller에 없는 경우
- 객체가 비활성화된 경우

예시 코드

```cs
public sealed class SafeAnimator
{
    private readonly Animator animator;
    private readonly HashSet<int> parameterHashes = new HashSet<int>();

    public SafeAnimator(Animator animator)
    {
        this.animator = animator;

        if (animator == null || animator.runtimeAnimatorController == null)
        {
            return;
        }

        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            parameterHashes.Add(parameter.nameHash);
        }
    }

    public void SetFloat(int parameterHash, float value)
    {
        if (!CanSet(parameterHash))
        {
            return;
        }

        animator.SetFloat(parameterHash, value);
    }

    public void SetBool(int parameterHash, bool value)
    {
        if (!CanSet(parameterHash))
        {
            return;
        }

        animator.SetBool(parameterHash, value);
    }

    public void SetInteger(int parameterHash, int value)
    {
        if (!CanSet(parameterHash))
        {
            return;
        }

        animator.SetInteger(parameterHash, value);
    }

    public void SetTrigger(int parameterHash)
    {
        if (!CanSet(parameterHash))
        {
            return;
        }

        animator.SetTrigger(parameterHash);
    }

    private bool CanSet(int parameterHash)
    {
        if (animator == null)
        {
            return false;
        }

        if (!animator.isActiveAndEnabled)
        {
            return false;
        }

        if (animator.runtimeAnimatorController == null)
        {
            return false;
        }

        return parameterHashes.Contains(parameterHash);
    }
}
```

---

### Animator Hash Policy

Animator 파라미터 이름은 문자열로 직접 사용하지 않는다.

각 객체는 전용 Hash 클래스를 둔다.

예시 코드

```cs
public static class PlayerAnimatorHashes
{
    public static readonly int moveX = Animator.StringToHash("moveX");
    public static readonly int verticalVelocity = Animator.StringToHash("verticalVelocity");
    public static readonly int isGrounded = Animator.StringToHash("isGrounded");
    public static readonly int isDashing = Animator.StringToHash("isDashing");
    public static readonly int isGliding = Animator.StringToHash("isGliding");
    public static readonly int isDead = Animator.StringToHash("isDead");
    public static readonly int hurt = Animator.StringToHash("hurt");
    public static readonly int meleeAttack = Animator.StringToHash("meleeAttack");
    public static readonly int rangedAttack = Animator.StringToHash("rangedAttack");
}
```

```cs
public static class EnemyAnimatorHashes
{
    public static readonly int moveX = Animator.StringToHash("moveX");
    public static readonly int verticalVelocity = Animator.StringToHash("verticalVelocity");
    public static readonly int isGrounded = Animator.StringToHash("isGrounded");
    public static readonly int isDead = Animator.StringToHash("isDead");
    public static readonly int hurt = Animator.StringToHash("hurt");
    public static readonly int attack = Animator.StringToHash("attack");
    public static readonly int isAlert = Animator.StringToHash("isAlert");
    public static readonly int isMoving = Animator.StringToHash("isMoving");
}
```

```cs
public static class BossAnimatorHashes
{
    public static readonly int moveX = Animator.StringToHash("moveX");
    public static readonly int verticalVelocity = Animator.StringToHash("verticalVelocity");
    public static readonly int isGrounded = Animator.StringToHash("isGrounded");
    public static readonly int isDead = Animator.StringToHash("isDead");
    public static readonly int hurt = Animator.StringToHash("hurt");
    public static readonly int phase = Animator.StringToHash("phase");
    public static readonly int attackIndex = Animator.StringToHash("attackIndex");
    public static readonly int attack = Animator.StringToHash("attack");
    public static readonly int specialAttack = Animator.StringToHash("specialAttack");
    public static readonly int intro = Animator.StringToHash("intro");
    public static readonly int enraged = Animator.StringToHash("enraged");
}
```

---

### Responsibility Rule

애니메이션은 결과를 보여주는 역할만 가진다.

아래 로직은 Animator에 의존하지 않는다.

- 이동 가능 여부
- 점프 가능 여부
- 대시 가능 여부
- 공격 가능 여부
- 공격 판정 생성
- 데미지 적용
- 피격 무적 처리
- 사망 처리
- 보스 페이즈 전환
- 스테이지 클리어 처리

즉, Animator가 없어도 게임은 플레이 가능해야 한다.

Animator가 있으면 더 보기 좋게 표현될 뿐이다.

<a id="section-23"></a>
## 23. Physics Layers and Tags

### Layers
- Player
- Enemy
- Ground
- Item
- Gimmick
- PlayerProjectile
- EnemyProjectile
- Hitbox
- Hurtbox

### Tags
- Player
- Enemy
- MainCamera
- StageGoal

### Rules
- Item은 Trigger Collider를 사용한다.
- FallingTrap 감지 영역은 Trigger Collider를 사용한다.
- Ground 판정은 Ground LayerMask를 사용한다.
- Projectile은 충돌 후 제거된다.

<a id="section-24"></a>
## 24. SceneBuilder Tools

### Menu Items
- Tools/Game/Build Main Menu Scene
- Tools/Game/Build Stage 01 Scene
- Tools/Game/Build Cutscene Test Scene
- Tools/Game/Build All Core Scenes

### Responsibilities
- 씬 생성
- 기본 GameObject 생성
- Camera 생성
- Canvas 생성
- EventSystem 생성
- PlayerSpawn 생성
- StageController 생성
- 필수 참조 연결
- 씬 저장

### Not Responsible For
- 아트 리소스 생성
- 완성된 레벨 디자인
- 복잡한 애니메이션 생성

<a id="section-25"></a>
## 25. Coding Style

- Brace style은 Allman style을 사용한다.
- class, struct, enum, interface 이름은 PascalCase를 사용한다.
- method, parameter, local variable은 camelCase를 사용한다.
- constants는 SCREAMING_SNAKE_CASE를 사용한다.
- private serialized field는 camelCase를 사용하고 [SerializeField]를 붙인다.
- 파일명은 public class 이름과 일치시킨다.
- Runtime 코드에서 UnityEditor 네임스페이스를 사용하지 않는다.

<a id="section-26"></a>
## 26. Forbidden Practices

다음은 금지한다.

- Player의 health, speed, jumpCount를 외부에서 직접 수정
- Item, Enemy, MapGimmick에서 SceneManager.LoadScene 직접 호출
- Runtime 코드에서 UnityEditor 네임스페이스 사용
- GameObject.Find 사용
- FindObjectOfType 사용
- SendMessage 사용
- public field 남발
- God Manager 생성
- 담당 폴더 밖 코드 수정
- 수동 Inspector 연결 후 문서화하지 않는 것

<a id="section-27"></a>
## 27. Expected Deliverables
> AI 에이전트를 위한 구현 대상 리스트가 될 것 (이후 작성함)

<a id="section-28"></a>
## 28. Agent Implementation Order
> AI 에이전트를 위한 구현 순서 리스트가 될 것 (이후 작성함)

<a id="section-29"></a>
## 29. Acceptance Criteria
> AI 에이전트를 위한 구현 성공 검증 기준 리스트가 될 것 (이후 작성함)

<a id="section-30"></a>
## 30. Definition of Done
> AI 에이전트를 위한 구현 종료 기준 리스트가 될 것 (이후 작성함)
