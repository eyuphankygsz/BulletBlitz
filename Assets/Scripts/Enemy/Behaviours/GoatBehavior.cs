using UnityEngine;

public class GoatBehavior : MonoBehaviour, EnemyBehaviorBase
{
    EnemyController _controller;

    public GoatPatrolState PatrolState { get; private set; }
    public GoatShootState ShootState { get; private set; }
    public GoatHitState HitState { get; private set; }
    public GoatDeadState DeadState { get; private set; }


    public void SetController(EnemyController controller)
    {
        _controller = controller;

        PatrolState = new GoatPatrolState(_controller);
        ShootState = new GoatShootState(_controller);
        HitState = new GoatHitState(_controller);
        DeadState = new GoatDeadState(_controller);
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
