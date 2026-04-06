/// <summary>
/// 외부 트리거에 의해 작동을 시작할 수 있는 오브젝트가 구현한다.
/// 문, 함정, 보스 트리거, 이동 발판 등 맵 기믹의 공통 진입점.
/// </summary>
public interface IActivatable
{
    void Activate();
}
