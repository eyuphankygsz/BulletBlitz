using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchHitState : EnemyBaseState
{
    private EnemyController.States _currentState = EnemyController.States.Patrol;
    private EnemyController.States _oldState;

    private EnemyStateManager _stateManager;
    private WitchBehavior _behavior;

    float _backToIdleTimer, _defaultBackToIdleTime = 1;
    bool _isHit;
    public WitchHitState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        if (_stateManager == null)
            Initialize(enemy);

        if (_isHit)
            return;

        _controller.Animator.SetTrigger("Hit");

        _controller.CurrentState = _currentState;
        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Witch"].Clips["Hit"], false);
        _isHit = true;
        _backToIdleTimer = _defaultBackToIdleTime;

        _controller.RB.gravityScale = 0;
        _controller.RB.velocity = Vector2.zero;

        Vector2 direction = (_controller.HitObject.transform.position - _controller.transform.position).normalized;
        _controller.RB.AddForce(direction * 80);

    }
    void Initialize(EnemyStateManager enemy)
    {
        _stateManager = enemy;
        _behavior = (WitchBehavior)_controller.EnemyBehavior;
    }

    public override void Update()
    {
        float xScale = _controller.RB.velocity.x < 0 ? Mathf.Abs(_controller.transform.localScale.x) : _controller.RB.velocity.x > 0 ? -Mathf.Abs(_controller.transform.localScale.x) : _controller.transform.localScale.x;
        _controller.transform.localScale = new Vector3(xScale, _controller.transform.localScale.y, _controller.transform.localScale.z);
        BackToIdle();
    }
    void BackToIdle()
    {
        _backToIdleTimer -= Time.deltaTime;
        if (_backToIdleTimer <= 0)
            _controller.TryToChangeState(_behavior.FallbackState, _currentState);
    }

    public override void ExitState()
    {
        _isHit = false;
    }
}
