# Runtime Agent Guidance

Runtime code is built into the game player. Do not reference `UnityEditor` from any file under this folder.

## Runtime Modules

- `Core/`: lead-owned game flow, scene routing, pause, retry, menu, game over, and stage progression.
- `Player/`: lead-owned Player state, health, input response, physics, movement, damage, heal, buff, knockback, and animation coordination.
- `Shared/`: lead-owned interfaces, request structs, enums, and simple cross-module data contracts.
- `Enemy/`: Enemy-owned behavior that uses Shared contracts and approved public receiver entry points.
- `Items/`: Item-owned behavior that uses Shared contracts and approved public receiver entry points.
- `MapGimmicks/`: MapGimmick-owned behavior that uses Shared contracts and approved public receiver entry points.
- `UI/`: UI-owned presentation and user requests routed through Core.
- `Stages/`, `Cutscenes/`, `Save/`: task-owned only when an assigned task explicitly allows edits.

## Boundary Rules

- Stay inside the assigned module folder.
- Non-lead modules must not edit `Core/`, `Player/`, or `Shared/`.
- Non-lead modules must not directly mutate Player internals, Player physics, Player transform, Player health, or Player state flags.
- Enemy, Items, MapGimmicks, and Projectiles must depend on Shared receiver contracts instead of concrete Player internals for damage, heal, buff, bounce, and knockback.
- Receiver components should implement interfaces explicitly, such as `IDamageReceiver.ReceiveDamage(DamageRequest request)`.
- UI must not load scenes directly. Route game-flow requests through Core.
- If a needed Shared contract or Player receiver does not exist, stop and write a Contract Change Request.
