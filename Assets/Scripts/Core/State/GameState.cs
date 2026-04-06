/// <summary>
/// 전역 게임 흐름 상태.
/// 옵션 창, 인벤토리 등 UI 로컬 상태는 여기에 넣지 않는다.
/// </summary>
public enum GameState
{
    Boot,
    Playing,
    Paused,
    GameOver,
    StageClear
}
