# MapGimmicks

맵/아이템 담당 작업 영역. 리드 개발자는 리뷰만.

## 역할

맵 배치 기믹 (바운스패드, 문, 함정, 이동 플랫폼). 모두 Inspector 설정, 씬 배치 가능.

## 현재 구현

- `BouncePad` — 트리거 진입 시 `IKnockbackReceiver.ApplyKnockback()`으로 상방 힘 적용.
- `PlayerTriggeredActivator` — 플레이어 태그 감지 → 대상 `IActivatable.Activate()` 호출. oneShot 지원.

## 기믹 패턴 2가지

### 1. 직접 효과형 (BouncePad 패턴)
트리거/충돌 감지 → 인터페이스(`IKnockbackReceiver`, `IDamageable`)로 즉시 효과.

### 2. 활성화 트리거형 (PlayerTriggeredActivator 패턴)
플레이어 감지 → `IActivatable.Activate()` 호출. 문, 함정, 플랫폼용. 새 기믹이 `IActivatable` 구현하면 기존 Activator와 바로 연동.

## 허용

- 위 2가지 패턴을 따르는 새 기믹
- `IKnockbackReceiver`, `IDamageable`, `IActivatable` 등 기존 인터페이스 사용
- `[SerializeField]`로 Inspector 수치 (힘, 속도, 지연 등)
- 태그 기반 감지 (`OnTriggerEnter2D`, `OnCollisionEnter2D`)
- 필요 시 `Core/Interfaces/MapGimmicks/`에 작은 인터페이스 추가 (리드 승인)

## 금지

- Player 클래스 직접 참조 (인터페이스만)
- Player 필드 직접 수정
- `Time.timeScale` 건드리기 (`GameStateController`만 가능)
- Enemy, Items, UI 폴더 파일 수정
- 기존 `BouncePad`, `PlayerTriggeredActivator` 동작 파괴
- 공통 기믹 프레임워크 과설계 (개별 스크립트로 충분)

## 리뷰 체크리스트 (리드용)

1. 인터페이스 준수: `IKnockbackReceiver`/`IDamageable`/`IActivatable` 경유?
2. Player 타입 미참조?
3. 폴더 격리: `MapGimmicks/` 안에서만?
4. Inspector 설정: GUI로 배치 + 설정 가능?
5. 기존 기믹 보존?
6. null 검증: Inspector 참조 비어있을 때 경고 로그? (`PlayerTriggeredActivator` 패턴)
