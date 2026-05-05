# Design Conflicts

## Missing Design Document Path
- Conflict: The request names `docs/design/tdd.md`, but the workspace contains `tdd.md` at the repository root.
- Why it matters: Future agents may look in the requested path and miss the actual design source.
- Resolving task: TASK-000 should create repo guidance that points agents to the canonical design path, or a lead should move/copy the design in a separate approved documentation task.
- Blocks later tasks: No, because planning used the root `tdd.md`, but it should be resolved early.

## ApplyDamage Examples vs Receiver Contract
- Conflict: Beginner-friendly examples show Enemy, Item, or MapGimmick calling `PlayerController.ApplyDamage`, `ApplyHeal`, or `ApplyBounce` directly, while sections 13 and 14 define canonical contracts around `IDamageReceiver.ReceiveDamage(DamageRequest request)`.
- Why it matters: If agents follow the early examples, non-lead modules may reference Player internals or concrete Player classes instead of Shared interfaces.
- Resolution: TASK-001 resolves this in `docs/architecture/canonical-gameplay-contracts.md`. Cross-module gameplay requests use Shared receiver interfaces and request structs. Convenience methods are allowed only as owning-module wrappers and do not replace the receiver boundary.
- Blocks later tasks: Resolved for TASK-001. Later tasks still depend on TASK-002 creating the Shared contracts and TASK-003 enforcing boundaries.

## Bounce vs Knockback Contract Shape
- Conflict: Section 5 describes `BounceRequest` and `KnockbackRequest` as possible request structs, while later sections focus primarily on `DamageRequest`, `HealRequest`, and `BuffRequest`.
- Why it matters: MapGimmicks and Player damage handling need a clear rule for external vertical bounce and combat knockback.
- Resolution: TASK-001 chooses both `BounceRequest` and `KnockbackRequest` as standalone Shared requests. Damage-caused knockback remains in `DamageRequest`; non-damaging forced movement uses `BounceRequest` or `KnockbackRequest`.
- Blocks later tasks: Resolved for TASK-001. Player receivers, Items, MapGimmicks, and combat remain blocked until TASK-002 creates the contracts.

## Direct Convenience Methods vs Interface-only Interactions
- Conflict: The TDD supports easy methods like `ApplyDamage(10)` for beginners, but module boundaries require cross-module interactions through Shared contracts.
- Why it matters: Convenience methods are useful inside Player but dangerous if exposed as the primary external integration point for non-lead modules.
- Resolution: TASK-001 treats convenience methods as owning-module wrappers only. Non-lead modules call Shared receiver interfaces, not concrete Player convenience methods.
- Blocks later tasks: Resolved for TASK-001. Non-lead gameplay modules remain blocked by their normal dependencies.

## Stage Result Persistence Strategy
- Conflict: Section 20 allows either JSON or PlayerPrefs for local persistence and does not choose one.
- Why it matters: UI and Core need a stable extension point, but storage choice can create unnecessary scope.
- Resolving task: TASK-017 should choose a minimal strategy or define an interface with one simple implementation.
- Blocks later tasks: Partially. Blocks final UI result persistence but not core stage flow.

## SceneBuilder Timing
- Conflict: The design requires SceneBuilder-created scenes, but runtime module implementation must happen before builders can wire references safely.
- Why it matters: Building scenes too early creates brittle placeholders or forces cross-module edits.
- Resolving task: TASK-018 runs after core runtime skeletons exist.
- Blocks later tasks: No, but SceneBuilder work must wait for dependencies.
