using UnityEngine;

public class GhostHitState : EnemyBaseState
{
    EnemyController.States _currentState = EnemyController.States.Attack;

    GhostBehavior _behavior;
    EnemyStateManager _stateManager;

    float _backToIdleTimer, _defaultBackToIdleTime = 1;
    bool _isHit;
    public GhostHitState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        if (_stateManager == null)
            Initialize(enemy);
        if (_isHit)
            return;
        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Ghost"].Clips["Hit"], false);
        _controller.CurrentState = _currentState;
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
        _behavior = (GhostBehavior)_controller.EnemyBehavior;
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
            _controller.TryToChangeState(_behavior.IdleState, _currentState);
    }

    public override void ExitState() 
    {
        _isHit = false;
    }

}
