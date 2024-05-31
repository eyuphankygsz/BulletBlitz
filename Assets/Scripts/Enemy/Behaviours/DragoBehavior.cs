using UnityEngine;

public class DragoBehavior : MonoBehaviour, EnemyBehaviorBase
{
    EnemyController _controller;

    public DragoPatrolState PatrolState { get; private set; }
    public DragoShootState ShootState { get; private set; }
    public DragoHitState HitState { get; private set; }
    public DragoDeadState DeadState { get; private set; }


    public void SetController(EnemyController controller)
    {
        _controller = controller;

        PatrolState = new DragoPatrolState(_controller);
        ShootState = new DragoShootState(_controller);
        HitState = new DragoHitState(_controller);
        DeadState = new DragoDeadState(_controller);
    }

    public void OnDrawGizmos()
    {
        if (_controller == null)
            return;

        Gizmos.DrawLine(_controller.transform.position, _controller.Player.transform.position);
    }
    public EnemyBaseState Hit()
    {
        return HitState;
    }

    public EnemyBaseState StartState()
    {
        return PatrolState;
    }

    public EnemyBaseState Dead()
    {
        return DeadState;
    }
}
