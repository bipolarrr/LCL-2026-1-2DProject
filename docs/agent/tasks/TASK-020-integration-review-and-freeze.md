# TASK-020-integration-review-and-freeze

## Status
Planned

## Owner Role
QA / Review

## Goal
Perform the final integration review and freeze decision by verifying module boundaries, request contracts, scene flow, UI flow, animation safety, and Unity compile health.

## Source Design Sections
- 1. Overview
- 2. Goals
- 5. Architecture Principles
- 8. Scene Strategy
- 9. Game Flow
- 10. Stage System
- 19. UI System
- 22. Animation Contract
- 24. SceneBuilder Tools
- 26. Forbidden Practices
- 29. Acceptance Criteria
- 30. Definition of Done

## Dependencies
- TASK-000
- TASK-001
- TASK-002
- TASK-003
- TASK-004
- TASK-005
- TASK-006
- TASK-007
- TASK-008
- TASK-009
- TASK-010
- TASK-011
- TASK-012
- TASK-013
- TASK-014
- TASK-015
- TASK-016
- TASK-017
- TASK-018
- TASK-019

## Allowed Write Paths
- docs/agent/review/
- docs/agent/notes/

## Forbidden Write Paths
- Assets/
- Packages/
- ProjectSettings/
- UserSettings/
- Library/
- Temp/
- .github/
- scripts/
- AGENTS.md
- CODEOWNERS
- docs/design/tdd.md

## Read-only Context
- tdd.md
- Assets/
- Packages/manifest.json
- docs/agent/
- AGENTS.md
- CODEOWNERS
- scripts/

## Required Future Changes
Write an integration review report that records pass/fail status for boundaries, request contracts, scene flow, UI flow, animation safety, compile status, test status, and remaining release blockers.

## Explicit Non-goals
- Do not fix implementation issues in this task.
- Do not edit runtime code.
- Do not create Unity assets.
- Do not change governance files.

## Boundary Rules
- Review only; do not mutate module code.
- Any discovered implementation issue must become a follow-up ticket or Contract Change Request.
- Freeze only if boundary and compile validation pass.

## Contract Change Behavior
If a contract problem is found, write a Contract Change Request and mark freeze blocked instead of editing Core, Player, Shared, or feature modules.

## Acceptance Criteria
- Integration review report exists.
- Boundary review checklist is completed.
- Forbidden API checklist is completed.
- Unity compile status is recorded.
- SceneBuilder validation status is recorded.
- Freeze decision is explicit: pass, blocked, or conditional.

## Definition of Done
- Changed files are limited to `docs/agent/review/` and `docs/agent/notes/`.
- All validation results and limitations are recorded.
- Follow-up tickets are listed for any blockers.
- No runtime implementation files are changed.

## Validation
- `git diff --name-only`
- Confirm changed files are only under `docs/agent/review/` and `docs/agent/notes/`.
- Run boundary validation script if available.
- Run forbidden API scans.
- Unity compile check.
- EditMode tests.
- PlayMode tests.
- Manual SceneBuilder scene creation checks.

## Notes for Future Codex Agent
This is a review and freeze task. Do not repair defects here; make them visible and route them to the right owner.

