using UnityEngine;

/// <summary>
/// 수집 가능한 오브젝트(코인, 아이템 등)가 구현하는 인터페이스.
/// </summary>
public interface ICollectible
{
    void Collect(GameObject collector);
}
