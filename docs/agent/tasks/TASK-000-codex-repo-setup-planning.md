# TASK-000-codex-repo-setup-planning

## Status
Planned

## Owner Role
Lead

## Goal
Create the future repository-governance tasks and documentation hooks that keep Codex agents inside module boundaries before gameplay work begins.

## Source Design Sections
- 5. Architecture Principles
- 5.9 모듈 간 경계
- 5.10 씬 구성과 에디터 코드
- 5.12 AI 코드 에이전트 작성 규칙
- 6. Folder Structure
- 7. Ownership and Contributor Boundaries
- 25. Coding Style
- 26. Forbidden Practices

## Dependencies
None

## Allowed Write Paths
- AGENTS.md
- Assets/Scripts/*/AGENTS.md
- CODEOWNERS
- docs/agent/review/
- docs/agent/notes/
- scripts/

## Forbidden Write Paths
- Assets/Scripts/**/*.cs
- Assets/**/*.unity
- Assets/**/*.prefab
- Assets/**/*.asset
- Packages/
- ProjectSettings/
- UserSettings/
- Library/
- Temp/
- docs/design/tdd.md

## Read-only Context
- tdd.md
- docs/agent/
- Assets/
- Packages/manifest.json

## Required Future Changes
Create future implementation tasks or governance files for root `AGENTS.md`, module-level `AGENTS.md`, `CODEOWNERS`, a boundary-check script, asmdef policy documentation, and the Contract Change Request process.

## Explicit Non-goals
- Do not implement gameplay.
- Do not create C# source files.
- Do not create Unity assets, scenes, prefabs, or asmdefs.
- Do not modify the design document.

## Boundary Rules
- Repo rules must state that agents may only edit assigned folders.
- Repo rules must state that non-lead modules cannot edit Core, Player, or Shared directly.
- SceneBuilder guidance must keep editor-only code under an `Editor` folder.

## Contract Change Behavior
If governance work requires changing gameplay contracts, stop and create a Contract Change Request instead of changing runtime code.

## Acceptance Criteria
- Root agent guidance exists or is explicitly scheduled.
- Module agent guidance exists or is explicitly scheduled.
- CODEOWNERS ownership is defined or explicitly scheduled.
- Boundary script and asmdef policy are defined or explicitly scheduled.
- Contract Change Request process is documented.

## Definition of Done
- Changed files are limited to governance and documentation paths.
- `git diff --name-only` shows no gameplay code changes.
- Boundary review checklist is referenced by repo guidance.
- A reviewer can tell which files each future role may edit.

## Validation
- `git diff --name-only`
- Manual check that no `Assets/**/*.cs` files changed.
- Manual check that no Unity assets, scenes, prefabs, or asmdefs changed.

## Notes for Future Codex Agent
Keep this patch procedural and small. Stop if the task expands into runtime architecture or gameplay implementation.

