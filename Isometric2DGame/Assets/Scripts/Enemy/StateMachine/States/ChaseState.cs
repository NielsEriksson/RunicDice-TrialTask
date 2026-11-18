
public class ChaseState : EnemyState
{
    public ChaseState(EnemyStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        UnityEngine.Debug.Log("Entering Chase");
    }

    public override void Update()
    {
        if (stateMachine.Detection.IsPlayerInAttackRange)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        if (!stateMachine.Detection.CanSeePlayer)
        {
            stateMachine.ChangeState(stateMachine.PatrolState);
            return;
        }

        stateMachine.Pathfinding.SetDestination(
            stateMachine.Detection.Player.position,
            stateMachine.Movement.chaseSpeed
        );
    }

    public override void Exit()
    {
        stateMachine.Movement.Stop();
    }
}
