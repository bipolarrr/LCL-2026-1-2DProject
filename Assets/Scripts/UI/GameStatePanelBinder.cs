using UnityEngine;

/// <summary>
/// 특정 GameState일 때 연결된 패널을 보여주고, 아닐 때 숨긴다.
///
/// [UI 담당 사용법]
/// 1. Canvas 아래에 패널(예: PausePanel, GameOverPanel)을 만든다.
/// 2. 빈 GameObject(또는 패널 자신)에 이 스크립트를 붙인다.
/// 3. Inspector에서:
///    - Target State → 표시하고 싶은 상태 선택 (예: Paused)
///    - Panel → 해당 패널 오브젝트 드래그
/// 4. 패널은 기본적으로 비활성 상태(SetActive false)로 두면 된다.
///
/// 상태별로 이 스크립트를 하나씩 붙이면 각 패널이 자동으로 켜지고 꺼진다.
/// </summary>
public class GameStatePanelBinder : MonoBehaviour
{
    [SerializeField] private GameState targetState;
    [SerializeField] private GameObject panel;

    private void Start()
    {
        if (GameStateController.Instance != null)
        {
            GameStateController.Instance.OnStateChanged += HandleStateChanged;
            UpdatePanel(GameStateController.Instance.CurrentState);
        }
    }

    private void OnDestroy()
    {
        if (GameStateController.Instance != null)
        {
            GameStateController.Instance.OnStateChanged -= HandleStateChanged;
        }
    }

    private void HandleStateChanged(GameState previous, GameState current)
    {
        UpdatePanel(current);
    }

    private void UpdatePanel(GameState current)
    {
        if (panel != null)
        {
            panel.SetActive(current == targetState);
        }
    }
}
