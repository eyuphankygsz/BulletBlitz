using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidHitState : EnemyBaseState
{
    EnemyController.States _currentState = EnemyController.States.Hit;

    bool _isHitting;
    float _defaultGroundCheckTime = 0.2f;

    KidBehavior _behavior;
    EnemyStateManager _stateManager;

    public KidHitState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        if (_stateManager == null)
            Initialize(enemy);

        if (_isHitting)
            return;
        else
            _isHitting = true;

        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Kid"].Clips["Hit"], false);

        _controller.CurrentState = _currentState;
        _controller.RB.gravityScale = 2;

        if (!_controller.Animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            _controller.Animator.SetTrigger("Hit");

        _controller.StartGroundCheckCountdown(_defaultGroundCheckTime);
        Hit();
    }
    void Initialize(EnemyStateManager enemy)
    {
        _stateManager = enemy;
        _behavior = (KidBehavior)_controller.EnemyBehavior;
    }

    public override void Update()
    {
        CheckGround();
    }

    void Hit()
    {
        _controller.RB.velocity = Vector2.zero;

        Vector2 direction = new Vector2(
            (_stateManager.transform.position.x - _controller.HitObject.transform.position.x >= 0 ? 1 : -1)
            , 1);
        _controller.RB.AddForce(new Vector2(direction.x * 200, direction.y * 300));
    }

    void CheckGround()
    {
        if (_controller.IsOnGround() && _controller.CanCheckGround)
        {
            _stateManager.SwitchState(_behavior.PatrolState);
            return;
        }
    }

    public override void ExitState()
    {
        _isHitting = false;
    }

}
