# Agent Task Index

## Ordered Task List
1. TASK-000-codex-repo-setup-planning
2. TASK-001-canonical-contract-resolution
3. TASK-002-shared-request-contracts
4. TASK-003-assembly-and-boundary-enforcement
5. TASK-004-core-game-flow-skeleton
6. TASK-005-stage-system-skeleton
7. TASK-006-player-base-state-health-motor
8. TASK-007-player-input-and-basic-actions
9. TASK-008-player-damage-heal-buff-receivers
10. TASK-009-player-combat-and-projectile
11. TASK-010-animation-safety-layer
12. TASK-011-enemy-basic-module
13. TASK-012-boss-basic-extension
14. TASK-013-item-basic-module
15. TASK-014-map-gimmick-basic-module
16. TASK-015-cutscene-and-dialogue-skeleton
17. TASK-016-ui-basic-views
18. TASK-017-save-stage-result-extension
19. TASK-018-scene-builder-tools
20. TASK-019-tests-and-validation
21. TASK-020-integration-review-and-freeze

## Dependency Graph
- TASK-000: no implementation dependencies.
- TASK-001: depends on TASK-000.
- TASK-002: depends on TASK-001.
- TASK-003: depends on TASK-000 and TASK-002.
- TASK-004: depends on TASK-002 and TASK-003.
- TASK-005: depends on TASK-002, TASK-003, and TASK-004.
- TASK-006: depends on TASK-002 and TASK-003.
- TASK-007: depends on TASK-006.
- TASK-008: depends on TASK-006 and TASK-007.
- TASK-009: depends on TASK-008 and TASK-010.
- TASK-010: depends on TASK-002 and TASK-003.
- TASK-011: depends on TASK-002, TASK-003, TASK-005, and TASK-010.
- TASK-012: depends on TASK-005, TASK-010, and TASK-011.
- TASK-013: depends on TASK-002, TASK-003, and TASK-008.
- TASK-014: depends on TASK-002, TASK-003, and TASK-008.
- TASK-015: depends on TASK-004 and TASK-007.
- TASK-016: depends on TASK-004, TASK-005, TASK-008, and TASK-015.
- TASK-017: depends on TASK-005.
- TASK-018: depends on TASK-004, TASK-005, TASK-006, TASK-011, TASK-013, TASK-014, TASK-015, TASK-016, and TASK-017.
- TASK-019: depends on TASK-003 and may expand after any implementation task.
- TASK-020: depends on all prior tasks.

## Lead-only Tasks
- TASK-000-codex-repo-setup-planning
- TASK-001-canonical-contract-resolution
- TASK-002-shared-request-contracts
- TASK-003-assembly-and-boundary-enforcement
- TASK-004-core-game-flow-skeleton
- TASK-005-stage-system-skeleton
- TASK-006-player-base-state-health-motor
- TASK-008-player-damage-heal-buff-receivers
- TASK-017-save-stage-result-extension
- TASK-020-integration-review-and-freeze

## Parallelization Guidance
- After TASK-003 completes, TASK-004, TASK-006, and TASK-010 may proceed in separate worktrees if their write paths remain separate.
- After TASK-008 completes, TASK-013 and TASK-014 may proceed in parallel.
- After TASK-011 completes, TASK-012 may proceed while TASK-013 and TASK-014 continue, provided no shared files are edited.
- TASK-015 may proceed after Core and input entry points exist; it should not overlap with TASK-016 if both touch Cutscene UI contracts.
- TASK-019 can add validation coverage incrementally after each implementation task, but final validation waits for all systems.

## Tasks That Must Not Be Parallelized
- TASK-001, TASK-002, and TASK-003 must run sequentially because they define the contract and dependency rules used by all later modules.
- TASK-006, TASK-007, TASK-008, and TASK-009 must run in order because Player state, input, receivers, and combat build on each other.
- TASK-018 must wait for the runtime objects it wires.
- TASK-020 must be last.

## Suggested Git Branch Names
- task/000-codex-repo-setup-planning
- task/001-canonical-contract-resolution
- task/002-shared-request-contracts
- task/003-assembly-and-boundary-enforcement
- task/004-core-game-flow-skeleton
- task/005-stage-system-skeleton
- task/006-player-base-state-health-motor
- task/007-player-input-and-basic-actions
- task/008-player-damage-heal-buff-receivers
- task/009-player-combat-and-projectile
- task/010-animation-safety-layer
- task/011-enemy-basic-module
- task/012-boss-basic-extension
- task/013-item-basic-module
- task/014-map-gimmick-basic-module
- task/015-cutscene-and-dialogue-skeleton
- task/016-ui-basic-views
- task/017-save-stage-result-extension
- task/018-scene-builder-tools
- task/019-tests-and-validation
- task/020-integration-review-and-freeze

## Suggested Codex Thread Names
- TASK-000 Repo Rules Planning
- TASK-001 Canonical Contract Resolution
- TASK-002 Shared Request Contracts
- TASK-003 Assembly Boundary Enforcement
- TASK-004 Core Game Flow Skeleton
- TASK-005 Stage System Skeleton
- TASK-006 Player Base State Health Motor
- TASK-007 Player Input Basic Actions
- TASK-008 Player Receivers Damage Heal Buff
- TASK-009 Player Combat Projectile
- TASK-010 Animation Safety Layer
- TASK-011 Enemy Basic Module
- TASK-012 Boss Basic Extension
- TASK-013 Item Basic Module
- TASK-014 Map Gimmick Basic Module
- TASK-015 Cutscene Dialogue Skeleton
- TASK-016 UI Basic Views
- TASK-017 Save Stage Result Extension
- TASK-018 Scene Builder Tools
- TASK-019 Tests Validation
- TASK-020 Integration Review Freeze

## Suggested Validation Priority
1. File boundary check with `git diff --name-only`.
2. Unity compile check.
3. Runtime scan for `using UnityEditor`.
4. Forbidden API scan for `GameObject.Find`, `FindObjectOfType`, and `SendMessage`.
5. Cross-module scan for forbidden Player internals.
6. EditMode tests for pure contracts and state transitions.
7. PlayMode smoke tests for scene flow and request routing.
8. Manual Unity Editor validation for SceneBuilder-generated scenes.
