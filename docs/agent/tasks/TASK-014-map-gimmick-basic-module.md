# TASK-014-map-gimmick-basic-module

## Status
Planned

## Owner Role
MapGimmicks

## Goal
Create the basic map gimmick module for `FallingTrap` and `CollapsePlatform` using Shared contracts and without mutating Player internals or StageController directly.

## Source Design Sections
- 5.3 Enemy, Item, MapGimmick의 책임
- 14. Damage / Heal / Buff Request Contracts
- 18. MapGimmick System
- 23. Physics Layers and Tags
- 26. Forbidden Practices

## Dependencies
- TASK-002
- TASK-003
- TASK-008

## Allowed Write Paths
- Assets/Scripts/MapGimmicks/
- Assets/Scripts/MapGimmicks/**/*.cs

## Forbidden Write Paths
- Assets/Scripts/Core/
- Assets/Scripts/Player/
- Assets/Scripts/Shared/
- Assets/Scripts/Enemy/
- Assets/Scripts/Items/
- Assets/Scripts/UI/
- Assets/Scripts/Editor/
- Assets/**/*.asset
- Assets/**/*.unity
- Assets/**/*.prefab
- Packages/
- ProjectSettings/

## Read-only Context
- tdd.md
- Assets/Scripts/Shared/
- Assets/Scripts/Player/
- docs/agent/review/boundary-review-checklist.md

## Required Future Changes
Create `FallingTrap` with delayed fall and damage request delivery, plus `CollapsePlatform` with delayed disable and optional restore behavior.

## Explicit Non-goals
- Do not edit Player receiver logic.
- Do not edit Shared contracts.
- Do not create prefabs or scenes.
- Do not implement complex level scripting.

## Boundary Rules
- MapGimmicks must not read or write Player health, Rigidbody2D, Transform, movement values, or state flags.
- Damage must be delivered with `DamageRequest` to `IDamageReceiver`.
- MapGimmicks must not directly manipulate StageController.

## Contract Change Behavior
If MapGimmicks need edits in Core, Player, or Shared, stop and write a Contract Change Request instead of editing those paths.

## Acceptance Criteria
- `FallingTrap` and `CollapsePlatform` classes exist and compile.
- FallingTrap uses Shared damage request contracts.
- CollapsePlatform disable and restore timing is locally owned.
- Runtime code does not use `UnityEditor`.

## Definition of Done
- Changed files are limited to `Assets/Scripts/MapGimmicks/`.
- Unity compile check passes.
- Boundary review confirms no Player internals or StageController direct manipulation.
- Manual or PlayMode smoke check covers trigger behavior if practical.

## Validation
- `git diff --name-only`
- `rg "PlayerHealth|PlayerMotor|PlayerMovement|currentHp|linearVelocity|\\.position\\s*=|isInvincible|isGuarding|isDead|StageController" Assets/Scripts/MapGimmicks`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/MapGimmicks`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage|SceneManager\\.LoadScene" Assets/Scripts/MapGimmicks`
- Unity compile check

## Notes for Future Codex Agent
Stay inside MapGimmicks. If collision behavior requires Player or Shared changes, file a Contract Change Request and stop.

