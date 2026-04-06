using UnityEngine;

/// <summary>
/// 2D Trigger Collider에 부착하여 플레이어 진입 시 연결된 대상들을 활성화한다.
/// Inspector에서 MonoBehaviour를 연결하고, 그중 IActivatable 구현체만 호출한다.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PlayerTriggeredActivator : MonoBehaviour
{
    [Tooltip("플레이어 태그")]
    [SerializeField] private string playerTag = "Player";

    [Tooltip("true면 최초 1회만 작동")]
    [SerializeField] private bool oneShot;

    [Tooltip("활성화할 대상들 (IActivatable 구현 필요)")]
    [SerializeField] private MonoBehaviour[] targets;

    private bool triggered;

    private void Reset()
    {
        if (TryGetComponent(out Collider2D col))
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered && oneShot) return;
        if (!other.CompareTag(playerTag)) return;

        triggered = true;
        ActivateTargets();
    }

    private void ActivateTargets()
    {
        if (targets == null || targets.Length == 0)
        {
            return;
        }

        for (int i = 0; i < targets.Length; i++)
        {
            MonoBehaviour target = targets[i];
            if (target == null)
            {
                Debug.LogWarning($"[PlayerTriggeredActivator] {gameObject.name}: targets[{i}] is null", this);
                continue;
            }

            if (target is IActivatable activatable)
            {
                activatable.Activate();
                continue;
            }

            Debug.LogWarning(
                $"[PlayerTriggeredActivator] {gameObject.name}: targets[{i}] ({target.GetType().Name}) does not implement IActivatable",
                target);
        }
    }
}
