# TASK-009-player-combat-and-projectile

## Status
Planned

## Owner Role
Player

## Goal
Create Player combat selection, melee target detection, ranged attack, projectile behavior, and outgoing `DamageRequest` generation through Shared contracts.

## Source Design Sections
- 12. Player System
- 13. Combat System
- 14. Damage / Heal / Buff Request Contracts
- 21. Input System
- 22. Animation Contract
- 23. Physics Layers and Tags

## Dependencies
- TASK-008
- TASK-010

## Allowed Write Paths
- Assets/Scripts/Player/Combat/
- Assets/Scripts/Player/Combat/**/*.cs
- Assets/Scripts/Projectiles/
- Assets/Scripts/Projectiles/**/*.cs
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
- Assets/Scripts/Animation/

## Required Future Changes
Create `PlayerAttackController`, `MeleeTargetDetector`, `PlayerMeleeAttack`, `PlayerRangedAttack`, and `PlayerProjectile` or equivalent classes that choose melee or ranged attack and send `DamageRequest` to `IDamageReceiver`.

## Explicit Non-goals
- Do not implement Enemy health.
- Do not create projectile prefabs.
- Do not create animation assets.
- Do not edit Shared contracts.

## Boundary Rules
- Player combat must target Shared receiver interfaces, not concrete Enemy classes.
- Projectiles must not mutate Player or Enemy internals.
- Attack logic must not depend on animation events to apply damage.

## Contract Change Behavior
If combat requires Shared contract changes, stop and create a Contract Change Request instead of editing Shared.

## Acceptance Criteria
- Attack input can be routed to melee or ranged logic.
- Melee and projectile attacks generate `DamageRequest`.
- Targets are discovered through collider/component references without forbidden find APIs.
- Runtime code does not use `UnityEditor`.

## Definition of Done
- Changed files are limited to Player combat and Projectiles paths.
- Unity compile check passes.
- Boundary review confirms outgoing damage uses Shared interfaces only.
- Manual or automated smoke check verifies attacks do not require Animator events.

## Validation
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Player Assets/Scripts/Projectiles`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage" Assets/Scripts/Player Assets/Scripts/Projectiles`
- Unity compile check
- PlayMode smoke check for melee and projectile request generation if practical

## Notes for Future Codex Agent
Keep combat request-based. Stop if you need to edit Enemy internals or create Unity assets.

