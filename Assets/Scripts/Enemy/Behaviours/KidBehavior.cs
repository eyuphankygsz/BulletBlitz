using UnityEngine;

public class KidBehavior : MonoBehaviour, EnemyBehaviorBase
{
    EnemyController _controller;

    public KidPatrolState PatrolState { get; private set; }
    public KidApproachState ApproachState { get; private set; }
    public KidAttackState AttackState { get; private set; }
    public KidHitState HitState { get; private set; }
    public KidDeadState DeadState { get; private set; }

    public void SetController(EnemyController controller)
    {
        _controller = controller;

        PatrolState = new KidPatrolState(_controller);
        ApproachState = new KidApproachState(_controller);
        AttackState = new KidAttackState(_controller);
        HitState = new KidHitState(_controller);
        DeadState = new KidDeadState(_controller);
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
