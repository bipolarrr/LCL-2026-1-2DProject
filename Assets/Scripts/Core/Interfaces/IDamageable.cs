using UnityEngine;

/// <summary>
/// 피격 가능한 오브젝트가 구현하는 인터페이스.
/// 플레이어, 적, 파괴 가능 오브젝트 등에서 사용.
/// </summary>
public interface IDamageable
{
    void TakeDamage(int amount);
    void ApplyKnockback(Vector2 force);
}
