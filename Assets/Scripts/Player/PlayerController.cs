using UnityEngine;
using System;

/// <summary>
/// 플레이어 컴포넌트들을 조율하는 상위 컨트롤러.
/// InputReader → Motor 흐름을 연결하고, ICollector(점수)를 구현한다.
///
/// 직접 소유하지 않는 것:
/// - 입력 파싱 → PlayerInputReader
/// - Rigidbody 물리/이동 → PlayerMotor
/// - HP/무적/사망 → PlayerHealth
/// - 상태 이상 → (추후 PlayerStatusController)
/// - 애니메이션 → (추후 PlayerAnimationBridge)
/// </summary>
public class PlayerController : MonoBehaviour, ICollector
{
    private PlayerInputReader inputReader;
    private PlayerMotor motor;

    private int score;

    public int Score => score;
    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        inputReader = GetComponent<PlayerInputReader>();
        motor = GetComponent<PlayerMotor>();
    }

    private void FixedUpdate()
    {
        motor.Move(inputReader.MoveInput);

        if (inputReader.JumpRequested && motor.IsGrounded)
        {
            motor.Jump();
            inputReader.ConsumeJump();
        }
    }

    /// <summary>
    /// ICollector 계약 이행. 아이템이 인터페이스를 통해 호출한다.
    /// </summary>
    void ICollector.AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
        Debug.Log($"Score: {score}");
    }
}
