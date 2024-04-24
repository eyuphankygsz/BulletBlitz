using UnityEngine;

public class GoatPatrolState : EnemyBaseState
{
    EnemyController.States _currentState = EnemyController.States.Patrol;

    bool _walkRight = true;
    float _startPoint, _endPoint;
    float _speed;
    
    GoatBehavior _behavior;
    EnemyStateManager _stateManager;

    float _newDirection = 1;
    float _attackRange;
    public GoatPatrolState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        //if statemanger is null, its the first initializing
        if (_stateManager == null)
            Initialize(enemy);

        if(!_controller.Animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"))
            _controller.Animator.SetTrigger("Walk");
        _controller.RB.gravityScale = 2f;
        _controller.CurrentState = _currentState;
    }
    void Initialize(EnemyStateManager enemy)
    {
        _stateManager = enemy;
        _behavior = (GoatBehavior)_controller.EnemyBehavior;
        DefinePatrolPoints();
        SetStats();
    }
    void DefinePatrolPoints()
    {
        _startPoint = _controller.StartPoint.x;
        _endPoint = _controller.EndPoint.x;
    }
    void SetStats()
    {
        _speed = _controller.EnemyStat.Speed;
        _attackRange = _controller.EnemyStat.AttackRange;
    }




    public override void Update()
    {
        LookToWay();
        if (_controller.DistanceBetweenPlayer() <= _attackRange)
            _controller.TryToChangeState(_behavior.ShootState, _currentState);



        if (_controller.CurrentState != _currentState)
            return;

        _controller.RB.velocity = new Vector2(_speed * (_walkRight ? 1 : -1), _controller.RB.velocity.y);
        if ((!_walkRight && _controller.transform.position.x <= _startPoint) || (_walkRight && _controller.transform.position.x >= _endPoint))
            ChangeRotation();

    }
    void ChangeRotation()
    {
        _walkRight = !_walkRight;
    }
    void LookToWay()
    {
        float direction = _controller.RB.velocity.x;
        if (direction < 0)
            _newDirection = 1;
        else if (direction > 0)
            _newDirection = -1;

        _controller.transform.localScale = new Vector3(Mathf.Abs(_controller.transform.localScale.x) * _newDirection, _controller.transform.localScale.y, _controller.
            transform.localScale.z);
    }

    public override void ExitState()
    {

    }

}
