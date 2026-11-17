using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference moveActionToUse;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float startAcceleration;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    private Animator animator;
    private Vector2 input;
    private bool isFacingBack;
    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        // Read player input
        input = moveActionToUse.action.ReadValue<Vector2>();

        input = Vector2.ClampMagnitude(input, 1f);

        // Calculate isometric direction
        Vector2 isometricDirection = new Vector2(
            input.x - input.y,
            (input.x + input.y) * 0.5f
        );

        // Check whether the player is moving
        bool isMoving = input.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            // Determine whether the character faces the back
            isFacingBack = isometricDirection.y > 0;

            // Flip sprite horizontally
            if (Mathf.Abs(isometricDirection.x) > 0.01f)
            {
                spriteRenderer.flipX = isometricDirection.x < 0;
            }
        }

        // Update animator
        animator.SetBool("IsMoving", isMoving);
        animator.SetBool("IsBack", isFacingBack);
    }
    private void FixedUpdate()
    {
        Vector2 isometricDirection = new Vector2(
       input.x - input.y,
       (input.x + input.y) * 0.5f
   );

        isometricDirection = Vector2.ClampMagnitude(
            isometricDirection, 1f
        );

        Vector2 movement = isometricDirection * movementSpeed;

        rigidBody.MovePosition(
            rigidBody.position + movement * Time.fixedDeltaTime
        );
    }
}
