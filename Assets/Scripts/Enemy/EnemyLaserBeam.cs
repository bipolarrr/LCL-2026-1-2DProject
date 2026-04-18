using UnityEngine;

/// <summary>
/// 차징→발사 2단계 레이저 공격 구현.
/// IAttackBehavior 를 통해 상위 컨트롤러가 구체 타입을 모르고도 개시/취소한다.
/// 방향은 IFacingProvider 로 주입받으며, 없으면 transform.right 로 폴백한다.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class EnemyLaserBeam : MonoBehaviour, IAttackBehavior
{
    [Header("데미지")]
    [SerializeField] private float damagePerSecond = 10f;

    [Header("충돌")]
    [SerializeField] private LayerMask wallLayers;
    [SerializeField] private LayerMask playerLayer = ~0;

    [Header("외형")]
    [SerializeField] private float maxLaserWidth = 0.15f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject hitEffectPrefab;

    [Header("차징")]
    [SerializeField] private float chargeDuration = 1.5f;
    [SerializeField] private float chargeFlickerSpeed = 20f;
    [SerializeField] private float chargeMinWidth = 0.01f;
    [SerializeField] private float chargeMaxWidth = 0.05f;

    private enum State { Idle, Charging, Firing }

    private LineRenderer _lr;
    private GameObject _hitEffectInstance;
    private IFacingProvider _facing;
    private State _state;
    private float _chargeTimer;
    private Vector2 _laserEnd;

    private void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        _facing = GetComponent<IFacingProvider>();
        _lr.positionCount = 2;
        _lr.useWorldSpace = true;
        _lr.enabled = false;

        if (hitEffectPrefab != null)
        {
            _hitEffectInstance = Instantiate(hitEffectPrefab);
            _hitEffectInstance.SetActive(false);
        }
    }

    private void Update()
    {
        if (_state == State.Idle) return;

        UpdateLaserLine();

        if (_state == State.Charging)
        {
            float t = (Mathf.Sin(Time.time * chargeFlickerSpeed) + 1f) * 0.5f;
            float w = Mathf.Lerp(chargeMinWidth, chargeMaxWidth, t);
            _lr.startWidth = w;
            _lr.endWidth = w;

            _chargeTimer -= Time.deltaTime;
            if (_chargeTimer <= 0f)
                EnterFiring();
        }
        else
        {
            _lr.startWidth = maxLaserWidth;
            _lr.endWidth = maxLaserWidth;
            DealDamage();
        }
    }

    public void BeginAttack()
    {
        _state = State.Charging;
        _chargeTimer = chargeDuration;
        _lr.enabled = true;
    }

    public void EndAttack()
    {
        _state = State.Idle;
        _lr.enabled = false;
        if (_hitEffectInstance != null)
            _hitEffectInstance.SetActive(false);
    }

    private void EnterFiring()
    {
        _state = State.Firing;
    }

    private void UpdateLaserLine()
    {
        Vector2 origin = firePoint != null ? (Vector2)firePoint.position : (Vector2)transform.position;
        Vector2 dir = _facing != null ? _facing.FacingDirection : (Vector2)transform.right;

        RaycastHit2D wallHit = Physics2D.Raycast(origin, dir, Mathf.Infinity, wallLayers);
        _laserEnd = wallHit.collider != null ? wallHit.point : origin + dir * 100f;

        _lr.SetPosition(0, origin);
        _lr.SetPosition(1, _laserEnd);

        if (_hitEffectInstance != null)
        {
            bool showEffect = _state == State.Firing && wallHit.collider != null;
            _hitEffectInstance.SetActive(showEffect);
            if (showEffect)
                _hitEffectInstance.transform.position = _laserEnd;
        }
    }

    private void DealDamage()
    {
        Vector2 origin = firePoint != null ? (Vector2)firePoint.position : (Vector2)transform.position;
        Vector2 dir = _facing != null ? _facing.FacingDirection : (Vector2)transform.right;
        float length = Vector2.Distance(origin, _laserEnd);

        Vector2 center = origin + dir * (length * 0.5f);
        Vector2 size = new Vector2(length, maxLaserWidth * 2f);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, angle, playerLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(Mathf.RoundToInt(damagePerSecond * Time.deltaTime));
                break;
            }
        }
    }

    private void OnDestroy()
    {
        if (_hitEffectInstance != null)
            Destroy(_hitEffectInstance);
    }
}
