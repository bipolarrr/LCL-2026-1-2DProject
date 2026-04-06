/// <summary>
/// 아이템을 수집할 수 있는 오브젝트(주로 플레이어)가 구현하는 인터페이스.
/// 아이템 쪽에서 플레이어 구체 클래스를 참조하지 않도록 분리.
/// </summary>
public interface ICollector
{
    void AddScore(int amount);
}
