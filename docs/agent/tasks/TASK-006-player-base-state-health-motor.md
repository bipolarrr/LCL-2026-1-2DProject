# TASK-006-player-base-state-health-motor

## Status
Planned

## Owner Role
Player

## Goal
Create the Player base skeleton for owned state, health, motor, and movement without full combat or cross-module receiver implementation.

## Source Design Sections
- 5.2 Player의 책임
- 5.6 물리와 이동 처리
- 5.7 PlayerMovement와 PlayerMotor의 역할
- 5.8 컴포넌트 분리 기준
- 12. Player System
- 21. Input System
- 26. Forbidden Practices

## Dependencies
- TASK-002
- TASK-003

## Allowed Write Paths
- Assets/Scripts/Player/
- Assets/Scripts/Player/**/*.cs

## Forbidden Write Paths
- Assets/Scripts/Core/
- Assets/Scripts/Shared/
- Assets/Scripts/Enemy/
- Assets/Scripts/Items/
- Assets/Scripts/MapGimmicks/
- Assets/Scripts/UI/
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

## Required Future Changes
Create `PlayerController`, `PlayerStateController` or equivalent, `PlayerHealth`, `PlayerMotor`, and `PlayerMovement` with Player-owned state and physics application points.

## Explicit Non-goals
- Do not implement full combat.
- Do not implement damage, heal, or buff receiver interfaces yet.
- Do not implement input reader.
- Do not create prefabs or scenes.

## Boundary Rules
- Player owns health, movement, physics, invincibility, guarding, death, and hit reactions.
- `PlayerMotor` or an equivalent Player-owned component is the only class that directly applies Player Rigidbody2D velocity.
- External modules must not be referenced.

## Contract Change Behavior
If Player base state needs changes to Shared contracts, stop and create a Contract Change Request instead of editing Shared.

## Acceptance Criteria
- Player base components exist and compile.
- Player state and health fields are private or otherwise protected from external mutation.
- Rigidbody2D writes are isolated to Player-owned motor code.
- Runtime code does not use `UnityEditor`.
- No forbidden find or message APIs are used.

## Definition of Done
- Changed files are limited to `Assets/Scripts/Player/`.
- Unity compile check passes.
- Boundary review confirms no feature module dependencies.

## Validation
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Player`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage" Assets/Scripts/Player`
- `rg "public .*currentHp|public .*speed|public .*jumpCount" Assets/Scripts/Player`
- Unity compile check
- EditMode tests for pure state or health rules if practical

## Notes for Future Codex Agent
Keep combat out of this task. Stop if adding receivers, projectiles, or attack selection becomes necessary.

