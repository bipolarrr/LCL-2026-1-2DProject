# TASK-008-player-damage-heal-buff-receivers

## Status
Planned

## Owner Role
Player

## Goal
Implement Player-side receivers for damage, heal, buff, bounce, and knockback requests so Player decides whether and how requests affect its owned state.

## Source Design Sections
- 5.2 Player의 책임
- 5.3 Enemy, Item, MapGimmick의 책임
- 5.4 요청 메서드와 구조체 사용 규칙
- 12. Player System
- 13. Combat System
- 14. Damage / Heal / Buff Request Contracts

## Dependencies
- TASK-006
- TASK-007

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
- Assets/Scripts/Player/
- docs/agent/notes/design-conflicts.md

## Required Future Changes
Create Player implementations of `IDamageReceiver`, `IHealReceiver`, `IBuffReceiver`, and the canonical bounce or knockback entry points, including invincibility, guard handling, death handling, hit reaction hooks, and buff application.

## Explicit Non-goals
- Do not implement Player attacks.
- Do not implement Enemy, Items, or MapGimmicks.
- Do not change Shared contracts unless approved.
- Do not create prefabs or scenes.

## Boundary Rules
- Player owns whether requests are accepted, reduced, ignored, or applied.
- Receiver methods must not expose mutable Player internals.
- Knockback and bounce must be applied through Player-owned movement or motor components.

## Contract Change Behavior
If the receiver implementation needs new fields or interfaces in Shared, stop and create a Contract Change Request instead of editing Shared.

## Acceptance Criteria
- Player implements the canonical receiver interfaces.
- Invincible, guarding, dead, and normal states produce distinct request outcomes.
- Heal and buff application respects Player-owned state limits.
- Runtime code does not use `UnityEditor`.

## Definition of Done
- Changed files are limited to Player paths.
- Unity compile check passes.
- Boundary review confirms no external module mutation of Player internals is introduced.
- Focused tests or documented manual checks cover receiver state behavior.

## Validation
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Player`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage" Assets/Scripts/Player`
- Unity compile check
- EditMode tests for receiver behavior if practical

## Notes for Future Codex Agent
Implement receiver behavior only on the Player side. Stop if you need Enemy, Item, or MapGimmick callers.

