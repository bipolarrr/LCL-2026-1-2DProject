using UnityEngine;
using System;

/// <summary>
/// 현재/최대 HP, 피격 처리, 무적 시간, 사망 판정만 담당한다.
/// 입력, 이동, 점수를 관리하지 않는다.
/// ApplyKnockback은 PlayerMotor에 위임한다.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    [Header("Invincibility")]
    [SerializeField] private float invincibleDuration = 1.5f;

    private int currentHealth;
    private bool isInvincible;
    private float invincibleTimer;
    private SpriteRenderer spriteRenderer;
    private PlayerMotor motor;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    /// <summary>(현재 HP, 최대 HP)</summary>
    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        motor = GetComponent<PlayerMotor>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (!isInvincible) return;

        invincibleTimer -= Time.deltaTime;
        spriteRenderer.enabled = Mathf.Sin(Time.time * 20f) > 0f;

        if (invincibleTimer <= 0f)
        {
            isInvincible = false;
            spriteRenderer.enabled = true;
        }
    }

    void IDamageable.TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        isInvincible = true;
        invincibleTimer = invincibleDuration;

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    /// <summary>
    /// IDamageable 계약 이행. 실제 물리 적용은 PlayerMotor에 위임한다.
    /// </summary>
    void IDamageable.ApplyKnockback(Vector2 force)
    {
        if (motor != null)
        {
            ((IKnockbackReceiver)motor).ApplyKnockback(force);
        }
    }
}
