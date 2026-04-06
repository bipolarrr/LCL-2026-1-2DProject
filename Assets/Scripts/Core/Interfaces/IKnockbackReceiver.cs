using UnityEngine;

/// <summary>
/// 넉백을 받을 수 있는 오브젝트가 구현한다.
/// 적이나 기믹이 플레이어를 밀어낼 때 이 인터페이스를 통해 소통한다.
/// </summary>
public interface IKnockbackReceiver
{
    void ApplyKnockback(Vector2 force);
}
