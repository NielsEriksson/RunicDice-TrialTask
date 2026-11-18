
using UnityEngine;

public class AttackState : EnemyState
{
    private float attackCooldown = 1.5f;
    private float nextAttackTime;

    public AttackState(EnemyStateMachine stateMachine)
        : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entering Attack");

        stateMachine.Movement.Stop();

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
            Debug.Log("Attack");

            stateMachine.Animator.SetTrigger("Attack");

            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Attack");
    }
}
