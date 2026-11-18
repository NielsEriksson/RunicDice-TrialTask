
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;

    private int currentIndex;

    public bool HasPatrolPoints =>
        patrolPoints != null &&
        patrolPoints.Length > 0 &&
        patrolPoints[currentIndex] != null;

    public Vector2 CurrentDestination
    {
        get
        {
            if (!HasPatrolPoints)
                return transform.position;

            return patrolPoints[currentIndex].position;
        }
    }

    public void UpdateDestination()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        currentIndex = (currentIndex + 1) % patrolPoints.Length;

        // Skip missing patrol points
        int attempts = 0;

        while (patrolPoints[currentIndex] == null &&
               attempts < patrolPoints.Length)
        {
            currentIndex = (currentIndex + 1) % patrolPoints.Length;
            attempts++;
        }
    }
}
