using UnityEngine;

public class KidApproachState : EnemyBaseState
{
    EnemyController.States _currentState = EnemyController.States.Approach;
    float _speed;

    KidBehavior _behavior;
    EnemyStateManager _stateManager;

    float _newDirection = 0;

    bool _moveAway;
    float _moveAwayTimer;
    float horizontalSpeed = 0;

    float _attackRange;
    float _followRange;
    public KidApproachState(EnemyController controller) : base(controller) { }

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
        _attackRange = _controller.EnemyStat.AttackRange;
        _followRange = _controller.EnemyStat.FollowRange;
    }
    public override void Update()
    {
        MoveAwayTimer();
        CheckEnemies();

        if (_moveAway)
            return;

        MovementController();

        LookAtPlayer();
        if (_controller.DistanceBetweenPlayer() < _attackRange)
            _controller.TryToChangeState(_behavior.AttackState, _currentState);
    }
    void LookAtPlayer()
    {
        Vector2 direction = _controller.Player.position - _controller.transform.position;
        if (direction.x < 0)
            _newDirection = -1;
        else if (direction.x > 0)
            _newDirection = 1;

        _controller.transform.localScale = new Vector3(Mathf.Abs(_controller.transform.localScale.x) * _newDirection, _controller.transform.localScale.y, _controller.
            transform.localScale.z);
    }

    void CheckEnemies()
    {
        if (_controller.IsEnemyAhead("Kid"))
        {
            if (_moveAway)
                return;

            _moveAway = true;
            _moveAwayTimer = 0.2f;

            Vector2 direction = (_controller.EnemyObject.transform.position - _controller.transform.position).normalized;
            horizontalSpeed = direction.x < 0 ? 1 : direction.x > 0 ? -1 : 0;
            horizontalSpeed *= _speed;
            _controller.RB.velocity = new Vector2(horizontalSpeed, _controller.RB.velocity.y);
            return;
        }

    }
    void MoveAwayTimer()
    {
        if (!_moveAway)
            return;

        _moveAwayTimer -= Time.deltaTime;
        if (_moveAwayTimer <= 0)
            _moveAway = false;
    }

    void MovementController()
    {
        horizontalSpeed = 0;
        CheckWalls();
        CheckPlayer();
    }
    void CheckWalls()
    {
        if (!_controller.IsWallAhead())
            if (_controller.DistanceBetweenPlayer() > _attackRange)
                horizontalSpeed = _controller.Player.position.x - _controller.transform.position.x < 0 ? -1 : 1;

            horizontalSpeed *= _speed;
    }
    void CheckPlayer()
    {
        if (_controller.CanSeePlayer() && _controller.DistanceBetweenPlayer() <= _followRange)
            _controller.RB.velocity = new Vector2(horizontalSpeed, _controller.RB.velocity.y);
        else
            _controller.TryToChangeState(_behavior.PatrolState, _currentState);
    }
    public override void ExitState()
    {
    }

}
