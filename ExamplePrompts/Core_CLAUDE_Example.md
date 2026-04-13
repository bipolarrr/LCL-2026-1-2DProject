# Core

Lead dev only. No other member modify.

## Role

Project-wide interfaces + game state system. All inter-system contracts here.

## Structure

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
    └── GameStateController.cs  — Singleton, state transitions, Time.timeScale owner
```

## Interface Rules

- One behavior contract per interface. No bundling.
- Communicate through interfaces, not concrete classes.
- New interface: Lead dev decides. Members request only.

## GameStateController Rules

- Solo `Time.timeScale` owner.
- Fixed transitions only (Boot->Playing, Playing<->Paused, Playing->GameOver/StageClear).
- External change: call `IGameStateChanger` Request methods.
- New `GameState` values or transition changes: Lead dev only.

## New Interface Checklist

1. Existing interface solve it? Use that.
2. Name = behavior (`IStatusEffectReceiver`, `ILaunchReceiver`).
3. 1-2 methods max.
4. Impl in relevant folder, not Core.

## Modification Caution

- Interface signature change = project-wide impact. Check all impl first.
- `GameStateController` transition change ripple to UI, Enemy, Items.
