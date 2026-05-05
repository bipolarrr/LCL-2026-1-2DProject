# Boundary Review Checklist

## Module Ownership
- Confirm each task changed only its allowed write paths.
- Confirm contributors did not edit outside their assigned module folders.
- Confirm non-lead modules did not edit Core, Player, or Shared without an approved Contract Change Request.
- Confirm SceneBuilder code is under an `Editor` folder only.

## Player Boundary
- Enemy, Items, MapGimmicks, Projectiles, and UI must not mutate Player internals.
- External systems must not directly change Player `Rigidbody2D`, `Transform`, `currentHp`, speed, jump count, invincible state, guarding state, or dead state.
- External systems may interact with Player only through Shared contracts and approved Player public receiver entry points.

## Core Boundary
- Core owns game flow, scene transitions, pause, retry, main menu routing, game over, and stage progression.
- UI must request state changes through Core and must not call scene loading APIs directly.
- Enemy, Items, and MapGimmicks must not call `SceneManager.LoadScene` directly.

## Shared Contract Boundary
- Cross-module gameplay interaction must use Shared contracts.
- `docs/architecture/canonical-gameplay-contracts.md` is the canonical contract decision record.
- `IDamageReceiver.ReceiveDamage(DamageRequest request)` is the canonical damage contract.
- Receiver implementations should use explicit interface implementation, such as `IDamageReceiver.ReceiveDamage(DamageRequest request)`.
- Request structs should remain simple data carriers.

## Review Output
- Record any violation with file path, line number, owning task, and required follow-up.
- If a boundary requires contract changes, create or request a Contract Change Request before allowing implementation changes.
