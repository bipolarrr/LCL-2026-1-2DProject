# Assembly Definition Policy

Assembly definitions are boundary enforcement, not gameplay implementation. Add or change `.asmdef` files only in a task that explicitly allows assembly boundary work.

## Target Shape

- Runtime modules should be separated by ownership when assemblies are introduced.
- `Shared` must contain pure request structs, interfaces, enums, and simple cross-module data.
- Feature modules may depend on `Shared`.
- `Core` may coordinate approved modules through Shared contracts and owned public entry points.
- Non-lead feature modules must not depend on `Player` internals.
- Runtime assemblies must not reference editor-only assemblies or `UnityEditor`.
- Editor assemblies must live under `Assets/Scripts/Editor/` and may reference runtime assemblies only as needed for tools.

## Review Checks

- Confirm every new assembly definition is in an allowed write path for the task.
- Confirm no non-lead module gains a direct reference to `Core`, `Player`, or another feature module without an approved task.
- Confirm editor-only references are restricted to editor assemblies.
- Confirm adding an assembly definition does not force unrelated runtime code edits.

If a dependency cannot be represented without breaking ownership rules, write a Contract Change Request and stop.
