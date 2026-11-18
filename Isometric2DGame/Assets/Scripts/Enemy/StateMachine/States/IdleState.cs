
using UnityEngine;

public class IdleState : EnemyState
{
    private const float IdleDuration = 3f;
    private float idleTimer;

    public IdleState(EnemyStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entering Idle");

        idleTimer = 0f;

        stateMachine.Pathfinding.StopFollowing();
    }

    public override void Update()
    {
        // Check if player is visible
        if (stateMachine.Detection.CanSeePlayer)
        {
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }

        // Wait before starting patrol
        idleTimer += Time.deltaTime;

        if (idleTimer >= IdleDuration)
        {
            stateMachine.ChangeState(stateMachine.PatrolState);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle");
    }
}
