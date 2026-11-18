
using UnityEngine;

public class IdleState : EnemyState
{
    private float idleDuration = 3f;
    private float idleTimer;

    public IdleState(EnemyStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entering Idle");

        idleTimer = 0f;

        stateMachine.Movement.Stop();
    }

    public override void Update()
    {
        // Check if player is detected
        if (stateMachine.Detection.CanSeePlayer)
        {
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }

        // Wait for idle duration
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration &&
            stateMachine.Patrol.HasPatrolPoints)
        {
            stateMachine.ChangeState(stateMachine.PatrolState);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle");
    }
}
