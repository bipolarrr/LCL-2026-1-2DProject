You are working in a Unity 6 2D platformer project.

Architectural goal:
Keep the codebase safe for team collaboration by enforcing strict single-responsibility file boundaries.
Do not collapse multiple gameplay responsibilities into PlayerController or any other god class.

Core rule:
Each class must have one primary responsibility only.
If a class starts handling input, movement, health, status effects, animation, and world interaction together, that is a failure.

Player file boundaries:
- PlayerInputReader:
  Reads and exposes player input state only.
  It does not move the character, apply damage, or manage statuses.

- PlayerMotor:
  Handles Rigidbody2D-based movement, jump, gravity, launch, and knockback application only.
  It does not read raw input directly, does not manage HP, and does not manage status effect timers.

- PlayerHealth:
  Owns current/max HP, damage application, death checks, invulnerability windows if applicable.
  It does not read input or perform movement logic.

- PlayerStatusController:
  Owns active status effects such as poison, burn, slow, duration ticking, and effect application.
  It does not read input.
  It should not directly own full movement logic.

- PlayerAnimationBridge:
  Maps gameplay state into Animator parameters only.
  It does not compute gameplay rules.

- PlayerController:
  Coordinates components and high-level flow only.
  It may call into PlayerMotor / PlayerHealth / PlayerStatusController.
  It must not become the implementation owner of all those systems.

World/UI boundaries:
- World gimmicks must never directly modify player internal fields.
  They must communicate through interfaces such as IDamageable, IStatusEffectReceiver, ILaunchReceiver.
- UI classes must only display values and forward button actions.
  UI must not calculate gameplay rules.

Hard constraints:
- Do not introduce god classes.
- Do not put raw input reading, HP logic, status ticking, and Rigidbody movement in the same file.
- Do not directly edit unrelated systems to “make it work”.
- Prefer adding a small focused component over expanding an existing broad class.
- If a responsibility is ambiguous, choose separation over convenience.

Before writing code:
1. List the responsibilities involved in this feature.
2. State which file/class should own each responsibility.
3. State what PlayerController will NOT own.

After writing code:
- Summarize which files own which responsibilities.
- Explicitly confirm that PlayerController does not directly own input parsing, HP storage, status ticking, and movement physics all at once.