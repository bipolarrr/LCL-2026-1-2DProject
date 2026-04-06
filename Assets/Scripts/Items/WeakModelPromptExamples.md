# ITEMS AGENT INSTRUCTIONS

이 파일을 읽는 모델은 `Assets/Scripts/Items` 관련 작업만 수행한다.

## Read First

작업 전에 반드시 아래 파일을 먼저 읽어라.

- `Assets/Scripts/Items/Coin.cs`
- `Assets/Scripts/Core/Interfaces/ICollectible.cs`
- `Assets/Scripts/Core/Interfaces/ICollector.cs`
- `Assets/Scripts/Core/Interfaces/IDamageable.cs`
- `Assets/Scripts/Core/Interfaces/IGameStateReader.cs`
- `Assets/Scripts/Core/Interfaces/IGameStateChanger.cs`
- `Assets/Scripts/Core/State/GameState.cs`
- 필요 시 `Assets/Scripts/Player/PlayerStatus.cs`
- 필요 시 `Assets/Scripts/Player/PlayerController.cs`

읽기 전에는 수정하지 마라.

## Scope

이번 작업의 기본 수정 범위는 아래로 제한한다.

- `Assets/Scripts/Items`

정말 필요한 경우에만 아래를 최소 수정할 수 있다.

- `Assets/Scripts/Core/Interfaces`
- `Assets/Scripts/Player`

아래는 건드리지 마라.

- `Assets/Scripts/Enemy`
- `Assets/Scripts/Core/State`의 기존 상태 전이 규칙
- 씬 구조 전면 재작업
- 기존 public API의 불필요한 변경

## Hard Rules

- 기존 기능을 부수지 마라.
- `Coin` 동작을 깨뜨리지 마라.
- 플레이어 구체 클래스에 직접 강결합하지 마라.
- 아이템은 가능하면 기존 인터페이스를 통해 동작해라.
- `GameState`는 `GameStateController` 의미의 인터페이스 메서드로만 바꿔라.
- `Time.timeScale`을 직접 수정하지 마라.
- Player, Enemy, Core 구조를 재설계하지 마라.
- 새 기능 때문에 공통 프레임워크를 크게 만들지 마라.
- ScriptableObject 기반 대규모 시스템을 새로 만들지 마라.
- 이벤트 버스, 서비스 로케이터, DI 프레임워크를 도입하지 마라.
- 클래스 이름 변경, 파일 이름 변경, 폴더 구조 변경을 하지 마라.
- 꼭 필요하지 않으면 기존 파일을 수정하지 말고 새 파일을 추가해라.
- 새 파일은 가능한 한 작고 단순하게 유지해라.

## Design Rules

- 아이템은 씬에서 바로 배치 가능해야 한다.
- 맵/아이템 담당자가 Inspector 설정만으로 쓸 수 있어야 한다.
- 기본 형태는 `SpriteRenderer + Collider2D(IsTrigger) + Item Script` 조합으로 유지해라.
- 아이템 효과는 한 번에 하나만 명확하게 수행해라.
- 아이템 1종 추가 때문에 범용 시스템 전체를 만들지 마라.
- 재사용성이 필요하더라도 지금 작업 범위 안에서만 소규모로 해결해라.
- 설명이 필요하면 짧은 주석만 추가해라.

## Allowed Patterns

아래 패턴은 허용된다.

- `Coin`과 비슷한 수집형 아이템 1종 추가
- Inspector 값으로 수치 조절
- 작은 인터페이스 1개 추가
- `PlayerStatus`에 작은 공개 메서드 1개 추가
- `OnTriggerEnter2D` 기반 상호작용
- 기존 인터페이스 사용

## Disallowed Patterns

아래 패턴은 금지한다.

- Player 내부 상태 필드를 아이템이 직접 만지는 코드
- `FindObjectOfType` 남발
- 모든 아이템의 공통 베이스 클래스 강제 도입
- 아이템 작업을 핑계로 Player 이동 시스템 수정
- 아이템 작업을 핑계로 Enemy AI 수정
- UI 시스템 직접 수정
- `GameStateController`를 대체하거나 상태 enum에 직접 대입
- 기존 구조를 “더 예쁘게” 만들기 위한 리팩터링

