
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float attackRange = 2f;

    [Header("Line Of Sight")]
    [SerializeField] private LayerMask obstacleLayer;

    private Transform player;

    public bool IsPlayerDetected => player != null;

    public bool CanSeePlayer { get; private set; }

    public bool IsPlayerInAttackRange =>
        CanSeePlayer &&
        Vector2.Distance(transform.position, player.position) <= attackRange;

    public Transform Player => player;

    private void Update()
    {
        if (player == null)
        {
            CanSeePlayer = false;
            return;
        }

        CheckLineOfSight();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        player = other.transform;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (player == null || other.transform != player)
            return;

        player = null;
        CanSeePlayer = false;
    }

    private void CheckLineOfSight()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            direction,
            distance,
            obstacleLayer
        );

        CanSeePlayer = hit.collider == null;
    }

    private void OnDrawGizmos()
    {
        if (player == null)
            return;

        Gizmos.color = CanSeePlayer ? Color.green : Color.red;

        Gizmos.DrawLine(transform.position, player.position);
    }
}
