# TASK-010-animation-safety-layer

## Status
Planned

## Owner Role
Lead

## Goal
Create the animation safety layer, animator hash classes, and adapter tickets so runtime logic can run even when Animator components, controllers, or parameters are missing.

## Source Design Sections
- 22. Animation Contract
- 25. Coding Style
- 26. Forbidden Practices

## Dependencies
- TASK-002
- TASK-003

## Allowed Write Paths
- Assets/Scripts/Animation/
- Assets/Scripts/Animation/**/*.cs
- Assets/Scripts/Player/Animation/
- Assets/Scripts/Enemy/Animation/
- Assets/Scripts/Boss/Animation/

## Forbidden Write Paths
- Assets/Scripts/Core/
- Assets/Scripts/Shared/
- Assets/Scripts/Items/
- Assets/Scripts/MapGimmicks/
- Assets/Scripts/UI/
- Assets/Scripts/Editor/
- Assets/**/*.controller
- Assets/**/*.anim
- Assets/**/*.asset
- Assets/**/*.unity
- Assets/**/*.prefab
- Packages/
- ProjectSettings/

## Read-only Context
- tdd.md
- Assets/Scripts/Player/
- Assets/Scripts/Enemy/
- docs/agent/review/forbidden-api-checklist.md

## Required Future Changes
Create `SafeAnimator`, common animator hash classes, and minimal Player, Enemy, and Boss animation adapter skeletons or tickets that safely mirror state without owning gameplay logic.

## Explicit Non-goals
- Do not create Animator Controllers or animation clips.
- Do not make gameplay depend on animations.
- Do not implement complex state machines.

## Boundary Rules
- Animation is an output layer only.
- Missing Animator, missing controller, and missing parameters must not break runtime logic.
- Animation adapters must not change gameplay state.

## Contract Change Behavior
If animation adapters need new gameplay events, stop and create a Contract Change Request instead of editing Core, Player, Enemy, or Shared.

## Acceptance Criteria
- `SafeAnimator` no-ops safely for null or incomplete Animator setup.
- Hash classes avoid string parameter calls at runtime call sites.
- Adapter skeletons do not drive gameplay decisions.
- Runtime code does not use `UnityEditor`.

## Definition of Done
- Changed files are limited to animation paths.
- Unity compile check passes.
- Boundary review confirms animation cannot block movement, damage, death, or stage clear logic.

## Validation
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Animation Assets/Scripts/Player/Animation Assets/Scripts/Enemy/Animation Assets/Scripts/Boss/Animation`
- `rg "SetFloat\\(\"|SetBool\\(\"|SetTrigger\\(\"|SetInteger\\(\"" Assets/Scripts`
- Unity compile check
- EditMode tests for `SafeAnimator` no-op behavior if practical

## Notes for Future Codex Agent
Keep animation defensive. Stop if the implementation starts requiring assets or Animator Controller changes.

