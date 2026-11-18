
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
        // Transition to attack
        if (stateMachine.Detection.IsPlayerInAttackRange)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        // Transition to patrol
        if (!stateMachine.Detection.CanSeePlayer)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        // Chase player
        stateMachine.Movement.MoveTowards(
            stateMachine.Detection.Player.position,
            stateMachine.Movement.chaseSpeed
        );
    }

    public override void Exit()
    {
        stateMachine.Movement.Stop();
    }
}
