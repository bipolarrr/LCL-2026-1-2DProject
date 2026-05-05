# TASK-013-item-basic-module

## Status
Planned

## Owner Role
Items

## Goal
Create the basic item module for heal, move speed buff, and extra jump buff pickups that emit Shared requests and never mutate Player internals.

## Source Design Sections
- 5.3 Enemy, Item, MapGimmick의 책임
- 14. Damage / Heal / Buff Request Contracts
- 17. Item System
- 23. Physics Layers and Tags
- 26. Forbidden Practices

## Dependencies
- TASK-002
- TASK-003
- TASK-008

## Allowed Write Paths
- Assets/Scripts/Items/
- Assets/Scripts/Items/**/*.cs

## Forbidden Write Paths
- Assets/Scripts/Core/
- Assets/Scripts/Player/
- Assets/Scripts/Shared/
- Assets/Scripts/Enemy/
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
- Assets/Scripts/Player/
- docs/agent/review/boundary-review-checklist.md

## Required Future Changes
Create `HealItem`, `MoveSpeedBuffItem`, and `ExtraJumpBuffItem` that detect eligible receivers, create `HealRequest` or `BuffRequest`, and deactivate or remove themselves after successful collection.

## Explicit Non-goals
- Do not edit Player receiver logic.
- Do not edit Shared contracts.
- Do not create item prefabs or assets.
- Do not implement inventory.

## Boundary Rules
- Items must not read or write Player health, speed, jump count, Rigidbody2D, Transform, or state flags.
- Items must interact through `IHealReceiver`, `IBuffReceiver`, or canonical Shared contracts.
- Items must not call scene loading APIs.

## Contract Change Behavior
If Items need edits in Core, Player, or Shared, stop and write a Contract Change Request instead of editing those paths.

## Acceptance Criteria
- Three initial item classes exist and compile.
- Items use Shared request structs and receiver interfaces.
- Items deactivate or remove themselves after collection according to a documented rule.
- Runtime code does not use `UnityEditor`.

## Definition of Done
- Changed files are limited to `Assets/Scripts/Items/`.
- Unity compile check passes.
- Boundary review confirms no Player internals are referenced.
- Manual or PlayMode check covers trigger-based collection if practical.

## Validation
- `git diff --name-only`
- `rg "PlayerHealth|PlayerMotor|PlayerMovement|currentHp|speed|jumpCount|linearVelocity|isInvincible|isGuarding|isDead" Assets/Scripts/Items`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Items`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage|SceneManager\\.LoadScene" Assets/Scripts/Items`
- Unity compile check

## Notes for Future Codex Agent
Stay in Items. If a receiver method is missing, file a Contract Change Request instead of editing Player.

