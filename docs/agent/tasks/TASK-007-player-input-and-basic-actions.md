# TASK-007-player-input-and-basic-actions

## Status
Planned

## Owner Role
Player

## Goal
Create Player input reading and basic movement action routing for move, jump, dash, glide, pause, cancel, and submit without mixing input polling into physics application.

## Source Design Sections
- 5.6 물리와 이동 처리
- 12. Player System
- 21. Input System
- 26. Forbidden Practices

## Dependencies
- TASK-006

## Allowed Write Paths
- Assets/Scripts/Player/Input/
- Assets/Scripts/Player/Input/**/*.cs
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
- Assets/**/*.inputactions
- Assets/**/*.asset
- Assets/**/*.unity
- Assets/**/*.prefab
- Packages/
- ProjectSettings/

## Read-only Context
- tdd.md
- Assets/InputSystem_Actions.inputactions
- Assets/Scripts/Player/
- Assets/Scripts/Core/

## Required Future Changes
Create `PlayerInputReader` and connect basic action intent to Player movement components, keeping `Update` input collection separate from `FixedUpdate` Rigidbody2D movement application.

## Explicit Non-goals
- Do not edit Input Actions assets.
- Do not implement combat selection.
- Do not implement UI views.
- Do not implement cutscene behavior.

## Boundary Rules
- Player input may request Core pause/cancel/submit through an approved Core entry point only if it already exists.
- Player input must not directly load scenes or mutate Core state.
- Dead state must ignore gameplay input.

## Contract Change Behavior
If input routing needs Core or Shared changes, stop and create a Contract Change Request instead of editing those modules.

## Acceptance Criteria
- Input reading is isolated in `PlayerInputReader` or equivalent.
- Physics application remains in Player movement or motor code.
- Move, jump, dash, glide, pause, cancel, and submit intents are represented.
- Runtime code does not use `UnityEditor`.

## Definition of Done
- Changed files are limited to Player paths.
- Unity compile check passes.
- Boundary review confirms no scene loading or UI mutation from Player input.

## Validation
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts/Player`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage" Assets/Scripts/Player`
- `rg "SceneManager\\.LoadScene" Assets/Scripts/Player`
- Unity compile check
- PlayMode smoke check for Update versus FixedUpdate behavior if practical

## Notes for Future Codex Agent
Keep this to input and basic actions. Stop if combat, UI, or cutscene scope starts expanding.

