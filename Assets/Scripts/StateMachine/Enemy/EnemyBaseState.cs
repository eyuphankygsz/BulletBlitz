using UnityEngine;

public abstract class EnemyBaseState
{
    protected EnemyController _controller;

    public EnemyBaseState(EnemyController controller)
    {
        _controller = controller;
    }

    public abstract void EnterState(EnemyStateManager enemy);
    public abstract void Update();
    public abstract void ExitState();
}
