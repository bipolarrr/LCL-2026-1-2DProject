using UnityEngine;

/// <summary>
/// 플레이어가 밟으면 위로 크게 튀어오르는 발판.
/// IKnockbackReceiver 인터페이스를 통해 힘을 전달하므로 Player 내부 필드를 직접 건드리지 않는다.
///
/// [맵 담당 사용법]
/// 1. 빈 오브젝트에 SpriteRenderer + BoxCollider2D(IsTrigger) 를 붙인다.
/// 2. 이 스크립트를 추가한다.
/// 3. Inspector에서 bounceForce를 조절한다.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class BouncePad : MonoBehaviour
{
    [SerializeField] private float bounceForce = 20f;

    private void Reset()
    {
        if (TryGetComponent(out Collider2D col))
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IKnockbackReceiver receiver = other.GetComponent<IKnockbackReceiver>();
        if (receiver == null) return;

        receiver.ApplyKnockback(Vector2.up * bounceForce);
    }
}
