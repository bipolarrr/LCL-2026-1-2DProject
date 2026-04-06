using UnityEngine;
using System;

/// <summary>
/// 전역 게임 흐름(Boot → Playing → Paused / GameOver / StageClear)을 관리한다.
///
/// 책임:
/// - 현재 상태 보관
/// - 전이 유효성 검사
/// - 부작용 적용 (Time.timeScale 등)
/// - 상태 변경 이벤트 발행
///
/// 씬에 빈 GameObject 하나 만들고 이 스크립트를 붙이면 된다.
/// autoPlay = true(기본값)면 Start 시 자동으로 Playing 상태로 전환된다.
/// </summary>
public class GameStateController : MonoBehaviour, IGameStateReader, IGameStateChanger
{
    public static GameStateController Instance { get; private set; }

    [Header("Initial Settings")]
    [SerializeField] private GameState initialState = GameState.Boot;
    [SerializeField] private bool autoPlay = true;

    private GameState currentState;

    // --- IGameStateReader ---
    public GameState CurrentState => currentState;
    public event Action<GameState, GameState> OnStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[GameState] Duplicate GameStateController destroyed.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentState = initialState;
    }

    private void Start()
    {
        // PlayerHealth.OnDeath → 자동 GameOver 전이
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnDeath += HandlePlayerDeath;
        }

        if (autoPlay && currentState == GameState.Boot)
        {
            RequestPlay();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            Time.timeScale = 1f;
        }
    }

    // --- IGameStateChanger ---

    public void RequestPlay()
    {
        TryTransition(GameState.Playing);
    }

    public void RequestPause()
    {
        TryTransition(GameState.Paused);
    }

    public void RequestResume()
    {
        TryTransition(GameState.Playing);
    }

    public void RequestGameOver()
    {
        TryTransition(GameState.GameOver);
    }

    public void RequestStageClear()
    {
        TryTransition(GameState.StageClear);
    }

    public void RequestRestart()
    {
        TryTransition(GameState.Boot);
    }

    // --- Internal ---

    private bool IsValidTransition(GameState from, GameState to)
    {
        return (from, to) switch
        {
            (GameState.Boot,       GameState.Playing)    => true,
            (GameState.Playing,    GameState.Paused)     => true,
            (GameState.Playing,    GameState.GameOver)   => true,
            (GameState.Playing,    GameState.StageClear) => true,
            (GameState.Paused,     GameState.Playing)    => true,   // resume
            (GameState.GameOver,   GameState.Boot)       => true,   // restart
            (GameState.StageClear, GameState.Boot)       => true,   // restart
            _ => false
        };
    }

    private void TryTransition(GameState to)
    {
        if (currentState == to) return;

        if (!IsValidTransition(currentState, to))
        {
            Debug.LogWarning($"[GameState] Invalid transition: {currentState} -> {to}");
            return;
        }

        GameState previous = currentState;
        currentState = to;

        ApplySideEffects(to);
        OnStateChanged?.Invoke(previous, currentState);
        Debug.Log($"[GameState] {previous} -> {currentState}");
    }

    private void ApplySideEffects(GameState state)
    {
        Time.timeScale = (state == GameState.Paused) ? 0f : 1f;
    }

    private void HandlePlayerDeath()
    {
        RequestGameOver();
    }
}
