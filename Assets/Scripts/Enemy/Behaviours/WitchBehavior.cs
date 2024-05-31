using UnityEngine;

public class WitchBehavior : MonoBehaviour, EnemyBehaviorBase
{
    EnemyController _controller;

    public WitchIdleState IdleState { get; private set; }
    public WitchFallbackState FallbackState { get; private set; }
    public WitchBulletAttackState ShootState { get; private set; }
    public WitchFlyAttackState FlyAttack { get; private set; }
    public WitchHitState HitState { get; private set; }
    public DragoDeadState DeadState { get; private set; }


    public void SetController(EnemyController controller)
    {
        _controller = controller;

        IdleState = new WitchIdleState(_controller);
        FallbackState = new WitchFallbackState(_controller);
        FlyAttack = new WitchFlyAttackState(_controller);
        ShootState = new WitchBulletAttackState(_controller);
        HitState = new WitchHitState(_controller);
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
        return IdleState;
    }

    public EnemyBaseState Dead()
    {
        return DeadState;
    }
}
