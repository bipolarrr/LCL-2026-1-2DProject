using System;

/// <summary>
/// 현재 게임 상태를 읽고, 상태 변경 이벤트를 구독할 수 있는 인터페이스.
/// UI, 오디오, 카메라 등 "상태에 반응하는" 시스템이 사용한다.
/// </summary>
public interface IGameStateReader
{
    GameState CurrentState { get; }

    /// <summary>(이전 상태, 새 상태)</summary>
    event Action<GameState, GameState> OnStateChanged;
}
