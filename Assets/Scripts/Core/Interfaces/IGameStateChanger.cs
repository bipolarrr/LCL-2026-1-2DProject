/// <summary>
/// 상태 변경을 "요청"하는 인터페이스.
/// 직접 enum을 대입하지 않고, 의미 있는 메서드를 통해서만 전이를 요청한다.
/// 유효하지 않은 전이는 GameStateController가 거부한다.
/// </summary>
public interface IGameStateChanger
{
    void RequestPlay();
    void RequestPause();
    void RequestResume();
    void RequestGameOver();
    void RequestStageClear();
    void RequestRestart();
}
