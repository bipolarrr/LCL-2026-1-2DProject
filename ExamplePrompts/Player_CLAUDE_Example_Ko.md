# Player

리드 개발자 전용. 다른 팀원 수정 금지. 리드 검토 후 최소한의 public 메서드 추가만 허용.

## 파일 경계 (엄격)

| 파일 | 책임 | 절대 안 하는 것 |
|------|------|----------------|
| `PlayerInputReader` | InputSystem 읽기, 상태 노출 | 이동, 데미지, 상태이상 |
| `PlayerMotor` | Rigidbody2D 이동, 점프, 넉백 | 입력 읽기, HP, 상태이상 타이머 |
| `PlayerHealth` | HP, 데미지, 무적, 사망 | 입력 읽기, 이동 |
| `PlayerController` | 컴포넌트 조율, 점수 | 입력 파싱, HP, 상태이상 틱, 물리 직접 소유 |
| `PlayerStatusController` (미구현) | 상태이상, 지속시간 | 입력 읽기, 이동 직접 소유 |
| `PlayerAnimationBridge` (미구현) | Animator 파라미터 매핑 | 게임플레이 규칙 계산 |

## PlayerController = 조율자

- `FixedUpdate`: InputReader 폴링 → Motor의 Move/Jump 호출.
- `ICollector` 구현으로 점수 추적.
- **절대 금지**: 입력 파싱, HP 로직, 상태이상 틱, Rigidbody 물리를 PlayerController 안에 넣는 것.

## 인터페이스 구현

- `PlayerHealth` → `IDamageable` (TakeDamage, ApplyKnockback → Motor에 위임)
- `PlayerMotor` → `IKnockbackReceiver` (ApplyKnockback)
- `PlayerController` → `ICollector` (AddScore)

## 외부 소통

- 외부 시스템(Enemy, Items, MapGimmicks)은 Player 내부에 접근 금지.
- `IDamageable`, `IKnockbackReceiver`, `ICollector`를 통해서만 상호작용.
- 새 상호작용 필요? `Core/Interfaces/`에 인터페이스 추가, Player에서 구현.

## 코드 작성 전

1. 기능에 필요한 책임 나열.
2. 각 책임을 파일에 배정.
3. PlayerController가 소유하지 않을 것을 명시.

## 코드 작성 후

- 각 파일의 소유 책임 요약.
- PlayerController가 god class가 아닌지 확인.

## 새 기능 판단

- 책임 추가로 파일이 2개 이상 역할? → 새 파일.
- PlayerController에 로직 넣고 싶은 유혹? → 전용 컴포넌트 만들고 Controller는 호출만.
