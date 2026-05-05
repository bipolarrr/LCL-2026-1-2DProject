# TASK-017-save-stage-result-extension

## Status
Planned

## Owner Role
Core

## Goal
Create the local stage result persistence extension point for `StageClearResult` using a minimal PlayerPrefs or JSON strategy and no online leaderboard.

## Source Design Sections
- 3. Non-Goals
- 10. Stage System
- 20. Save / Stage Result / Leaderboard Extension Point

## Dependencies
- TASK-005

## Allowed Write Paths
- Assets/Scripts/Core/Results/
- Assets/Scripts/Core/Results/**/*.cs
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
- Assets/Scripts/Core/Stage/
- docs/agent/notes/design-conflicts.md

## Required Future Changes
Define or extend `StageClearResult` persistence through a small interface and one local implementation using either PlayerPrefs or JSON, with fields for stage id, clear time, death count, damage taken, collected item count, and cleared time.

## Explicit Non-goals
- Do not implement online leaderboard.
- Do not implement cloud save.
- Do not implement encryption.
- Do not implement UI result submission.

## Boundary Rules
- Persistence belongs to Core result flow.
- UI may later request or display result data but must not own persistence.
- Player and Enemy modules must not write result storage directly.

## Contract Change Behavior
If persistence needs Shared or UI changes, stop and create a Contract Change Request instead of editing outside allowed paths.

## Acceptance Criteria
- `StageClearResult` fields are represented.
- A local persistence extension point exists.
- One simple storage strategy is selected or isolated behind an interface.
- Runtime code does not use `UnityEditor`.

## Definition of Done
- Changed files are limited to Core stage/result paths.
- Unity compile check passes.
- Boundary review confirms no UI or feature module edits.
- Manual check confirms no online leaderboard scope.

## Validation
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Core/Results Assets/Scripts/Core/Stage`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage" Assets/Scripts/Core/Results Assets/Scripts/Core/Stage`
- Unity compile check
- EditMode tests for serialization or persistence adapter if practical

## Notes for Future Codex Agent
Choose the simplest local path that fits the existing Core code. Stop if UI submission or remote ranking enters scope.

