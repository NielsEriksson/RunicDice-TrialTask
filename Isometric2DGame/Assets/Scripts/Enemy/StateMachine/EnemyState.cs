
public abstract class EnemyState
{
    protected EnemyStateMachine stateMachine;

    protected EnemyState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
