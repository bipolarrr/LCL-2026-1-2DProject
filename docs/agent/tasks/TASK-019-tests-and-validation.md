# TASK-019-tests-and-validation

## Status
Planned

## Owner Role
QA / Review

## Goal
Create focused validation coverage and review commands for compile checks, boundary validation, forbidden API scans, Runtime `UnityEditor` scans, and Player direct-reference scans.

## Source Design Sections
- 4. Unity Version and Packages
- 5.12 AI 코드 에이전트 작성 규칙
- 25. Coding Style
- 26. Forbidden Practices
- 29. Acceptance Criteria
- 30. Definition of Done

## Dependencies
- TASK-003

## Allowed Write Paths
- Assets/Tests/
- Assets/Tests/**/*.cs
- docs/agent/review/
- scripts/

## Forbidden Write Paths
- Assets/Scripts/Core/
- Assets/Scripts/Player/
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
- Assets/Scripts/
- docs/agent/review/boundary-review-checklist.md
- docs/agent/review/forbidden-api-checklist.md

## Required Future Changes
Add EditMode and PlayMode test plans or tests where appropriate, plus validation scripts or documented commands for compile validation, boundary validation, forbidden API scan, Runtime `UnityEditor` scan, and Player direct-reference scan.

## Explicit Non-goals
- Do not change runtime gameplay code.
- Do not create scenes or prefabs.
- Do not weaken boundary rules to make tests pass.

## Boundary Rules
- Tests may reference public contracts and public entry points only.
- Tests must not require non-lead modules to expose internals.
- Validation scripts must flag, not rewrite, violations.

## Contract Change Behavior
If tests reveal a missing contract, stop and create a Contract Change Request instead of editing runtime modules.

## Acceptance Criteria
- Validation commands are documented and runnable or have clear manual fallback.
- Runtime `UnityEditor` scan is included.
- Forbidden API scan is included.
- Player direct-reference scan is included.
- Unity compile and test paths are documented.

## Definition of Done
- Changed files are limited to tests, review docs, and scripts.
- Existing implementation files are not modified.
- Boundary review checklist and forbidden API checklist are up to date.
- Test or validation limitations are documented.

## Validation
- `git diff --name-only`
- Run boundary validation script if available.
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage" Assets/Scripts`
- `rg "PlayerHealth|PlayerMotor|PlayerMovement|currentHp|jumpCount|isInvincible|isGuarding|isDead" Assets/Scripts/Enemy Assets/Scripts/Items Assets/Scripts/MapGimmicks Assets/Scripts/UI`
- Unity compile check
- EditMode tests
- PlayMode tests

## Notes for Future Codex Agent
This is a QA task. Do not fix runtime issues inside this ticket unless the allowed paths explicitly permit it.

