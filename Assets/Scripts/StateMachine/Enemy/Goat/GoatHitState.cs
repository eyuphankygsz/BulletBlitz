using UnityEngine;

public class GoatHitState : EnemyBaseState
{
    EnemyController.States _currentState = EnemyController.States.Hit;

    bool _isHitting;
    float _defaultGroundCheckTime = 0.2f;

    GoatBehavior _behavior;
    EnemyStateManager _stateManager;

    public GoatHitState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        //if statemanger is null, its the first initializing
        if (_stateManager == null)
            Initialize(enemy);

        if (_isHitting)
            return;
        else
            _isHitting = true;

        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Goat"].Clips["Hit"], false);
        _controller.Animator.SetTrigger("Hit");

        _controller.CurrentState = _currentState;
        _controller.RB.gravityScale = 2;

        _controller.StartGroundCheckCountdown(_defaultGroundCheckTime);
        Hit();


    }
    void Initialize(EnemyStateManager enemy)
    {
        _stateManager = enemy;
        _behavior = (GoatBehavior)_controller.EnemyBehavior;
    }

    public override void Update()
    {
        LimitPosition();
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
    void LimitPosition()
    {
        Vector3 limitedPos = _controller.transform.position;
        limitedPos.x = Mathf.Clamp(limitedPos.x, _controller.StartPoint.x, _controller.EndPoint.x);
        _controller.transform.position = limitedPos;
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
