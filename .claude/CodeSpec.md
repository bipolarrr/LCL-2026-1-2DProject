# Code Specification

SRP enforcement. Authoritative source — CLAUDE.md files summarize, this defines.

## Core Rule

One class = one responsibility. Class handling input + movement + health + status + animation = failure.

## Player File Boundaries

| File | Owns | Never |
|------|------|-------|
| PlayerInputReader | InputSystem read, state exposure | Movement, damage, status |
| PlayerMotor | Rigidbody2D move, jump, knockback | Raw input, HP, status timers |
| PlayerHealth | HP, damage, invincibility, death | Input, movement |
| PlayerStatusController | Status effects, duration ticking | Input, movement logic |
| PlayerAnimationBridge | Animator param mapping | Gameplay rule calc |
| PlayerController | Component coordination, high-level flow | Impl of above systems |

PlayerController may **call into** components. Must not **become** them.

## Inter-System Boundaries

- World gimmicks -> Player: `IDamageable`, `IKnockbackReceiver`, `IStatusEffectReceiver`, `ILaunchReceiver`
- Items -> Player: `ICollectible`, `ICollector`
- MapGimmicks chaining: `IActivatable`
- Direct field access across boundaries: **forbidden**

## UI Boundary

Display values + forward button actions. No gameplay rule calc.

## Hard Constraints

1. No god classes.
2. No file combining input + HP + status + physics.
3. No editing unrelated systems.
4. New focused component > expanding broad class.
5. Ambiguous ownership -> separate over convenient.

## Workflow

**Before code:** List responsibilities -> assign to files -> state what PlayerController won't own.
**After code:** Summarize ownership -> confirm PlayerController not god class.
