# Codex Repository Guidance

This repository is a Unity 2D project. Follow `tdd.md` as the current canonical design source unless a later approved design document replaces it.

## Task Scope

- Work only on the task explicitly assigned in `docs/agent/tasks/`.
- Edit only the paths listed in that task's `Allowed Write Paths`.
- Do not start later tasks, dependency tasks, cleanup work, or gameplay implementation unless the assigned task says to do so.
- If the task needs a contract or module change outside its allowed paths, stop and write a Contract Change Request instead of editing runtime code.

## Required Reviews

- Use `docs/agent/review/boundary-review-checklist.md` before and after implementation.
- Use `docs/agent/review/forbidden-api-checklist.md` for every runtime C# change.
- Use `docs/agent/review/asmdef-policy.md` before adding or changing assembly definitions.
- Use `docs/agent/review/contract-change-request.md` when a task needs an unapproved gameplay contract change.

## Module Boundaries

- Contributors may edit only their assigned folders.
- Non-lead modules must not edit `Assets/Scripts/Runtime/Core/`, `Assets/Scripts/Runtime/Player/`, or `Assets/Scripts/Runtime/Shared/` directly.
- Cross-module gameplay interactions must follow `docs/architecture/canonical-gameplay-contracts.md`.
- Damage, heal, buff, bounce, and knockback requests must go through Shared receiver interfaces and request structs.
- External modules must not mutate Player internals, Player `Rigidbody2D`, Player `Transform`, health fields, movement state, invincibility state, guarding state, or death state.
- Runtime code must not use `UnityEditor` APIs.
- SceneBuilder and editor tooling must live under `Assets/Scripts/Editor/`.

## Ownership

| Area | Owner role | Writable by default |
| --- | --- | --- |
| `Assets/Scripts/Runtime/Core/` | Lead | Lead only |
| `Assets/Scripts/Runtime/Player/` | Lead | Lead only |
| `Assets/Scripts/Runtime/Shared/` | Lead | Lead only |
| `Assets/Scripts/Editor/SceneBuilders/` | Lead | Lead only |
| `Assets/Scripts/Runtime/UI/` | UI | UI task owner |
| `Assets/Prefabs/UI/` | UI | UI task owner |
| `Assets/Scripts/Runtime/Enemy/` | Enemy | Enemy task owner |
| `Assets/ScriptableObjects/Enemy/` | Enemy | Enemy task owner |
| `Assets/Prefabs/Enemy/` | Enemy | Enemy task owner |
| `Assets/Scripts/Runtime/Items/` | Items | Item task owner |
| `Assets/Scripts/Runtime/MapGimmicks/` | MapGimmicks | MapGimmick task owner |
| `Assets/Prefabs/Items/` | Items | Item task owner |
| `Assets/Prefabs/MapGimmicks/` | MapGimmicks | MapGimmick task owner |

## Validation

Run the relevant checks before handoff:

```powershell
scripts/check-boundaries.ps1 -TaskFile docs/agent/tasks/TASK-NNN-task-name.md
```

If Git is available, also run:

```powershell
git diff --name-only
```

Confirm no task changed gameplay code, Unity scenes, prefabs, assets, packages, project settings, or user settings unless the task explicitly allowed it.
