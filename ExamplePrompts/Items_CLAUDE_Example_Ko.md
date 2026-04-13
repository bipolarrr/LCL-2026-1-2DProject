# Items

맵/아이템 담당 작업 영역. 리드 개발자는 리뷰만.

## 역할

수집형 아이템. 모두 씬 배치 가능, Inspector만으로 설정.

## 작업 가이드

상세 규칙, 허용/금지 패턴, 예시: `WeakModelPromptExamples.md` 참조.

## 현재 구현

- `Coin` — `ICollectible` 구현. 트리거 진입 시 `ICollector.AddScore()` 호출 후 자기 파괴.

## 기본 아이템 구조

```
SpriteRenderer + Collider2D(IsTrigger) + Item Script
```

## 변경 범위 제한

- 작업당 변경 파일 최대 3개.
- `Assets/Scripts/Items/` 안에서 해결. Core 구조까지 번지면 중단하고 범위 재조정.

## 리뷰 체크리스트 (리드용)

1. 인터페이스 준수: `ICollectible`/`ICollector`/`IDamageable` 경유 상호작용?
2. Player 타입 미참조: 코드/using에 `PlayerHealth`, `PlayerMotor`, `PlayerController` 없는가?
3. 폴더 격리: `Items/` 안에 집중? Core/Player 수정 최소?
4. Inspector 설정: 코드 없이 배치 + 설정 가능?
5. 과설계 없음: 아이템 1종 추가지, 범용 시스템 구축이 아닌가?
6. 기존 동작 보존: `Coin` 정상 작동?
