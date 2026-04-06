using UnityEngine;

/// <summary>
/// Rigidbody2D 기반 이동, 점프, 넉백 적용만 담당한다.
/// 입력을 직접 읽지 않고, HP나 상태 이상을 관리하지 않는다.
/// </summary>

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour, IKnockbackReceiver
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(
            groundCheck.position, groundCheckRadius, groundLayer);
    }

    public void Move(float direction)
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    /// <summary>
    /// IKnockbackReceiver 계약 이행. 외부에서 전달된 넉백을 적용한다.
    /// </summary>
    void IKnockbackReceiver.ApplyKnockback(Vector2 force)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
