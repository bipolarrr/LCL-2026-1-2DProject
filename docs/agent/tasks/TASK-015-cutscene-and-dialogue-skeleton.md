# TASK-015-cutscene-and-dialogue-skeleton

## Status
Planned

## Owner Role
Core

## Goal
Create the reusable cutscene and dialogue skeleton with submit/skip flow and GameFlow completion notification without complex branching.

## Source Design Sections
- 2. Goals
- 3. Non-Goals
- 9. Game Flow
- 11. Cutscene System
- 19. UI System
- 21. Input System

## Dependencies
- TASK-004
- TASK-007

## Allowed Write Paths
- Assets/Scripts/Core/Cutscenes/
- Assets/Scripts/Core/Cutscenes/**/*.cs

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
- Assets/Scripts/Player/Input/

## Required Future Changes
Create `CutsceneDefinition`, `CutsceneStep`, `CutsceneController`, and a dialogue view model or controller-facing data shape that supports submit, cancel, skip, and GameFlow completion notification.

## Explicit Non-goals
- Do not implement complex branching.
- Do not implement Timeline or Cinemachine.
- Do not create cutscene assets.
- Do not implement UI views.

## Boundary Rules
- Cutscene completion must notify Core game flow through approved Core entry points.
- Cutscene input must not directly mutate Player internals.
- Runtime code must not use editor APIs.

## Contract Change Behavior
If cutscene flow needs Shared or UI contract changes, stop and create a Contract Change Request instead of editing outside allowed paths.

## Acceptance Criteria
- Cutscene and dialogue data skeletons exist and compile.
- Submit and skip flow entry points exist.
- Completion can route back to GameFlowController.
- Runtime code does not use `UnityEditor`.

## Definition of Done
- Changed files are limited to `Assets/Scripts/Core/Cutscenes/`.
- Unity compile check passes.
- Boundary review confirms no UI or Player internals are edited.
- Manual check confirms no external package dependency is introduced.

## Validation
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Core/Cutscenes`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage" Assets/Scripts/Core/Cutscenes`
- Unity compile check
- EditMode tests for step progression if practical

## Notes for Future Codex Agent
Keep this data and flow oriented. Stop if you need real cutscene content or UI layout.

