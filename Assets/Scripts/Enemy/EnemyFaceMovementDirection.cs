using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyFaceMovementDirection : MonoBehaviour
{
    public Vector2 FacingDirection { get; private set; } = Vector2.right;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float vx = _rb.linearVelocity.x;
        if (Mathf.Abs(vx) < 0.01f) return;

        FacingDirection = vx > 0f ? Vector2.right : Vector2.left;
    }
}
