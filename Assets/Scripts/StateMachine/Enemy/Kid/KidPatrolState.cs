using System.Collections;
using UnityEngine;
public class KidPatrolState : EnemyBaseState
{
    EnemyController.States _currentState = EnemyController.States.Patrol;

    bool _walkRight = true;
    float _speed;

    KidBehavior _behavior;
    EnemyStateManager _stateManager;

    float _newDirection;
    public KidPatrolState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        if (_stateManager == null)
            Initialize(enemy);

        _controller.RB.gravityScale = 2f;
        _controller.CurrentState = _currentState;

        if (!_controller.Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            _controller.Animator.SetTrigger("Walk");
    }
    void Initialize(EnemyStateManager enemy)
    {
        _stateManager = enemy;
        _behavior = (KidBehavior)_controller.EnemyBehavior;
        SetStats();
    }

    void SetStats()
    {
        _speed = _controller.EnemyStat.Speed;
    }
    public override void Update()
    {
        if (_controller.CanSeePlayer())
            _controller.TryToChangeState(_behavior.ApproachState, _currentState);

        if (_controller.CurrentState != _currentState)
            return;

        float ySpeed = _controller.IsOnGround() ? 0 : _controller.RB.velocity.y;
        _controller.RB.velocity = new Vector2(_speed * (_walkRight ? 1 : -1), ySpeed);
        LookByVelocity();
        if (_controller.IsWallAhead() && _controller.RB.velocity.y >= 0 && _controller.CanCheckWalls)
            ChangeRotation();

    }
    void LookByVelocity()
    {
        if (_controller.RB.velocity.x < 0)
            _newDirection = -1;
        else if (_controller.RB.velocity.x > 0)
            _newDirection = 1;

        _controller.transform.localScale = new Vector3(Mathf.Abs(_controller.transform.localScale.x) * _newDirection, _controller.transform.localScale.y, _controller.
            transform.localScale.z);
    }
    void ChangeRotation()
    {
        _walkRight = !_walkRight;
        _controller.StartWallCheckCountdown();
    }

    public override void ExitState()
    {

    }

}
