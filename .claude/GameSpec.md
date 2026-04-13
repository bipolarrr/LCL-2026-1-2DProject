# Game Specification

2D platformer with platforms, gimmicks, items, status effects.

## Collaboration

Multi-person team. Each person works within assigned folders only. See `RolePerPerson.md`.

## Technical Constraints

- DeltaTime-based in Update(). FixedUpdate allowed when appropriate.
- No frame-rate-dependent physics results.

## Communication Rule

All cross-system interaction through interfaces. No direct field modification across boundaries.
- Forbidden: `Player.isPoisoned = true` from Item script.
- Correct: call interface method Player implements.

## Extensibility

- Lead dev extends Core interfaces + framework.
- Other members' work must not require modifying shared utilities.
- Folder-based isolation per team role.

## Naming

No Hungarian notation. Unity `m_` for serialized fields OK.
