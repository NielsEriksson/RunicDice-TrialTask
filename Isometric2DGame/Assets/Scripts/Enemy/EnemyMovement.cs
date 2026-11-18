
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] public float patrolSpeed = 2f;
    [SerializeField] public float chaseSpeed = 3f;

    [Header("Animation")]
    [SerializeField] private float movementThreshold = 0.01f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector2 movementTarget;
    private Vector2 movementDirection;

    private float currentSpeed;
    private bool isMoving;

    private bool isFacingBack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isMoving)
            return;

        Vector2 previousPosition = rb.position;

        Vector2 newPosition = Vector2.MoveTowards(
            previousPosition,
            movementTarget,
            currentSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);

        movementDirection = (newPosition - previousPosition).normalized;
    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        bool currentlyMoving = movementDirection.sqrMagnitude >
                               movementThreshold * movementThreshold;

        if (currentlyMoving)
        {
            // Determine whether the enemy faces the back
            isFacingBack = movementDirection.y > 0;

            // Flip sprite horizontally
            if (Mathf.Abs(movementDirection.x) > 0.01f)
            {
                spriteRenderer.flipX = movementDirection.x < 0;
            }
        }

        animator.SetBool("IsMoving", currentlyMoving);
        animator.SetBool("IsBack", isFacingBack);
    }

    public void MoveTowards(Vector2 target, float speed)
    {
        movementTarget = target;
        currentSpeed = speed;
        isMoving = true;
    }

    public void Stop()
    {
        isMoving = false;
        movementDirection = Vector2.zero;
    }
}
