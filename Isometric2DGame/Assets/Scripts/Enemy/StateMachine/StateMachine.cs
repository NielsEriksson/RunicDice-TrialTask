
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyDetection))]
[RequireComponent(typeof(EnemyPatrol))]
public class EnemyStateMachine : MonoBehaviour
{
    public EnemyState CurrentState { get; private set; }
    public EnemyPathfinding Pathfinding { get; private set; }
    // States
    public IdleState IdleState { get; private set; }
    public PatrolState PatrolState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }

    // Components
    public EnemyMovement Movement { get; private set; }
    public EnemyDetection Detection { get; private set; }
    public EnemyPatrol Patrol { get; private set; }

    public Animator Animator { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Movement = GetComponent<EnemyMovement>();
        Detection = GetComponent<EnemyDetection>();
        Patrol = GetComponent<EnemyPatrol>();
        Pathfinding = GetComponent<EnemyPathfinding>();
        // Initialize states
        IdleState = new IdleState(this);
        PatrolState = new PatrolState(this);
        ChaseState = new ChaseState(this);
        AttackState = new AttackState(this);
    }

    private void Start()
    {
        ChangeState(IdleState);
    }

    private void Update()
    {
        CurrentState?.Update();
    }

    public void ChangeState(EnemyState newState)
    {
        if (newState == null || CurrentState == newState)
            return;

        CurrentState?.Exit();

        CurrentState = newState;

        CurrentState.Enter();
    }
}
