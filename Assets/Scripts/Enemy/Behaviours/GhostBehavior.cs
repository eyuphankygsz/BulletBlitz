using UnityEditor;
using UnityEngine;

public class GhostBehavior : MonoBehaviour, EnemyBehaviorBase
{
    EnemyController _controller;

    public GhostIdleState IdleState { get; private set; }
    public GhostAttackState AttackState { get; private set; }
    public GhostHitState HitState { get; private set; }
    public GhostDeadState DeadState { get; private set; }

    public EnemyBaseState StartState()
    {
        return IdleState;
    }

    public void SetController(EnemyController controller)
    {
        _controller = controller;

        IdleState = new GhostIdleState(controller);
        AttackState = new GhostAttackState(controller);
        HitState = new GhostHitState(controller);
        DeadState = new GhostDeadState(controller);
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

    public EnemyBaseState Dead()
    {
        return DeadState;
    }
}
