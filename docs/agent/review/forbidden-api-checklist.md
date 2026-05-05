# Forbidden API Checklist

## Runtime Forbidden APIs
- `using UnityEditor`
- `UnityEditor.`
- `GameObject.Find`
- `FindObjectOfType`
- `Object.FindObjectOfType`
- `FindObjectsOfType`
- `SendMessage`
- Direct `SceneManager.LoadScene` outside Core or approved SceneBuilder/editor tools

## Player Internal Mutation Scan
- Direct writes to Player health fields such as `currentHp`, `maxHp`, or equivalent internals.
- Direct writes to Player movement values such as speed, jump count, dash state, glide state, or equivalent internals.
- Direct writes to Player `Rigidbody2D.linearVelocity`, `Rigidbody2D.velocity`, `Transform.position`, or `Transform.Translate` from outside Player-owned components.
- Direct writes to Player invincible, guarding, hit reaction, or dead state from outside Player-owned components.

## Suggested Commands
- `git diff --name-only`
- `rg "using UnityEditor|UnityEditor\\." Assets/Scripts`
- `rg "GameObject\\.Find|FindObjectOfType|FindObjectsOfType|SendMessage" Assets/Scripts`
- `rg "SceneManager\\.LoadScene" Assets/Scripts`
- `rg "currentHp|jumpCount|isInvincible|isGuarding|isDead|linearVelocity|\\.velocity|Transform\\.position|\\.position\\s*=" Assets/Scripts`

## Manual Checks
- Confirm every runtime assembly compiles without editor-only references.
- Confirm UI changes game flow only through Core request methods.
- Confirm Enemy, Items, MapGimmicks, and Projectiles depend on Shared contracts, not Player implementation details.

