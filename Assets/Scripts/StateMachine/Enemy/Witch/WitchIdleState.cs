using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchIdleState : EnemyBaseState
{
    private EnemyController.States _currentState = EnemyController.States.Idle;

    private WitchBehavior _behavior;
    private EnemyStateManager _stateManager;

    private float _attackTimer, _defaultAttackTimer;

    private int _choosenAttack;

    private float _minX, _maxX, _minY, _maxY;
    private bool _goRight;
    public WitchIdleState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        //if statemanger is null, its the first initializing
        if (_stateManager == null)
            Initialize(enemy);

        _controller.Animator.SetTrigger("Float");

        _controller.RB.gravityScale = 0;
        _controller.RB.velocity = Vector2.zero;

        _controller.CurrentState = _currentState;
        SetStats();

        Debug.Log("Idle");
    }

    void Initialize(EnemyStateManager enemy)
    {
        _minX = _controller.MinX;
        _maxX = _controller.MaxX;
        _minY = _controller.MinY;
        _maxY = _controller.MaxY;

        _stateManager = enemy;
        _behavior = (WitchBehavior)_controller.EnemyBehavior;
    }
    void SetStats()
    {
        _defaultAttackTimer = _controller.EnemyStat.AttackDefaultTime;
        _attackTimer = _defaultAttackTimer;
        _choosenAttack = Random.Range(0, 2);
    }




    public override void Update()
    {
        _controller.RB.velocity = Vector2.zero;
        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0)
            ChangeAttack();

        Move();

    }

    private void Move()
    {

        if(_goRight)
        {
            _controller.RB.velocity = Vector3.right * _controller.EnemyStat.Speed;
            if (_controller.transform.position.x > _maxX)
                _goRight = false;
        }
        else
        {
            _controller.RB.velocity = Vector3.left * _controller.EnemyStat.Speed;
            if (_controller.transform.position.x < _minX)
                _goRight = true;
        }
        float x = _controller.transform.position.x;
        float y = _controller.transform.position.y;

        Vector3 clampedPos = new Vector3(Mathf.Clamp(x, _minX, _maxX), Mathf.Clamp(y, _minY, _maxY), 0);
        _controller.transform.position = clampedPos;
        SetRotation();
    }

    void SetRotation()
    {
        float xScale = _controller.RB.velocity.x < 0 ? Mathf.Abs(_controller.transform.localScale.x) : _controller.RB.velocity.x > 0 ? -Mathf.Abs(_controller.transform.localScale.x) : _controller.transform.localScale.x;
        _controller.transform.localScale = new Vector3(xScale, _controller.transform.localScale.y, _controller.transform.localScale.z);
    }

    private void ChangeAttack()
    {
        if (_choosenAttack == 0)
            _controller.TryToChangeState(_behavior.FlyAttack, _currentState);
        else
            _controller.TryToChangeState(_behavior.ShootState, _currentState);
    }
    public override void ExitState()
    {

    }
}
