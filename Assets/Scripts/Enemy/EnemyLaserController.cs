using UnityEngine;

public class EnemyLaserController : MonoBehaviour
{
    [Header("레이저")]
    [SerializeField] private EnemyLaserBeam laserBeam;

    [Header("타이밍")]
    [SerializeField] private float laserOnDuration = 2f;
    [SerializeField] private float laserOffDuration = 1.5f;

    [Header("감지")]
    [SerializeField] private float detectionRange = 15f;

    private float _timer;
    private bool _laserOn;
    private Transform _player;

    private void Start()
    {
        if (laserBeam == null)
            laserBeam = GetComponent<EnemyLaserBeam>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;

        _laserOn = false;
        _timer = laserOffDuration;
        laserBeam?.Deactivate();
    }

    private void Update()
    {
        if (!IsPlayerInRange())
        {
            if (_laserOn)
            {
                _laserOn = false;
                laserBeam?.Deactivate();
            }
            _timer = laserOffDuration;
            return;
        }

        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _laserOn = !_laserOn;
            _timer = _laserOn ? laserOnDuration : laserOffDuration;
            if (_laserOn) laserBeam?.Activate();
            else laserBeam?.Deactivate();
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
