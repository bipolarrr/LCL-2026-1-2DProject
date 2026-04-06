using UnityEngine;

/// <summary>
/// 좌우 순찰 이동만 담당한다.
/// 데미지 처리, 사망 등은 다른 컴포넌트가 담당한다.
///
/// [몹 담당 확장 가이드]
/// - 다른 이동 패턴(추적, 점프, 비행)은 별도 스크립트로 만들 것.
/// - patrolSpeed, patrolDistance는 Inspector에서 조절.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float patrolDistance = 3f;

    private Rigidbody2D rb;
    private Vector2 startPosition;
    private int direction = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(direction * patrolSpeed, rb.linearVelocity.y);

        float distFromStart = transform.position.x - startPosition.x;
        if (Mathf.Abs(distFromStart) >= patrolDistance)
        {
            direction = distFromStart > 0 ? -1 : 1;
        }
    }
}
