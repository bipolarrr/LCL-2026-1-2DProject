# Planning Assumptions

## TDD Sections 27-30
- Sections 27 through 30 are placeholders in the design source, so task deliverables, implementation order, acceptance criteria, and definition of done were inferred from sections 1 through 26 and the user's required task sequence.
- The required top-level task sequence is treated as the authoritative implementation order where the TDD is incomplete.

## Canonical Contracts
- Sections 13 and 14 are treated as the canonical direction for combat and request contracts.
- `IDamageReceiver.ReceiveDamage(DamageRequest request)` is assumed to supersede beginner-friendly `PlayerController.ApplyDamage(...)` examples for cross-module damage.
- Heal and buff flows are assumed to follow the same receiver-style pattern with `IHealReceiver` and `IBuffReceiver`.
- Bounce or knockback may be represented as `BounceRequest`, `KnockbackRequest`, or a focused Player-owned public receiver entry point, but the final rule must be resolved in TASK-001 before implementation.

## Unity Validation
- Unity 6000.3.11 is assumed to be the intended editor version.
- Unity command-line validation may not be available to every future agent, so tickets include manual Unity compile checks and precise scan commands.
- SceneBuilder validation requires Unity Editor execution because it creates and saves Unity scenes.

## Contributor Boundaries
- Core and Player are treated as lead-controlled modules.
- Enemy, Items, MapGimmicks, and UI agents must stop and create a Contract Change Request if they need edits in Core, Player, or Shared.
- Shared contracts must be settled before non-lead modules begin implementation.

## Existing Project Files
- The requested `docs/design/tdd.md` path was not present during planning.
- A root-level `tdd.md` was present and was used as the design source.
- `docs/agent/` did not exist before planning, so all generated files under it are new planning artifacts.
- The workspace did not contain a `.git` directory during planning, so Git diff validation cannot run locally in this snapshot.

