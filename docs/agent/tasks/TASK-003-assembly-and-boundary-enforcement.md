# TASK-003-assembly-and-boundary-enforcement

## Status
Planned

## Owner Role
Lead

## Goal
Set up future assembly dependency direction and automated boundary checks so non-lead modules cannot reference Player internals or use forbidden APIs.

## Source Design Sections
- 5.9 모듈 간 경계
- 6. Folder Structure
- 7. Ownership and Contributor Boundaries
- 25. Coding Style
- 26. Forbidden Practices

## Dependencies
- TASK-000
- TASK-002

## Allowed Write Paths
- Assets/Scripts/**/*.asmdef
- scripts/
- docs/agent/review/
- AGENTS.md
- Assets/Scripts/*/AGENTS.md

## Forbidden Write Paths
- Assets/Scripts/**/*.cs
- Assets/**/*.unity
- Assets/**/*.prefab
- Assets/**/*.asset
- Packages/
- ProjectSettings/
- docs/design/tdd.md

## Read-only Context
- tdd.md
- Assets/Scripts/
- docs/agent/review/boundary-review-checklist.md
- docs/agent/review/forbidden-api-checklist.md

## Required Future Changes
Create or update asmdef files to express allowed dependencies, add a boundary-check script, and document the dependency policy for Core, Player, Shared, Enemy, Items, MapGimmicks, Projectiles, UI, and Editor code.

## Explicit Non-goals
- Do not implement runtime gameplay classes.
- Do not create scenes or prefabs.
- Do not change package versions.

## Boundary Rules
- Feature modules may depend on Shared.
- Non-lead modules must not depend on Player internals.
- Runtime assemblies must not depend on Editor assemblies.
- SceneBuilder/editor assemblies must remain editor-only.

## Contract Change Behavior
If enforcing boundaries requires changing contracts, stop and create a Contract Change Request instead of editing runtime C#.

## Acceptance Criteria
- Assembly dependency direction is documented and enforced where possible.
- Boundary script scans forbidden APIs and direct Player internals.
- Runtime/editor separation is explicit.
- Future agents have clear validation commands.

## Definition of Done
- Changed files are limited to asmdefs, scripts, and governance docs.
- Boundary script can run locally or has precise manual fallback.
- No runtime C# implementation is added.

## Validation
- `git diff --name-only`
- Run boundary-check script if available.
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage" Assets/Scripts`
- Unity compile check

## Notes for Future Codex Agent
Keep enforcement incremental. Do not redesign module layout unless the current layout makes the boundary impossible.

