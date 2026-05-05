# Canonical Gameplay Contracts

Decision for TASK-001.

## Decision

Cross-module gameplay requests use Shared receiver interfaces and request structs. Later implementation tasks may add convenient methods inside the owning module, but non-lead modules must not depend on concrete Player or Core classes to damage, heal, buff, bounce, or knock back the Player.

The beginner-style `ApplyDamage`, `ApplyHeal`, `ApplyBounce`, and `ApplyKnockback` examples in `tdd.md` are treated as shorthand for Player-owned internal wrappers or examples that predate the receiver boundary. They do not authorize Enemy, Items, MapGimmicks, UI, or Projectiles to reference Player internals or concrete Player controllers.

## Interface Implementation Rule

Receiver components should implement Shared receiver interfaces explicitly. Use the interface name on the method so accidental concrete calls do not become the cross-module API.

```cs
public sealed class PlayerDamageReceiver : MonoBehaviour, IDamageReceiver
{
    void IDamageReceiver.ReceiveDamage(DamageRequest request)
    {
        // Player-owned handling.
    }
}
```

Apply the same pattern to the other receiver interfaces:

- `IHealReceiver.ReceiveHeal(HealRequest request)`
- `IBuffReceiver.ReceiveBuff(BuffRequest request)`
- `IBounceReceiver.ReceiveBounce(BounceRequest request)`
- `IKnockbackReceiver.ReceiveKnockback(KnockbackRequest request)`

If a Player-owned component also exposes convenience methods, those methods are internal to Player-owned orchestration and do not replace the explicit interface entry point.

## Damage

- Canonical request: `DamageRequest`.
- Canonical receiver: `IDamageReceiver.ReceiveDamage(DamageRequest request)`.
- Targets: Player, Enemy, Boss, or any future damageable object that owns its own health rules.
- Initial fields: `amount`, `damageKind`, `knockbackDirection`, `knockbackForce`, `source`.
- `DamageKind` initial values: `Unknown`, `PlayerMelee`, `PlayerProjectile`, `EnemyContact`, `Trap`, `Item`.
- Damage with combat knockback keeps the knockback fields inside `DamageRequest`.
- The receiver decides whether damage applies based on its own state, such as invincible, guarding, dead, armor, or shield.

External modules should cache or query `IDamageReceiver`, create `DamageRequest`, and call `ReceiveDamage` through the interface.

## Heal

- Canonical request: `HealRequest`.
- Canonical receiver: `IHealReceiver.ReceiveHeal(HealRequest request)`.
- Initial fields: `amount`, `source`.
- Items and future heal sources must not write Player health fields directly.
- The receiver decides whether heal applies, clamps to max HP, and ignores heal when its own state requires it.

## Buff

- Canonical request: `BuffRequest`.
- Canonical receiver: `IBuffReceiver.ReceiveBuff(BuffRequest request)`.
- Initial fields: `buffKind`, `value`, `durationSeconds`, `source`.
- Initial buff kinds should cover the TDD item scope: move speed and extra jump.
- Buff sources must not edit Player movement fields, jump count, timers, or state flags directly.
- The receiver owns stacking, refresh, duration, and rejection rules.

## Bounce

- Canonical request: `BounceRequest`.
- Canonical receiver: `IBounceReceiver.ReceiveBounce(BounceRequest request)`.
- Initial purpose: non-damaging vertical or directional bounce from map gimmicks, jump pads, and similar environment interactions.
- Initial fields should include at least `direction`, `force`, and `source`.
- If a simple vertical bounce is needed, the request may use `Vector2.up` as the direction.
- External systems must not set Player `Rigidbody2D` velocity or `Transform` directly.

## Knockback

- Canonical request: `KnockbackRequest`.
- Canonical receiver: `IKnockbackReceiver.ReceiveKnockback(KnockbackRequest request)`.
- Initial purpose: non-damaging forced displacement from combat, hazards, or scripted effects.
- Initial fields should include at least `direction`, `force`, and `source`.
- Damage-caused knockback remains inside `DamageRequest`; standalone knockback uses `KnockbackRequest`.
- The Player-owned receiver delegates physical application to Player movement or motor components.

## Cross-Module Access

- Enemy, Items, MapGimmicks, Projectiles, and UI must not call concrete Player internals.
- Enemy, Items, MapGimmicks, and Projectiles may use Unity component lookup against receiver interfaces, such as `TryGetComponent(out IDamageReceiver receiver)`.
- UI does not use Player receiver interfaces for game flow. UI routes flow requests through Core-owned APIs or presentation models defined by the relevant Core/UI tasks.
- Shared request structs are data carriers only. They must not own gameplay behavior.

## Blocked Tasks

Before this decision, TASK-002 through TASK-018 were blocked anywhere they needed cross-module gameplay contracts. With this decision complete, later agents should cite this document instead of rereading the full TDD.

Implementation is still blocked by normal task dependencies:

- TASK-002 must create the Shared request structs and interfaces before feature modules use them.
- TASK-003 must enforce assembly and boundary rules before broad gameplay work.
- Player combat, Enemy, Items, MapGimmicks, and UI tasks remain blocked until their listed dependencies are complete.

## Contract Change Rule

If a later task needs a new receiver interface, request field, enum value, or public owner entry point that is not covered here, stop and write a Contract Change Request in `docs/agent/notes/` instead of editing another module.
