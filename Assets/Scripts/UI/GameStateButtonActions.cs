using UnityEngine;

/// <summary>
/// UI 버튼의 OnClick()에 연결하기 위한 얇은 브리지.
///
/// [UI 담당 사용법]
/// 1. 아무 GameObject에 이 스크립트를 붙인다 (Canvas나 GameManager 등).
/// 2. 버튼의 OnClick() 이벤트에서 이 스크립트의 메서드를 선택한다.
///    예: Resume 버튼 → GameStateButtonActions.Resume()
///
/// 이 스크립트가 하는 일:
/// - GameStateController에 상태 변경을 "요청"만 한다.
/// - Time.timeScale 등을 직접 건드리지 않는다.
/// - 유효하지 않은 전이는 GameStateController가 알아서 거부한다.
/// </summary>
public class GameStateButtonActions : MonoBehaviour
{
    public void Play()
    {
        GameStateController.Instance?.RequestPlay();
    }

    public void Pause()
    {
        GameStateController.Instance?.RequestPause();
    }

    public void Resume()
    {
        GameStateController.Instance?.RequestResume();
    }

    public void Restart()
    {
        GameStateController.Instance?.RequestRestart();
    }
}
