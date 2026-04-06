using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 입력 상태만 읽어서 노출한다.
/// 이동, 데미지, 상태 이상 등의 로직은 일절 없다.
///
/// Inspector에서 moveActionRef, jumpActionRef에
/// InputSystem_Actions 에셋의 Player/Move, Player/Jump를 연결한다.
/// DemoSceneBuilder가 자동으로 연결해 준다.
/// </summary>
public class PlayerInputReader : MonoBehaviour
{
    [Header("Input Action References")]
    [SerializeField] private InputActionReference moveActionRef;
    [SerializeField] private InputActionReference jumpActionRef;

    public float MoveInput { get; private set; }
    public bool JumpRequested { get; private set; }

    private InputAction moveAction;
    private InputAction jumpAction;

    private void Awake()
    {
        moveAction = moveActionRef.action;
        jumpAction = jumpActionRef.action;
    }

    private void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        jumpAction.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        jumpAction.performed -= OnJumpPerformed;
        jumpAction.Disable();
        moveAction.Disable();
    }

    private void Update()
    {
        MoveInput = moveAction.ReadValue<Vector2>().x;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        JumpRequested = true;
    }

    /// <summary>점프 입력을 소비한 뒤 호출한다.</summary>
    public void ConsumeJump()
    {
        JumpRequested = false;
    }
}
