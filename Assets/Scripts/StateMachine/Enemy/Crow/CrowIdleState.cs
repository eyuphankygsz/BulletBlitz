using UnityEngine;

public class CrowIdleState : EnemyBaseState
{
    EnemyController.States _currentState = EnemyController.States.Idle;

    CrowBehavior _behavior;
    EnemyStateManager _stateManager;

    float _attackTimer, _defaultAttackTimer;

    public CrowIdleState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        //if statemanger is null, its the first initializing
        if (_stateManager == null)
            Initialize(enemy);

        _controller.RB.gravityScale = 0;
        _controller.CurrentState = _currentState;
        SetStats();
    }
    void Initialize(EnemyStateManager enemy)
    {
        _stateManager = enemy;
        _behavior = (CrowBehavior)_controller.EnemyBehavior;
    }
    void SetStats()
    {
        _defaultAttackTimer = _controller.EnemyStat.AttackDefaultTime;
        _attackTimer = _defaultAttackTimer;
    }




    public override void Update()
    {
        _controller.RB.velocity = Vector2.zero;
        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0)
            _controller.TryToChangeState(_behavior.AttackState, _currentState);

    }
    public override void ExitState()
    {

    }

}
