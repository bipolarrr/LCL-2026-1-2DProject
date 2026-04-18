using UnityEngine;

/// <summary>
/// Rigidbody2D.linearVelocity.x 부호를 따라 좌/우 바라보는 방향을 갱신한다.
/// IFacingProvider 구현 — 소비자(레이저 등)는 구체 클래스를 몰라도 된다.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyFaceMovementDirection : MonoBehaviour, IFacingProvider
{
    public Vector2 FacingDirection { get; private set; } = Vector2.right;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float vx = _rb.linearVelocity.x;
        if (Mathf.Abs(vx) < 0.01f) return;

        FacingDirection = vx > 0f ? Vector2.right : Vector2.left;
    }
}
