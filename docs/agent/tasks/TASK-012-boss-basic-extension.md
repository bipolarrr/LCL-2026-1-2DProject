# TASK-012-boss-basic-extension

## Status
Planned

## Owner Role
Enemy

## Goal
Add the minimal Boss extension as a special Enemy that can notify stage clear on defeat without implementing complex phase AI.

## Source Design Sections
- 10. Stage System
- 16. Boss System
- 22. Animation Contract
- 26. Forbidden Practices

## Dependencies
- TASK-005
- TASK-010
- TASK-011

## Allowed Write Paths
- Assets/Scripts/Enemy/Boss/
- Assets/Scripts/Enemy/Boss/**/*.cs
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
- Assets/Scripts/Enemy/
- Assets/Scripts/Core/Stage/
- Assets/Scripts/Shared/
- Assets/Scripts/Animation/

## Required Future Changes
Create `BossController` or `BossHealth` only if needed, plus a Boss defeated event or request path that integrates with existing Stage clear flow through approved contracts.

## Explicit Non-goals
- Do not implement complex phase transitions.
- Do not implement multiple boss encounters.
- Do not edit Core, Player, or Shared.
- Do not create boss prefabs or assets.

## Boundary Rules
- Boss should extend or compose Enemy behavior rather than duplicating unrelated systems.
- Boss defeat notification must use approved Stage/Core entry points.
- Boss logic must not depend on Animator state for defeat or stage clear.

## Contract Change Behavior
If Boss needs edits in Core, Player, or Shared, stop and write a Contract Change Request instead of editing those paths.

## Acceptance Criteria
- Boss can be represented as a special Enemy.
- Boss defeat can trigger an approved stage clear notification path.
- No complex phase AI is introduced.
- Runtime code does not use `UnityEditor`.

## Definition of Done
- Changed files are limited to Enemy/Boss paths.
- Unity compile check passes.
- Boundary review confirms no Core, Player, or Shared edits.
- Manual check verifies boss clear is not Animator-dependent.

## Validation
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Enemy`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage|SceneManager\\.LoadScene" Assets/Scripts/Enemy`
- Unity compile check
- PlayMode smoke check for boss defeated event if practical

## Notes for Future Codex Agent
Keep Boss minimal. Stop if the work turns into AI design or stage system changes.

