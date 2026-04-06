# Project Rules

## Before writing any code

Read and follow these specification files:

- `.claude/CodeSpec.md` — File boundary rules, single-responsibility enforcement, hard constraints
- `.claude/GameSpec.md` — Game design constraints, collaboration rules, interface requirements

## Key rules from CodeSpec (quick reference)

- Each class must have one primary responsibility only.
- PlayerController coordinates only — it must NOT own input parsing, HP storage, status ticking, or movement physics.
- Player file boundaries: PlayerInputReader, PlayerMotor, PlayerHealth, PlayerStatusController, PlayerAnimationBridge, PlayerController.
- World gimmicks communicate through interfaces (IDamageable, ILaunchReceiver, IStatusEffectReceiver, etc.), never by modifying player fields directly.
- UI classes only display values and forward button actions. UI must not calculate gameplay rules.
- Before writing code: list responsibilities, assign each to a file, state what PlayerController will NOT own.
- After writing code: summarize file ownership, confirm PlayerController is not a god class.
