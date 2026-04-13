# CLAUDE.md

Unity 6 2D platformer prototype. Multi-person team, role-based folder separation.

## Specs (read before writing code)

- `.claude/CodeSpec.md` — SRP file boundaries, hard constraints
- `.claude/GameSpec.md` — Game design constraints, interface requirements (Korean)
- `.claude/RolePerPerson.md` — Team role assignments, folder ownership (Korean)

## Team Structure

| Role | Scope | Folder |
|------|-------|--------|
| Lead (me) | Core utils, framework, player system | `Core/`, `Player/` |
| Enemy AI | Monster design | `Enemy/` |
| Map/Item (2) | Map layout, gimmick/item scripts | `MapGimmicks/`, `Items/` |
| UI/Menu | Scene transitions, pause, options | `UI/` |

Each folder under `Assets/Scripts/` has own `CLAUDE.md` with subsystem rules.

## Project-Wide Rules

- One responsibility per class. No god classes.
- Communicate through `Core/Interfaces/`, never direct field access.
- DeltaTime-based. FixedUpdate acceptable when appropriate.
- No Hungarian notation (except Unity `m_` for serialized fields).
- No EventBus, ServiceLocator, DI framework, SO-based large systems.
- `Time.timeScale` owned by `GameStateController` only.
- Prefer new focused file over expanding broad class.

## Lead Dev Review Stance

When reviewing other members' code (Enemy, Items, MapGimmicks, UI):
1. Interface compliance — communicates through `Core/Interfaces/`?
2. Folder containment — stays in assigned folder? Minimal Core/Player touches?
3. No structural side effects — no modifying game state, player internals, core framework?
4. Inspector-driven — designers configure without code changes?
