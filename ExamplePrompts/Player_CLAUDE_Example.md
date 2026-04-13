# Player

Lead dev only. Others don't modify. Minimal public method additions allowed after Lead review.

## File Boundaries (Strict)

| File | Responsibility | Never Does |
|------|---------------|------------|
| `PlayerInputReader` | Read InputSystem, expose state | Move, damage, status |
| `PlayerMotor` | Rigidbody2D move, jump, knockback | Read input, HP, status timers |
| `PlayerHealth` | HP, damage, invincibility, death | Read input, movement |
| `PlayerController` | Coordinate components, score | Own input parsing, HP, status ticks, physics |
| `PlayerStatusController` (not impl) | Status effects, duration | Read input, own movement |
| `PlayerAnimationBridge` (not impl) | Animator param mapping | Gameplay rule calc |

## PlayerController = Coordinator Only

- `FixedUpdate`: Poll InputReader, call Motor Move/Jump.
- Impl `ICollector` for score tracking.
- **Never**: Input parsing, HP logic, status ticks, Rigidbody physics inside PlayerController.

## Interface Impl

- `PlayerHealth` -> `IDamageable` (TakeDamage, ApplyKnockback -> delegates to Motor)
- `PlayerMotor` -> `IKnockbackReceiver` (ApplyKnockback)
- `PlayerController` -> `ICollector` (AddScore)

## External Communication

- External systems (Enemy, Items, MapGimmicks) never access Player internals.
- Interact only via `IDamageable`, `IKnockbackReceiver`, `ICollector`.
- New interaction needed? Add interface in `Core/Interfaces/`, impl in Player.

## Before Writing Code

1. List responsibilities for feature.
2. Assign each to specific file.
3. State what PlayerController will NOT own.

## After Writing Code

- Summarize each file's ownership.
- Confirm PlayerController not god class.

## New Feature Decision

- Adding responsibility makes file do 2+ roles? -> New file.
- Tempted to put logic in PlayerController? -> Make dedicated component, Controller only calls it.
