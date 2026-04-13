using UnityEngine;

/// <summary>
/// 접촉 시 IDamageable에게 데미지와 넉백을 주는 컴포넌트.
/// 이동 로직을 소유하지 않는다 — 어떤 적이든 이 컴포넌트를 붙이면 접촉 데미지가 동작한다.
///
/// [몹 담당 확장 가이드]
/// - contactDamage, knockbackForce를 Inspector에서 조절.
/// - 새로운 적에게도 이 컴포넌트를 그대로 붙이면 된다.
/// </summary>
public class EnemyContactDamage : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private int contactDamage = 1;
    [SerializeField] private float knockbackForce = 7f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable == null) return;

        damageable.TakeDamage(contactDamage);

        // Knockback direction: away from enemy + upward bias (0.5f) so target arcs upward
        Vector2 knockDir = (collision.transform.position - transform.position).normalized;
        knockDir.y = 0.5f;
        damageable.ApplyKnockback(knockDir.normalized * knockbackForce);
    }
}
