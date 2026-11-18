
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private float minPatrolDistance = 2f;
    [SerializeField] private float maxPatrolDistance = 10f;

    private EnemyPathfinding pathfinding;

    public Vector2 CurrentDestination { get; private set; }

    public bool HasDestination { get; private set; }

    private void Awake()
    {
        pathfinding = GetComponent<EnemyPathfinding>();
    }

    public bool ChooseNewDestination()
    {
        HasDestination = pathfinding.TryGetRandomWalkablePoint(
            transform.position,
            minPatrolDistance,
            maxPatrolDistance,
            out Vector2 destination
        );

        if (HasDestination)
        {
            CurrentDestination = destination;
        }

        return HasDestination;
    }
}
