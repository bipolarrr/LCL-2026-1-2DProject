# Editor Agent Guidance

Editor code is for Unity Editor tooling only. Runtime gameplay code must not be placed here.

## Allowed Editor Areas

- `SceneBuilders/`: reproducible scene construction tools owned by the lead.
- `Tools/`: development-only editor utilities when an assigned task explicitly allows them.

## Boundary Rules

- SceneBuilder code must remain under an `Editor` folder.
- Editor scripts may use `UnityEditor`; runtime scripts may not.
- SceneBuilder work must not introduce gameplay behavior just to satisfy editor setup.
- If a scene builder needs a new runtime API, stop and write a Contract Change Request instead of editing runtime modules outside the assigned task.
