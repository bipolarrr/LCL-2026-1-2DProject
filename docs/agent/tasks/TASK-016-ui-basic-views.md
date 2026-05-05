# TASK-016-ui-basic-views

## Status
Planned

## Owner Role
UI

## Goal
Create basic UI view scripts for menu, HUD, cutscene, stage result, and game over that display state and request Core actions without directly changing game state.

## Source Design Sections
- 5.1 Core와 게임 흐름
- 9. Game Flow
- 11. Cutscene System
- 19. UI System
- 20. Save / Stage Result / Leaderboard Extension Point
- 21. Input System

## Dependencies
- TASK-004
- TASK-005
- TASK-008
- TASK-015

## Allowed Write Paths
- Assets/Scripts/UI/
- Assets/Scripts/UI/**/*.cs

## Forbidden Write Paths
- Assets/Scripts/Core/
- Assets/Scripts/Player/
- Assets/Scripts/Shared/
- Assets/Scripts/Enemy/
- Assets/Scripts/Items/
- Assets/Scripts/MapGimmicks/
- Assets/Scripts/Editor/
- Assets/**/*.asset
- Assets/**/*.unity
- Assets/**/*.prefab
- Packages/
- ProjectSettings/

## Read-only Context
- tdd.md
- Assets/Scripts/Core/
- Assets/Scripts/Core/Stage/
- Assets/Scripts/Core/Cutscenes/
- Assets/Scripts/Player/
- Assets/Scripts/Shared/

## Required Future Changes
Create `MainMenuView`, `HudView`, `CutsceneView`, `StageResultView`, and `GameOverView` scripts that bind display data and call approved Core request methods for user actions.

## Explicit Non-goals
- Do not create canvases, scenes, prefabs, or UI assets.
- Do not implement persistence.
- Do not edit Core, Player, or Shared.
- Do not read Player internals directly.

## Boundary Rules
- UI must not directly change game state.
- UI must request game-flow changes through Core.
- HUD must use a Player status model, event data, or approved read-only presentation data; it must not inspect Player internals.

## Contract Change Behavior
If UI needs edits in Core, Player, or Shared, stop and write a Contract Change Request instead of editing those paths.

## Acceptance Criteria
- Required view scripts exist and compile.
- UI action handlers call Core request entry points instead of scene APIs.
- HUD does not read concrete Player internals.
- Runtime code does not use `UnityEditor`.

## Definition of Done
- Changed files are limited to `Assets/Scripts/UI/`.
- Unity compile check passes.
- Boundary review confirms no Player internal reads and no direct state mutation.
- Manual UI wiring notes are added if inspector references are required later.

## Validation
- `git diff --name-only`
- `rg "PlayerHealth|PlayerMotor|PlayerMovement|currentHp|jumpCount|isInvincible|isGuarding|isDead" Assets/Scripts/UI`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/UI`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage|SceneManager\\.LoadScene" Assets/Scripts/UI`
- Unity compile check

## Notes for Future Codex Agent
Stay in UI. If a Core request method or presentation model is missing, file a Contract Change Request and stop.

