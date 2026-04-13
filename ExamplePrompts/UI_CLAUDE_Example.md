# UI

UI/Menu dev workspace. Lead dev reviews only.

## Role

Game UI. View layer only:
- **Display** values via event subscription (`OnHealthChanged`, `OnScoreChanged`). Never own or calculate data.
- **Forward** button actions to system public methods. No gameplay logic (no `if (hp < 30) ShowWarning()`).

## Current Impl

- `GameStateButtonActions` — Forward button clicks to `GameStateController.Instance` Request methods. Play, Pause, Resume, Restart.
- `GameStatePanelBinder` — Subscribe `OnStateChanged`, show/hide panels per `GameState`.

## Allowed

- New UI binders following `GameStatePanelBinder` pattern
- New button actions following `GameStateButtonActions` pattern
- Subscribe `IGameStateReader.OnStateChanged`
- Subscribe Player/Enemy public events for display
- Inspector refs for panels, text, images

## Forbidden

- Touch `Time.timeScale` (only via `GameStateController`)
- Direct assign `GameState` enum
- Gameplay rule calc (damage formulas, score multipliers)
- Edit files in Player, Enemy, Items, MapGimmicks
- `Find` scene objects for direct manipulation

## Review Checklist (Lead dev)

1. View only: No gameplay logic (conditionals, math) in UI scripts?
2. Event-based: Value updates via event subscription, not per-frame polling?
3. Via GameStateController: State changes through Request methods?
4. Folder isolation: Changes in `UI/` only?
5. Existing behavior preserved: Panels/buttons still work?
