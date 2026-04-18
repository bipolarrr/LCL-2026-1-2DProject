using UnityEngine;

/// <summary>
/// 오브젝트의 현재 바라보는 방향을 제공한다.
/// 레이저, 투사체 발사, 조준선 표시 등 "방향이 필요한" 로직이
/// 구체 클래스(속도 기반, 입력 기반, 타겟 추적 등)를 몰라도 되도록 분리한다.
///
/// FacingDirection은 정규화된 벡터여야 한다.
/// </summary>
public interface IFacingProvider
{
    Vector2 FacingDirection { get; }
}
