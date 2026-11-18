
using UnityEngine;

public class PatrolState : EnemyState
{
    public PatrolState(EnemyStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entering Patrol");
    }

    public override void Update()
    {
        // Check if player is detected
        if (stateMachine.Detection.CanSeePlayer)
        {
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }

        // Get current patrol destination
        Vector2 target = stateMachine.Patrol.CurrentDestination;

        // Move towards destination
        stateMachine.Movement.MoveTowards(
            target,
            stateMachine.Movement.patrolSpeed
        );

        // Check if destination has been reached
        if (Vector2.Distance(
            stateMachine.transform.position,
            target) < 0.2f)
        {
            stateMachine.Patrol.UpdateDestination();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Patrol");

        stateMachine.Movement.Stop();
    }
}
