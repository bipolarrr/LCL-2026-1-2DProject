# CLAUDE.md

Unity 6 2D 플랫포머 프로토타입. 다인 팀, 역할별 폴더 분리.

## 스펙 (코드 작성 전 필독)

- `.claude/CodeSpec.md` — SRP 파일 경계, 하드 제약
- `.claude/GameSpec.md` — 게임 설계 제약, 인터페이스 요구사항
- `.claude/RolePerPerson.md` — 팀 역할 배정, 폴더 소유권

## 팀 구조

| 역할 | 범위 | 폴더 |
|------|------|------|
| 리드 (나) | 코어 유틸, 프레임워크, 플레이어 시스템 | `Core/`, `Player/` |
| 몹 담당 | 몬스터 설계 | `Enemy/` |
| 맵/아이템 (2명) | 맵 레이아웃, 기믹/아이템 스크립트 | `MapGimmicks/`, `Items/` |
| UI/메뉴 | 씬 전환, 일시정지, 옵션 | `UI/` |

`Assets/Scripts/` 하위 각 폴더에 자체 `CLAUDE.md`가 있다.

## 프로젝트 전체 규칙

- 클래스당 책임 하나. god class 금지.
- `Core/Interfaces/`를 통해 소통. 직접 필드 접근 금지.
- DeltaTime 기반. FixedUpdate 적절히 사용 가능.
- 헝가리안 표기법 금지 (Unity `m_` 직렬화 필드 제외).
- EventBus, ServiceLocator, DI 프레임워크, SO 대규모 시스템 금지.
- `Time.timeScale`은 `GameStateController`만 소유.
- 넓은 클래스 확장보다 새 집중 파일 추가 우선.

## 리드 개발자 리뷰 관점

다른 팀원 코드(Enemy, Items, MapGimmicks, UI) 리뷰 시:
1. 인터페이스 준수 — `Core/Interfaces/` 경유 소통?
2. 폴더 격리 — 담당 폴더 안에 머무는가? Core/Player 수정 최소?
3. 구조적 부작용 없음 — 게임 상태, 플레이어 내부, 코어 프레임워크 수정 없는가?
4. Inspector 기반 — 디자이너가 코드 없이 설정 가능?
