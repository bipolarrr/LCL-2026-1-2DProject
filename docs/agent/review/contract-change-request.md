# Contract Change Request Process

Use this process when a task needs a gameplay contract that is missing, ambiguous, or outside its allowed write paths.

## When To Stop

- A non-lead module needs to edit `Core`, `Player`, or `Shared`.
- A feature needs a new request struct, interface, enum, event, receiver method, or public Player/Core entry point that the current task does not allow.
- Existing design examples conflict with the canonical Shared contract direction.
- A scene builder or UI flow needs runtime API changes outside its allowed task scope.

## Request Contents

Write the request in `docs/agent/notes/` using this filename pattern:

```text
contract-change-request-TASK-NNN-short-name.md
```

Include:

- Requesting task.
- Blocking file or module.
- Current allowed write paths.
- Needed contract change.
- Why existing public APIs or Shared contracts are insufficient.
- Proposed owner for the decision.
- Tasks blocked until the request is resolved.

## Behavior After Filing

- Do not implement the blocked runtime behavior in the same task.
- Do not work around the boundary by using concrete Player/Core internals.
- Mark the assigned task as blocked in handoff notes and wait for a lead-owned task or explicit approval.
