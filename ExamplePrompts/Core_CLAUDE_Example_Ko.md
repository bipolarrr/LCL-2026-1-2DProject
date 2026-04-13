# Core

리드 개발자 전용. 다른 팀원 수정 금지.

## 역할

프로젝트 전체 인터페이스 + 게임 상태 시스템. 시스템 간 계약이 여기에 있다.

## 구조

```
Core/
├── Interfaces/
│   ├── IDamageable.cs         — TakeDamage(int), ApplyKnockback(Vector2)
│   ├── IKnockbackReceiver.cs  — ApplyKnockback(Vector2)
│   ├── ICollectible.cs        — Collect(GameObject collector)
│   ├── ICollector.cs          — AddScore(int)
│   ├── IGameStateReader.cs    — CurrentState, OnStateChanged
│   ├── IGameStateChanger.cs   — RequestPlay/Pause/Resume/GameOver/StageClear/Restart
│   └── MapGimmicks/
│       └── IActivatable.cs    — Activate()
└── State/
    ├── GameState.cs            — enum: Boot, Playing, Paused, GameOver, StageClear
    └── GameStateController.cs  — 싱글턴, 상태 전이, Time.timeScale 소유자
```

## 인터페이스 규칙

- 인터페이스 하나 = 행동 계약 하나. 묶지 않는다.
- 구체 클래스가 아닌 인터페이스로 소통.
- 새 인터페이스: 리드가 결정. 팀원은 요청만.

## GameStateController 규칙

- `Time.timeScale`의 유일한 소유자.
- 전이 고정 (Boot→Playing, Playing↔Paused, Playing→GameOver/StageClear).
- 외부 변경: `IGameStateChanger`의 Request 메서드 호출.
- `GameState` 값 추가나 전이 변경: 리드만.

## 새 인터페이스 체크리스트

1. 기존 인터페이스로 해결 가능? 그걸 쓴다.
2. 이름 = 행동 (`IStatusEffectReceiver`, `ILaunchReceiver`).
3. 메서드 최대 1~2개.
4. 구현은 해당 폴더에. Core에 넣지 않는다.

## 수정 주의

- 인터페이스 시그니처 변경 = 프로젝트 전체 영향. 모든 구현체 먼저 확인.
- `GameStateController` 전이 변경은 UI, Enemy, Items에 파급.
