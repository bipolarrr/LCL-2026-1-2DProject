# UI

UI/메뉴 담당 작업 영역. 리드 개발자는 리뷰만.

## 역할

게임 UI. 뷰 레이어 전용:
- 이벤트 구독으로 값 **표시** (`OnHealthChanged`, `OnScoreChanged`). 데이터를 소유하거나 계산하지 않는다.
- 버튼 액션을 시스템 public 메서드로 **전달**. 게임플레이 로직 금지 (`if (hp < 30) ShowWarning()` 같은 것 안 됨).

## 현재 구현

- `GameStateButtonActions` — 버튼 클릭을 `GameStateController.Instance`의 Request 메서드로 전달. Play, Pause, Resume, Restart.
- `GameStatePanelBinder` — `OnStateChanged` 구독, `GameState`별 패널 표시/숨김.

## 허용

- `GameStatePanelBinder` 패턴의 새 바인더
- `GameStateButtonActions` 패턴의 새 버튼 액션
- `IGameStateReader.OnStateChanged` 구독
- Player/Enemy public 이벤트 구독하여 표시
- Inspector에서 패널, 텍스트, 이미지 참조 연결

## 금지

- `Time.timeScale` 건드리기 (`GameStateController` 경유만)
- `GameState` enum 직접 대입
- 게임플레이 규칙 계산 (데미지 공식, 점수 배수 등)
- Player, Enemy, Items, MapGimmicks 파일 수정
- `Find`로 씬 오브젝트 직접 조작

## 리뷰 체크리스트 (리드용)

1. 뷰 전용: UI 스크립트에 게임플레이 로직(조건문, 수식) 없는가?
2. 이벤트 기반: 이벤트 구독으로 갱신, 매 프레임 폴링 아닌가?
3. GameStateController 경유: 상태 변경이 Request 메서드 통하는가?
4. 폴더 격리: `UI/` 안에서만?
5. 기존 동작 보존: 패널/버튼 정상 작동?
