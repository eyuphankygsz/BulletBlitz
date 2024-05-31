using UnityEngine;

public class GhostIdleState : EnemyBaseState
{
    private EnemyController.States _currentState = EnemyController.States.Idle;

    private GhostBehavior _behavior;
    private EnemyStateManager _stateManager;

    private float _attackTimer, _defaultAttackTimer;
    private bool _seen;

    public GhostIdleState(EnemyController controller) : base(controller) { }

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
        _behavior = (GhostBehavior)_controller.EnemyBehavior;
    }
    void SetStats()
    {
        _defaultAttackTimer = _controller.EnemyStat.AttackDefaultTime;
        _attackTimer = _defaultAttackTimer;
    }




    public override void Update()
    {
        if (!_seen)
        {
            _seen = _controller.HasSeenPlayer();
            return;
        }

        _controller.RB.velocity = Vector2.zero;
        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0)
            _controller.TryToChangeState(_behavior.AttackState, _currentState);

    }
    public override void ExitState()
    {

    }

}
