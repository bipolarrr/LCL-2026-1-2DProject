# Enemy

몹 담당 작업 영역. 리드 개발자는 리뷰만.

## 역할

적 AI 행동 + 전투 로직. 각 행동(순찰, 추적, 공격) = 별도 스크립트.

## 현재 구현

- `EnemyPatrol` — 좌우 순찰만. 전투 없음.
- `EnemyContactDamage` — 접촉 시 `IDamageable`로 데미지 + 넉백. 아무 적 오브젝트에 부착 가능.

## 허용

- 새 행동 스크립트 (`EnemyChaseBehavior`, `EnemyJumpAttack` 등)
- 기존 인터페이스 사용: `IDamageable`, `IKnockbackReceiver`
- `[SerializeField]`로 Inspector 수치 (속도, 거리, 데미지)
- `OnCollisionEnter2D` / `OnTriggerEnter2D` 감지
- 적이 `IDamageable` 구현하여 플레이어 공격 수신

## 금지

- Player 클래스 직접 참조 (`PlayerHealth`, `PlayerMotor` 등)
- Player 필드 직접 수정
- `Time.timeScale` 건드리기 (`GameStateController`만 가능)
- `Core/State` 전이 규칙 수정
- Items, MapGimmicks, UI 폴더 파일 수정
- 공통 적 베이스 클래스 과설계 (지금은 컴포넌트 조합으로 충분)

## 설계 패턴

- **행동 = 컴포넌트**: 적 하나 = 행동 스크립트 조합. 예: 순찰 + 접촉 데미지 = `EnemyPatrol` + `EnemyContactDamage`
- **인터페이스 소통**: `IDamageable.TakeDamage()`로 데미지 전달
- **넉백**: 기존 `EnemyContactDamage` 패턴을 따를 것

## 리뷰 체크리스트 (리드용)

1. 인터페이스 준수: Player 상호작용이 `IDamageable`/`IKnockbackReceiver` 경유?
2. 단일 책임: 한 스크립트가 이동 + 전투 + 상태를 다 하지 않는가?
3. 폴더 격리: `Enemy/` 밖 수정 없는가?
4. Inspector 설정: `[SerializeField]`로 노출, 하드코딩 아닌가?
5. 기존 동작 보존: `EnemyPatrol`, `EnemyContactDamage` 정상 작동?
