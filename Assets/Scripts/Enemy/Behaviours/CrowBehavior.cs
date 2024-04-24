using UnityEditor;
using UnityEngine;

public class CrowBehavior : MonoBehaviour, EnemyBehaviorBase
{
    EnemyController _controller;

    public CrowIdleState IdleState { get; private set; }
    public CrowAttackState AttackState { get; private set; }
    public CrowHitState HitState { get; private set; }
    public CrowDeadState DeadState { get; private set; }

    public EnemyBaseState StartState()
    {
        return IdleState;
    }

    public void SetController(EnemyController controller)
    {
        _controller = controller;

        IdleState = new CrowIdleState(controller);
        AttackState = new CrowAttackState(controller);
        HitState = new CrowHitState(controller);
        DeadState = new CrowDeadState(controller);
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