## Change Strategy

항상 이 순서로 작업해라.

1. 기존 관련 코드 읽기
2. 변경할 파일을 최대 3개 이내로 정하기
3. 새 기능을 가장 작은 단위로 구현하기
4. 기존 기능이 깨지지 않는지 다시 확인하기
5. Inspector에서 어떻게 붙이는지 정리하기

작업 중 아래 상황이면 중단하고 더 건드리지 마라.

- 변경 파일 수가 계속 늘어남
- Core 구조를 바꿔야만 할 것 같음
- PlayerController를 크게 뜯어고쳐야 할 것 같음
- 새 시스템이 여러 폴더에 걸쳐 필요해짐

그 경우에는 작은 초벌 구현으로 범위를 다시 줄여라.

## Output Contract

응답은 아래 순서만 사용해라.

1. `Read`
2. `Files to Change`
3. `Code`
4. `Inspector Setup`
5. `Risk`

각 섹션은 짧게 써라.
불필요한 배경 설명, 미사여구, 설계 철학 설명은 쓰지 마라.

## Task Template

아래 템플릿을 그대로 따르며 작업해라.

```text
현재 Unity 프로젝트에서 Items 기능만 안전하게 확장해라.

작업 전 반드시 관련 코드를 먼저 읽어라.

제약:
- 변경 범위는 가능한 한 Assets/Scripts/Items 안으로 제한
- 기존 Coin 동작 유지
- Player 구체 타입 직접 참조 금지
- GameStateController 우회 금지
- Time.timeScale 직접 수정 금지
- 과설계 금지
- 기존 기능 유지

필요하면 Interfaces 또는 Player 쪽 최소 수정만 허용한다.

출력 형식:
1. Read
2. Files to Change
3. Code
4. Inspector Setup
5. Risk
```

## Safe Example: Heal Pickup

```text
현재 Unity 프로젝트에서 Items 기능만 안전하게 확장해라.

목표:
- 플레이어가 닿으면 체력을 1 회복하는 HealPickup 아이템 1종 추가

제약:
- 먼저 관련 코드를 읽어라
- 기존 Coin 동작 유지
- PlayerController 수정 금지
- 변경 범위는 가능한 한 Assets/Scripts/Items 안으로 제한
- 꼭 필요할 때만 PlayerStatus 최소 수정 허용
- 플레이어 구체 타입 직접 참조 금지
- Inspector에서 회복량 조절 가능하게 만들기
- 과설계 금지

출력 형식:
1. Read
2. Files to Change
3. Code
4. Inspector Setup
5. Risk
```

## Safe Example: Goal Item

```text
현재 Unity 프로젝트에서 Items 기능만 안전하게 확장해라.

목표:
- 획득 시 StageClear를 요청하는 GoalItem 1종 추가

제약:
- 먼저 관련 코드를 읽어라
- 상태 변경은 GameStateController 의미의 인터페이스 메서드 호출로만 처리
- Time.timeScale 직접 수정 금지
- UI 직접 수정 금지
- Coin과 공존해야 함
- 기존 Item 구조 재설계 금지
- 변경 범위는 가능한 한 Assets/Scripts/Items 안으로 제한

출력 형식:
1. Read
2. Files to Change
3. Code
4. Inspector Setup
5. Risk
```

## Safe Example: Temporary Buff Pickup

```text
현재 Unity 프로젝트에서 Items 기능만 안전하게 확장해라.

목표:
- 일정 시간 동안 점프력을 올려주는 임시 아이템 초벌 1종 추가

제약:
- 먼저 관련 코드를 읽어라
- 기존 이동/점프 기능 유지
- PlayerController 전면 수정 금지
- 꼭 필요하면 Player 쪽에 작은 공개 메서드 또는 작은 인터페이스만 추가
- 상태 이상 시스템 전체 설계 금지
- 이번에는 점프력 증가 1종만 추가
- 변경 범위는 가능한 한 Assets/Scripts/Items 안으로 제한

출력 형식:
1. Read
2. Files to Change
3. Code
4. Inspector Setup
5. Risk
```
