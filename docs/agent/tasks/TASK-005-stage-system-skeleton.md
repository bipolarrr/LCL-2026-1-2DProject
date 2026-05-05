# TASK-005-stage-system-skeleton

## Status
Planned

## Owner Role
Core

## Goal
Create the stage-system skeleton for stage definitions, stage type, clear-condition routing, and `StageClearResult` integration points.

## Source Design Sections
- 8. Scene Strategy
- 9. Game Flow
- 10. Stage System
- 16. Boss System
- 20. Save / Stage Result / Leaderboard Extension Point

## Dependencies
- TASK-002
- TASK-003
- TASK-004

## Allowed Write Paths
- Assets/Scripts/Core/Stage/
- Assets/Scripts/Core/Stage/**/*.cs

## Forbidden Write Paths
- Assets/Scripts/Player/
- Assets/Scripts/Enemy/
- Assets/Scripts/Items/
- Assets/Scripts/MapGimmicks/
- Assets/Scripts/UI/
- Assets/Scripts/Shared/
- Assets/Scripts/Editor/
- Assets/**/*.asset
- Assets/**/*.unity
- Assets/**/*.prefab
- Packages/
- ProjectSettings/

## Read-only Context
- tdd.md
- Assets/Scripts/Core/
- Assets/Scripts/Shared/
- docs/agent/review/boundary-review-checklist.md

## Required Future Changes
Create `StageDefinition`, `StageType`, `StageController`, stage clear request flow, and `StageClearResult` integration points without creating ScriptableObject assets.

## Explicit Non-goals
- Do not create stage assets or scenes.
- Do not implement Enemy or Boss behavior.
- Do not implement result UI or persistence.

## Boundary Rules
- Stage progression is owned by Core.
- Enemy and Boss may notify StageController through approved events or request methods, but StageController must not depend on Enemy internals.
- MapGimmicks must not directly manipulate StageController.

## Contract Change Behavior
If Stage needs new Shared event data, stop and create a Contract Change Request unless already covered by TASK-002.

## Acceptance Criteria
- Stage skeleton types exist and compile.
- Stage clear flow can represent normal, mid-boss, and final-boss stage types.
- `StageClearResult` data can be produced or passed through.
- Runtime code does not use `UnityEditor`.

## Definition of Done
- Changed files are limited to `Assets/Scripts/Core/Stage/`.
- Unity compile check passes.
- Boundary review confirms no feature-module internals are referenced.

## Validation
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Core/Stage`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage" Assets/Scripts/Core/Stage`
- Unity compile check
- EditMode tests for clear-condition routing if practical

## Notes for Future Codex Agent
Make this a narrow Core skeleton. Do not create real level content or boss AI.

