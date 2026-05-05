# TASK-002-shared-request-contracts

## Status
Planned

## Owner Role
Lead

## Goal
Create the Shared request structs, interfaces, enums, and minimal game-flow request contracts used for safe cross-module gameplay interaction.

## Source Design Sections
- 5.4 요청 메서드와 구조체 사용 규칙
- 5.5 요청 구조체의 범위
- 5.9 모듈 간 경계
- 9. Game Flow
- 10. Stage System
- 13. Combat System
- 14. Damage / Heal / Buff Request Contracts
- 20. Save / Stage Result / Leaderboard Extension Point

## Dependencies
- TASK-001

## Allowed Write Paths
- Assets/Scripts/Shared/
- Assets/Scripts/Shared/**/*.cs

## Forbidden Write Paths
- Assets/Scripts/Core/
- Assets/Scripts/Player/
- Assets/Scripts/Enemy/
- Assets/Scripts/Items/
- Assets/Scripts/MapGimmicks/
- Assets/Scripts/UI/
- Assets/Scripts/Editor/
- Packages/
- ProjectSettings/
- docs/design/tdd.md

## Read-only Context
- tdd.md
- docs/agent/notes/design-conflicts.md
- docs/agent/review/boundary-review-checklist.md
- Assets/Scripts/

## Required Future Changes
Create `DamageRequest`, `HealRequest`, `BuffRequest`, `BounceRequest` or `KnockbackRequest` as decided by TASK-001, `DamageKind`, `BuffKind`, `IDamageReceiver`, `IHealReceiver`, `IBuffReceiver`, and minimal stage/game-flow request or event data needed by Core and Stage systems.

## Explicit Non-goals
- Do not implement gameplay behavior.
- Do not create Player, Enemy, Item, MapGimmick, UI, or Core controllers.
- Do not create ScriptableObjects or Unity assets.

## Boundary Rules
- Shared must not depend on concrete feature modules.
- Shared contracts may reference Unity runtime types only when necessary and must not use `UnityEditor`.
- Shared must avoid owning gameplay state.

## Contract Change Behavior
If the Shared contract needs Core or Player behavior to compile, stop and create a Contract Change Request instead of editing those modules.

## Acceptance Criteria
- Required request structs and receiver interfaces exist.
- Damage request fields match the canonical decision.
- Enums cover initial damage and buff kinds from the TDD.
- Runtime code does not use `UnityEditor`.
- Contracts are simple and do not mutate target state.

## Definition of Done
- Changed files are limited to `Assets/Scripts/Shared/`.
- Unity compile check passes.
- Forbidden API scan passes for Shared runtime code.
- Boundary review confirms no feature-module dependency.

## Validation
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Shared`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage" Assets/Scripts/Shared`
- Unity compile check
- EditMode tests if pure contract tests are added

## Notes for Future Codex Agent
Keep Shared boring. If a contract starts accumulating logic, stop and move that behavior to the owning module.

