# Editor

Lead dev only. Editor-time tooling for scene setup + dev convenience.

## Current Impl

- `DemoSceneBuilder` — Menu: `Tools > Build Demo Scene`. One-click demo scene: Ground, Platform, Player, Enemy, Coin, BouncePad, GameManager.

## DemoSceneBuilder Rules

- All ops Undo-supported (`Undo.RegisterCreatedObjectUndo`, `Undo.CollapseUndoOperations`).
- Same-name objects destroyed before recreation (`DestroyIfExists`).
- Ground layer auto-created if missing (`GetOrCreateLayer`).
- PlayerInputReader actions auto-wired from `Assets/InputSystem_Actions.inputactions`.
- PlayerMotor.groundCheck + groundLayer auto-wired via `SerializedObject`.

## Adding Scene Objects

1. Add `CreateXxx(...)` private static method, follow existing pattern.
2. Register with `Undo.RegisterCreatedObjectUndo`.
3. Add `DestroyIfExists("Xxx")` in `BuildDemoScene()` before Build section.
4. Call `CreateXxx(...)` in Build section.
5. SerializedProperty wiring needed? Follow `CreatePlayer` pattern.

## Constraints

- Editor scripts only — no runtime dependency. Must be under `Editor/`.
- Never reference Editor APIs from runtime scripts.
- `DemoSceneBuilder` = single static class. No MonoBehaviour.
- New tools: separate static class, own `[MenuItem]`.
