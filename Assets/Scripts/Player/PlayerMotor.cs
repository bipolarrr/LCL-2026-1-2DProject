using UnityEngine;

/// <summary>
/// Rigidbody2D 기반 이동, 점프, 넉백 적용만 담당한다.
/// 입력을 직접 읽지 않고, HP나 상태 이상을 관리하지 않는다.
///
/// ═════════════════════════════════════════════════════
/// [필수 에디터 설정] — 스크립트만으로는 해결되지 않는 사항
/// ═════════════════════════════════════════════════════
/// 1. Physics Material 2D 생성 (Friction 0, Bounciness 0) 후
///    Player Collider2D.Material 슬롯에 할당.
///    → 벽/지면 마찰로 플레이어가 체공하는 현상 방지.
/// 2. Project Settings → Physics 2D → Layer Collision Matrix 에서
///    'Player'와 'Enemy' 레이어 간 물리 충돌 체크 해제.
///    → 적과의 물리적 엉킴 제거. 피격 판정은 Trigger 히트박스 +
///       IDamageable 경로로만 처리한다.
/// ═════════════════════════════════════════════════════
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerMotor : MonoBehaviour, IKnockbackReceiver
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float[] jumpForceMultipliers = { 1f, 0.5f };

    [Header("Ground / Wall Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float groundCastDistance = 0.05f;
    [SerializeField] private float wallCastDistance = 0.05f;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 0.15f;

    private Rigidbody2D rb;
    private Collider2D col;
    private ContactFilter2D groundFilter;
    private ContactFilter2D wallFilter;
    private readonly RaycastHit2D[] castResults = new RaycastHit2D[1];

    private int jumpCount;
    private bool wasGrounded;
    private bool isKnockbacked;
    private float knockbackTimer;

    public bool IsGrounded { get; private set; }
    public bool CanJump => jumpCount < maxJumpCount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        groundFilter = new ContactFilter2D { useTriggers = false };
        groundFilter.SetLayerMask(groundLayer);
        groundFilter.useLayerMask = true;

        wallFilter = new ContactFilter2D { useTriggers = false };
        wallFilter.SetLayerMask(wallLayer);
        wallFilter.useLayerMask = true;
    }

    private void FixedUpdate()
    {
        IsGrounded = col.Cast(Vector2.down, groundFilter, castResults, groundCastDistance) > 0;

        if (IsGrounded && !wasGrounded)
        {
            jumpCount = 0;
        }
        wasGrounded = IsGrounded;

        if (isKnockbacked)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockbacked = false;
            }
        }
    }

    public void Move(float direction)
    {
        if (isKnockbacked) return;

        float vx = direction * moveSpeed;

        if (!IsGrounded && direction != 0f && IsTouchingWall(direction))
        {
            vx = 0f;
        }

        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }

    public bool Jump()
    {
        if (!CanJump)
        {
            return false;
        }

        float jumpForceMultiplier = GetJumpForceMultiplier(jumpCount);
        float targetJumpVelocityY = jumpForce * jumpForceMultiplier;
        float nextVelocityY = Mathf.Max(rb.linearVelocity.y, targetJumpVelocityY);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, nextVelocityY);
        jumpCount++;

        return true;
    }

    private bool IsTouchingWall(float direction)
    {
        Vector2 castDir = new Vector2(Mathf.Sign(direction), 0f);
        return col.Cast(castDir, wallFilter, castResults, wallCastDistance) > 0;
    }

    private float GetJumpForceMultiplier(int jumpIndex)
    {
        if (jumpForceMultipliers == null || jumpForceMultipliers.Length == 0)
        {
            return 1f;
        }

        int clampedIndex = Mathf.Clamp(jumpIndex, 0, jumpForceMultipliers.Length - 1);
        return jumpForceMultipliers[clampedIndex];
    }

    /// <summary>
    /// IKnockbackReceiver 계약 이행. 외부에서 전달된 넉백을 적용하고,
    /// 짧은 시간 동안 Move 입력을 무시해 linearVelocity 덮어쓰기로
    /// 넉백이 상쇄되는 것을 방지한다.
    /// </summary>
    void IKnockbackReceiver.ApplyKnockback(Vector2 force)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
        isKnockbacked = true;
        knockbackTimer = knockbackDuration;
    }
}
