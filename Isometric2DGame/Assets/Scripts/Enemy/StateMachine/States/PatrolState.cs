
using UnityEngine;

public class PatrolState : EnemyState
{
    public PatrolState(EnemyStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entering Patrol");

        stateMachine.Patrol.ChooseNewDestination();
    }

    public override void Update()
    {
        // Check if the enemy can see the player
        if (stateMachine.Detection.CanSeePlayer)
        {
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }

        // No valid destination found
        if (!stateMachine.Patrol.HasDestination)
        {
            stateMachine.Patrol.ChooseNewDestination();
            return;
        }

        Vector2 target = stateMachine.Patrol.CurrentDestination;

        // Move towards the destination using pathfinding
        stateMachine.Pathfinding.SetDestination(
            target,
            stateMachine.Movement.patrolSpeed
        );

        // Check whether the enemy has arrived
        if (Vector2.Distance(
            stateMachine.transform.position,
            target) < 0.2f)
        {
            stateMachine.Pathfinding.StopFollowing();

            stateMachine.Patrol.ChooseNewDestination();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Patrol");

        stateMachine.Pathfinding.StopFollowing();
    }
}
