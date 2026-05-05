# TASK-018-scene-builder-tools

## Status
Planned

## Owner Role
SceneBuilder

## Goal
Create editor-only SceneBuilder tools for Main Menu, Stage 01, Cutscene Test, and Build All Core Scenes after runtime objects exist.

## Source Design Sections
- 5.10 씬 구성과 에디터 코드
- 8. Scene Strategy
- 23. Physics Layers and Tags
- 24. SceneBuilder Tools
- 26. Forbidden Practices

## Dependencies
- TASK-004
- TASK-005
- TASK-006
- TASK-011
- TASK-013
- TASK-014
- TASK-015
- TASK-016
- TASK-017

## Allowed Write Paths
- Assets/Scripts/Editor/SceneBuilders/
- Assets/Scripts/Editor/SceneBuilders/**/*.cs
- Assets/Scenes/
- Assets/Scenes/**/*.unity

## Forbidden Write Paths
- Assets/Scripts/Core/
- Assets/Scripts/Player/
- Assets/Scripts/Shared/
- Assets/Scripts/Enemy/
- Assets/Scripts/Items/
- Assets/Scripts/MapGimmicks/
- Assets/Scripts/UI/
- Assets/**/*.prefab
- Assets/**/*.asset
- Packages/
- ProjectSettings/
- UserSettings/
- Library/
- Temp/

## Read-only Context
- tdd.md
- Assets/Scripts/Core/
- Assets/Scripts/Player/
- Assets/Scripts/Enemy/
- Assets/Scripts/Items/
- Assets/Scripts/MapGimmicks/
- Assets/Scripts/UI/
- Assets/Scripts/Shared/

## Required Future Changes
Create `MainMenuSceneBuilder`, `Stage01SceneBuilder`, `CutsceneTestSceneBuilder`, and a Build All Core Scenes menu item under `Assets/Scripts/Editor/SceneBuilders/` that creates core objects, cameras, canvas, EventSystem, player spawn, StageController, and required references.

## Explicit Non-goals
- Do not create art assets.
- Do not create prefabs.
- Do not implement runtime gameplay.
- Do not edit runtime module code.

## Boundary Rules
- SceneBuilder code must be editor-only.
- Runtime code must not use `UnityEditor`.
- SceneBuilder may wire existing public references but must not require runtime forbidden APIs.

## Contract Change Behavior
If SceneBuilder needs runtime API changes, stop and create a Contract Change Request instead of editing runtime modules.

## Acceptance Criteria
- SceneBuilder scripts live under an `Editor` folder.
- Menu items exist for Main Menu, Stage 01, Cutscene Test, and Build All Core Scenes.
- Generated scenes compile and open in Unity.
- Runtime code remains free of `UnityEditor`.

## Definition of Done
- Changed files are limited to SceneBuilder editor scripts and generated scenes.
- Unity Editor validation is performed explicitly.
- Boundary review confirms no runtime module edits.
- SceneBuilder-generated scene references are documented.

## Validation
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts`
- Confirm UnityEditor usage appears only under `Assets/Scripts/Editor/`.
- Unity Editor manual run: Tools/Game/Build Main Menu Scene
- Unity Editor manual run: Tools/Game/Build Stage 01 Scene
- Unity Editor manual run: Tools/Game/Build Cutscene Test Scene
- Unity compile check

## Notes for Future Codex Agent
This task requires Unity Editor validation. Stop if runtime code must change to make scene creation possible.

