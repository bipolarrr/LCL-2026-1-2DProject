# MapGimmicks

Map/Item dev workspace. Lead dev reviews only.

## Role

Map-placed gimmicks (bounce pads, doors, traps, moving platforms). All Inspector-configurable, scene-placeable.

## Current Impl

- `BouncePad` — Trigger enter: `IKnockbackReceiver.ApplyKnockback()` upward force.
- `PlayerTriggeredActivator` — Detect player tag, call target `IActivatable.Activate()`. Supports oneShot flag.

## Two Gimmick Patterns

### 1. Direct Effect (BouncePad pattern)
Trigger/collision detect -> apply effect via interface (`IKnockbackReceiver`, `IDamageable`).

### 2. Activation Trigger (PlayerTriggeredActivator pattern)
Detect player -> `IActivatable.Activate()` targets. For doors, traps, platforms. New gimmick impl `IActivatable` = instant compatibility with existing Activator.

## Allowed

- New gimmicks following above 2 patterns
- Use `IKnockbackReceiver`, `IDamageable`, `IActivatable` + existing interfaces
- Inspector values via `[SerializeField]` (force, speed, delay, etc.)
- Tag-based detection (`OnTriggerEnter2D`, `OnCollisionEnter2D`)
- Add small interface to `Core/Interfaces/MapGimmicks/` if needed (Lead dev approval)

## Forbidden

- Direct ref to Player classes (interfaces only)
- Direct modify Player fields
- Touch `Time.timeScale` (only `GameStateController`)
- Edit files in Enemy, Items, UI folders
- Break existing `BouncePad`, `PlayerTriggeredActivator`
- Over-engineer shared gimmick framework (individual scripts enough)

## Review Checklist (Lead dev)

1. Interface compliance: Interaction via `IKnockbackReceiver`/`IDamageable`/`IActivatable`?
2. No Player type refs in code?
3. Folder isolation: Changes in `MapGimmicks/` only?
4. Inspector config: Placeable + configurable via GUI?
5. Existing gimmicks preserved?
6. Null checks: Warn log when Inspector refs empty (like `PlayerTriggeredActivator` pattern)?
