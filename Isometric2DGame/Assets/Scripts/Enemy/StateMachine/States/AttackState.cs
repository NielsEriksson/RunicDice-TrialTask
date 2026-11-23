
using UnityEngine;

public class AttackState : EnemyState
{
    private float nextAttackTime;

    public AttackState(EnemyStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entering Attack");

        stateMachine.Pathfinding.StopFollowing();

        // Allow the first attack immediately
        nextAttackTime = 0f;
    }

    public override void Update()
    {
        // Check if player is still within attack range
        if (!stateMachine.Detection.IsPlayerInAttackRange)
        {
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }

        // Check attack cooldown
        if (Time.time >= nextAttackTime)
        {
           Debug.Log("attacking");
           stateMachine.Attack.Attack();
           nextAttackTime = Time.time + stateMachine.Attack.attackCooldown;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Attack");
    }
}
