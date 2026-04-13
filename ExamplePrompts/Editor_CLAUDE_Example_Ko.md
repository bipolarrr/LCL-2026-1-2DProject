# Editor

리드 개발자 전용. 에디터 타임 씬 셋업 + 개발 편의 도구.

## 현재 구현

- `DemoSceneBuilder` — 메뉴: `Tools > Build Demo Scene`. 원클릭 데모 씬: Ground, Platform, Player, Enemy, Coin, BouncePad, GameManager.

## DemoSceneBuilder 규칙

- 모든 작업 Undo 지원 (`Undo.RegisterCreatedObjectUndo`, `Undo.CollapseUndoOperations`).
- 동명 오브젝트는 삭제 후 재생성 (`DestroyIfExists`).
- Ground 레이어 없으면 자동 생성 (`GetOrCreateLayer`).
- PlayerInputReader 액션은 `Assets/InputSystem_Actions.inputactions`에서 자동 연결.
- PlayerMotor.groundCheck + groundLayer는 `SerializedObject`로 자동 연결.

## 씬 오브젝트 추가 시

1. `CreateXxx(...)` private static 메서드 추가. 기존 패턴 따를 것.
2. `Undo.RegisterCreatedObjectUndo`로 등록.
3. `BuildDemoScene()`의 Build 전에 `DestroyIfExists("Xxx")` 추가.
4. Build 구간에서 `CreateXxx(...)` 호출.
5. SerializedProperty 연결 필요? `CreatePlayer` 패턴 참조.

## 제약

- 에디터 스크립트만 — 런타임 의존 금지. `Editor/` 폴더 안에만.
- 런타임 스크립트에서 Editor API 참조 금지.
- `DemoSceneBuilder` = static 클래스 유지. MonoBehaviour 아님.
- 새 도구: 별도 static 클래스, 자체 `[MenuItem]`.
