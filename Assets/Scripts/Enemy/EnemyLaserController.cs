using UnityEngine;

/// <summary>
/// 공격 on/off 루프와 플레이어 감지 범위만 담당하는 타이밍 컨트롤러.
/// 구체 공격 구현(레이저/미사일/폭탄 등)은 IAttackBehavior 로 분리되어 있어
/// 같은 GameObject에 IAttackBehavior 를 구현한 어떤 공격 컴포넌트가 붙어 있어도 동작한다.
/// </summary>
public class EnemyLaserController : MonoBehaviour
{
    [Header("타이밍")]
    [SerializeField] private float laserOnDuration = 2f;
    [SerializeField] private float laserOffDuration = 1.5f;

    [Header("감지")]
    [SerializeField] private float detectionRange = 15f;

    private float _timer;
    private bool _laserOn;
    private Transform _player;
    private IAttackBehavior _attack;

    private void Start()
    {
        _attack = GetComponent<IAttackBehavior>();
        if (_attack == null)
        {
            Debug.LogWarning(
                $"[{nameof(EnemyLaserController)}] IAttackBehavior 구현 컴포넌트가 없습니다. 공격이 동작하지 않습니다. ({name})",
                this);
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;

        _laserOn = false;
        _timer = laserOffDuration;
        _attack?.EndAttack();
    }

    private void Update()
    {
        if (!IsPlayerInRange())
        {
            if (_laserOn)
            {
                _laserOn = false;
                _attack?.EndAttack();
            }
            _timer = laserOffDuration;
            return;
        }

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _laserOn = !_laserOn;
            _timer = _laserOn ? laserOnDuration : laserOffDuration;
            if (_laserOn) _attack?.BeginAttack();
            else _attack?.EndAttack();
        }
    }

    private bool IsPlayerInRange()
    {
        if (_player == null) return false;
        if (detectionRange <= 0f) return true;
        return Vector2.Distance(transform.position, _player.position) <= detectionRange;
    }

    private void OnDrawGizmosSelected()
    {
        if (detectionRange > 0f)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}
