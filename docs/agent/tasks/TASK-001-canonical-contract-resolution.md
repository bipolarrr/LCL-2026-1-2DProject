# TASK-001-canonical-contract-resolution

## Status
Planned

## Owner Role
Lead

## Goal
Resolve and document the canonical cross-module gameplay contract so later agents do not mix beginner `ApplyDamage` examples with the receiver-based `IDamageReceiver.ReceiveDamage` direction.

## Source Design Sections
- 5.2 Player의 책임
- 5.3 Enemy, Item, MapGimmick의 책임
- 5.4 요청 메서드와 구조체 사용 규칙
- 5.5 요청 구조체의 범위
- 5.9 모듈 간 경계
- 13. Combat System
- 14. Damage / Heal / Buff Request Contracts

## Dependencies
- TASK-000

## Allowed Write Paths
- docs/agent/notes/
- docs/agent/review/
- docs/architecture/
- AGENTS.md
- Assets/Scripts/*/AGENTS.md

## Forbidden Write Paths
- Assets/Scripts/**/*.cs
- Assets/**/*.asmdef
- Assets/**/*.unity
- Assets/**/*.prefab
- Assets/**/*.asset
- Packages/
- ProjectSettings/
- docs/design/tdd.md

## Read-only Context
- tdd.md
- docs/agent/notes/design-conflicts.md
- docs/agent/review/boundary-review-checklist.md

## Required Future Changes
Create a concise canonical contract decision record stating whether cross-module damage, heal, buff, bounce, and knockback use Shared receiver interfaces, Player public methods, or a mixed rule with strict scope.

## Explicit Non-goals
- Do not implement the contracts.
- Do not edit runtime C# files.
- Do not implement Player, Enemy, Items, MapGimmicks, UI, or combat.

## Boundary Rules
- Treat sections 13 and 14 as the default canonical direction unless the decision record explicitly overrides them.
- Non-lead modules must not reference concrete Player internals.
- Convenience methods, if allowed, must not weaken Shared contract boundaries.

## Contract Change Behavior
If this task cannot resolve the rule from the design, stop and write a Contract Change Request documenting the unresolved decision and blocking tasks.

## Acceptance Criteria
- The conflict between `ApplyDamage` examples and `IDamageReceiver.ReceiveDamage` is explicitly addressed.
- Damage, heal, buff, bounce, and knockback integration rules are each covered.
- The decision identifies which later tasks are blocked until complete.
- The decision is easy for a future implementation agent to find.

## Definition of Done
- Changed files are documentation or governance only.
- Later tasks can cite the canonical contract without rereading the whole TDD.
- Blocking status is reflected in `docs/agent/notes/design-conflicts.md` or equivalent review note.

## Validation
- `git diff --name-only`
- Manual check that no runtime code changed.
- Manual check that the decision mentions `IDamageReceiver.ReceiveDamage(DamageRequest request)`.
- Manual check that Enemy, Items, MapGimmicks, UI, and Player combat remain blocked until this task is complete.

## Notes for Future Codex Agent
Keep the decision narrow. Do not solve every future interaction pattern; define only enough to unblock Shared contracts and module work.

