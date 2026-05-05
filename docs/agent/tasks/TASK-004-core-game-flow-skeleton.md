# TASK-004-core-game-flow-skeleton

## Status
Planned

## Owner Role
Core

## Goal
Create the Core game-flow skeleton that owns game start, pause, retry, main menu routing, game over, stage progression, and scene change request entry points.

## Source Design Sections
- 5.1 Core와 게임 흐름
- 8. Scene Strategy
- 9. Game Flow
- 19. UI System
- 26. Forbidden Practices

## Dependencies
- TASK-002
- TASK-003

## Allowed Write Paths
- Assets/Scripts/Core/
- Assets/Scripts/Core/**/*.cs

## Forbidden Write Paths
- Assets/Scripts/Player/
- Assets/Scripts/Enemy/
- Assets/Scripts/Items/
- Assets/Scripts/MapGimmicks/
- Assets/Scripts/UI/
- Assets/Scripts/Shared/
- Assets/Scripts/Editor/
- Assets/**/*.unity
- Assets/**/*.prefab
- Assets/**/*.asset
- Packages/
- ProjectSettings/

## Read-only Context
- tdd.md
- Assets/Scripts/Shared/
- docs/agent/review/boundary-review-checklist.md
- docs/agent/review/forbidden-api-checklist.md

## Required Future Changes
Create `GameFlowController` and supporting Core state types or services for scene change requests, pause, resume, retry, main menu requests, game over, and stage clear routing.

## Explicit Non-goals
- Do not implement UI views.
- Do not implement StageController internals.
- Do not implement Player, Enemy, Items, or MapGimmicks.
- Do not create scenes.

## Boundary Rules
- Core owns scene transitions and game state flow.
- UI may call Core request methods later, but Core must not depend on concrete UI views.
- Enemy, Items, and MapGimmicks must not get scene-loading authority from this task.

## Contract Change Behavior
If Core needs a Shared contract change, stop and create a Contract Change Request unless the change is already allowed by TASK-002.

## Acceptance Criteria
- `GameFlowController` exists with clear public request entry points.
- Scene changes are routed only through Core.
- Pause, retry, main menu, game over, and stage clear entry points exist as skeleton behavior.
- Runtime code does not use `UnityEditor`.
- No forbidden find or message APIs are used.

## Definition of Done
- Changed files are limited to `Assets/Scripts/Core/`.
- Unity compile check passes.
- Boundary review confirms Core does not depend on UI or feature-module internals.

## Validation
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Core`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage" Assets/Scripts/Core`
- `rg "SceneManager\\.LoadScene" Assets/Scripts`
- Unity compile check
- EditMode tests for state transitions if practical

## Notes for Future Codex Agent
Keep this as a skeleton with stable entry points. Stop if you need concrete scene assets or UI implementation.

