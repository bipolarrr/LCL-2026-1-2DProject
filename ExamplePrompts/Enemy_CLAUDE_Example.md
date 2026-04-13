# Enemy

Monster dev workspace. Lead dev reviews only.

## Role

Enemy AI behavior + combat logic. Each behavior (patrol, chase, attack) = separate script.

## Current Impl

- `EnemyPatrol` — Left-right patrol only. No combat.
- `EnemyContactDamage` — Contact triggers `IDamageable` damage + knockback. Attach to any enemy GameObject.

## Allowed

- New behavior scripts (`EnemyChaseBehavior`, `EnemyJumpAttack`, etc.)
- Use existing interfaces: `IDamageable`, `IKnockbackReceiver`
- Inspector-driven values (speed, distance, damage) via `[SerializeField]`
- `OnCollisionEnter2D` / `OnTriggerEnter2D` detection
- Enemy impl `IDamageable` to receive player attacks

## Forbidden

- Direct ref to Player classes (`PlayerHealth`, `PlayerMotor`, etc.)
- Direct modify Player fields
- Touch `Time.timeScale` (only `GameStateController`)
- Modify `Core/State` transition rules
- Edit files in Items, MapGimmicks, UI folders
- Over-engineering shared enemy base class (component composition enough for now)

## Design Pattern

- **Behavior = Component**: One enemy = combo of behavior scripts. Ex: patrol + contact damage = `EnemyPatrol` + `EnemyContactDamage`
- **Interface communication**: Damage player via `IDamageable.TakeDamage()`
- **Knockback**: Follow existing `EnemyContactDamage` pattern

## Review Checklist (Lead dev)

1. Interface compliance: Player interaction through `IDamageable`/`IKnockbackReceiver`?
2. Single responsibility: One script not handling move + combat + state?
3. Folder isolation: No edits outside `Enemy/`?
4. Inspector config: Values in `[SerializeField]`, not hardcoded?
5. Existing behavior preserved: `EnemyPatrol`, `EnemyContactDamage` still work?
