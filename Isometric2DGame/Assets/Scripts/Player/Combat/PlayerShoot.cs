
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject arrowPrefab;
    [SerializeField] private float shootCooldown = 0.5f;
    [SerializeField] private Transform shotOrigin;

    [Header("Input Actions")]
    public InputActionReference shootAction;
    public InputActionReference pointerPositionAction;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Camera mainCamera;
    private Vector2 shootingDirection;
    private float nextShootTime;

    void Awake()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        shootAction.action.Enable();
        pointerPositionAction.action.Enable();
    }

    void OnDisable()
    {
        shootAction.action.Disable();
        pointerPositionAction.action.Disable();
    }

    void Update()
    {
        if (shootAction.action.WasPressedThisFrame() && Time.time >= nextShootTime)
        {
            // Play shooting animation
            CalculateDirection();
            animator.SetTrigger("Shoot");
            nextShootTime = Time.time + shootCooldown;
        }
    }

    void Shoot()
    {
        // Calculate arrow rotation
        float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        // Spawn arrow at player position
        GameObject arrow = Instantiate(
            arrowPrefab,
            shotOrigin.position,
            Quaternion.Euler(0f, 0f, angle)
        );

        // Launch arrow
        arrow.GetComponent<ArrowProjectile>().Launch(shootingDirection);
    }
    public void CalculateDirection()
    {
        // Get mouse position
        Vector2 mousePosition = pointerPositionAction.action.ReadValue<Vector2>();

        // Convert screen position to world position
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        Plane playerPlane = new Plane(Vector3.forward, shotOrigin.position);

        if (!playerPlane.Raycast(ray, out float distance))
            return;

        Vector3 targetPosition = ray.GetPoint(distance);

        // Calculate shooting direction
        shootingDirection = (targetPosition - shotOrigin.position).normalized;

        // Flip player depending on shooting direction
        spriteRenderer.flipX = shootingDirection.x < 0;

        
    }
}
