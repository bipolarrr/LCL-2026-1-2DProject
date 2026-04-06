using UnityEngine;

/// <summary>
/// 코인 아이템. 플레이어가 접촉하면 점수를 올린다.
/// ICollector 인터페이스를 통해 점수를 전달하므로 Player 구체 클래스를 참조하지 않는다.
///
/// [맵/아이템 담당 가이드]
/// - 씬에서 빈 오브젝트에 이 스크립트를 붙이고, SpriteRenderer + CircleCollider2D(IsTrigger)를 추가.
/// - scoreValue를 Inspector에서 조절하면 코인 가치를 바꿀 수 있다.
/// - 새로운 아이템을 만들 때는 이 스크립트를 참고해서 ICollectible을 구현하면 된다.
/// </summary>
public class Coin : MonoBehaviour, ICollectible
{
    [SerializeField] private int scoreValue = 1;

    /// <summary>
    /// ICollectible 계약 이행. 수집자에게 점수를 전달한다.
    /// </summary>
    void ICollectible.Collect(GameObject collector)
    {
        ICollector col = collector.GetComponent<ICollector>();
        if (col != null)
        {
            col.AddScore(scoreValue);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ICollector>() != null)
        {
            ((ICollectible)this).Collect(other.gameObject);
        }
    }
}
