using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchFallbackState : EnemyBaseState
{
    private EnemyController.States _currentState = EnemyController.States.Patrol;
    private float _minX, _maxX, _minY, _maxY;
    private EnemyStateManager _stateManager;
    private WitchBehavior _behavior;

    private Vector3 direction;

    private float _defaultFallbackTime;
    private float _fallbackTimer;
    public WitchFallbackState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        if (_stateManager == null)
        {
            Initialize(enemy);
        }

        if (!_controller.Animator.GetCurrentAnimatorStateInfo(0).IsName("FlyAttack"))
            _controller.Animator.SetTrigger("FlyAttack");

        _controller.CurrentState = _currentState;
        _controller.RB.gravityScale = 0;
        _controller.RB.velocity = Vector2.zero;

        direction = (_controller.Player.position - _controller.transform.position).normalized;
        direction.x = -direction.x;
        direction.y = Mathf.Abs(direction.y);
        direction.z = 0;
        float xScale = _controller.RB.velocity.x < 0 ? Mathf.Abs(_controller.transform.localScale.x) : _controller.RB.velocity.x > 0 ? -Mathf.Abs(_controller.transform.localScale.x) : _controller.transform.localScale.x;
        _controller.transform.localScale = new Vector3(xScale, _controller.transform.localScale.y, _controller.transform.localScale.z);
        _fallbackTimer = _defaultFallbackTime;
    }

    void Initialize(EnemyStateManager enemy)
    {
        _stateManager = enemy;
        _minX = _controller.MinX;
        _maxX = _controller.MaxX;
        _minY = _controller.MinY;
        _maxY = _controller.MaxY;

        _behavior = (WitchBehavior)_controller.EnemyBehavior;
        _defaultFallbackTime = 1f;
    }
    public override void Update()
    {
        _fallbackTimer -= Time.deltaTime;
        if (_fallbackTimer <= 0)
            _controller.TryToChangeState(_behavior.IdleState, _currentState);

        _controller.RB.velocity = direction * _controller.EnemyStat.Speed;

        float x = _controller.transform.position.x;
        float y = _controller.transform.position.y;
        //float y = Mathf.Clamp(y, _minY, _maxY);

        Vector3 clampedPos = new Vector3(Mathf.Clamp(x, _minX, _maxX), Mathf.Clamp(y, _minY, _maxY), 0);
        _controller.transform.position = clampedPos;

    }

    public override void ExitState()
    {

    }

}
