# Items

Map/Item dev workspace. Lead dev reviews only.

## Role

Collectible items. All scene-placeable, Inspector-only config.

## Worker Guide

Detailed rules, allowed/forbidden patterns, examples: see `WeakModelPromptExamples.md`.

## Current Impl

- `Coin` — Impl `ICollectible`. On trigger enter: `ICollector.AddScore()` then self-destroy.

## Base Item Structure

```
SpriteRenderer + Collider2D(IsTrigger) + Item Script
```

## Scope Limit

- Max 3 files changed per task.
- Solve within `Assets/Scripts/Items/`. Stop and reassess if scope creeps into Core structure.

## Review Checklist (Lead dev)

1. Interface compliance: Interaction via `ICollectible`/`ICollector`/`IDamageable`?
2. No Player type refs: No `PlayerHealth`, `PlayerMotor`, `PlayerController` in code/using?
3. Folder isolation: Changes in `Items/`? Core/Player touches minimal?
4. Inspector config: Placeable + configurable without code?
5. No over-engineering: Adding 1 item, not building universal system?
6. Existing behavior preserved: `Coin` still works?
