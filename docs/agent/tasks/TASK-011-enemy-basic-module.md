# TASK-011-enemy-basic-module

## Status
Planned

## Owner Role
Enemy

## Goal
Create the basic Enemy module skeleton for health, movement, attack, contact damage, damage receiving, death notification, and definition data without referencing Player internals.

## Source Design Sections
- 5.3 Enemy, Item, MapGimmick의 책임
- 13. Combat System
- 15. Enemy System
- 22. Animation Contract
- 23. Physics Layers and Tags
- 26. Forbidden Practices

## Dependencies
- TASK-002
- TASK-003
- TASK-005
- TASK-010

## Allowed Write Paths
- Assets/Scripts/Enemy/
- Assets/Scripts/Enemy/**/*.cs

## Forbidden Write Paths
- Assets/Scripts/Core/
- Assets/Scripts/Player/
- Assets/Scripts/Shared/
- Assets/Scripts/Items/
- Assets/Scripts/MapGimmicks/
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
- Assets/Scripts/Core/Stage/
- Assets/Scripts/Animation/
- docs/agent/review/boundary-review-checklist.md

## Required Future Changes
Create `EnemyController`, `EnemyHealth`, `EnemyMovement`, `EnemyAttack`, `EnemyContactDamage`, `EnemyDamageReceiver`, `EnemyDeathHandler`, and `EnemyDefinition` type without creating ScriptableObject assets.

## Explicit Non-goals
- Do not implement complex FSM AI.
- Do not implement Boss-specific behavior.
- Do not edit Player, Core, or Shared.
- Do not create enemy prefabs or assets.

## Boundary Rules
- Enemy must not reference `PlayerHealth`, `PlayerMotor`, `PlayerMovement`, or Player internals.
- Enemy contact damage must use `DamageRequest` and `IDamageReceiver`.
- Enemy death may notify Stage through approved Core/Stage entry points only.

## Contract Change Behavior
If Enemy needs edits in Core, Player, or Shared, stop and write a Contract Change Request instead of editing those paths.

## Acceptance Criteria
- Required Enemy classes exist and compile.
- Enemy receives damage through Shared contracts.
- Enemy sends Player damage through Shared contracts.
- Enemy does not call `SceneManager.LoadScene`.
- Runtime code does not use `UnityEditor`.

## Definition of Done
- Changed files are limited to `Assets/Scripts/Enemy/`.
- Unity compile check passes.
- Boundary review confirms no Player internal references.
- Manual or automated scan verifies forbidden APIs are absent.

## Validation
- `git diff --name-only`
- `rg "PlayerHealth|PlayerMotor|PlayerMovement|currentHp|jumpCount|isInvincible|isGuarding|isDead" Assets/Scripts/Enemy`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Enemy`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage|SceneManager\\.LoadScene" Assets/Scripts/Enemy`
- Unity compile check

## Notes for Future Codex Agent
Stay inside Enemy. If Stage or Shared entry points are missing, file a Contract Change Request and stop.

