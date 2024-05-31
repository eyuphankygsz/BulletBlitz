using UnityEngine;

public class WitchFlyAttackState : EnemyBaseState
{
    private EnemyController.States _currentState = EnemyController.States.Attack;

    private WitchBehavior _behavior;
    private EnemyStateManager _stateManager;

    private float _maxRange = 8f;
    private float _speed;

    private bool _enteredRange;

    private Vector3 direction;

    private bool _canPlaySound = true;
    private int _attempts;

    public WitchFlyAttackState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        if (_stateManager == null)
            Initialize(enemy);

        if (!_controller.Animator.GetCurrentAnimatorStateInfo(0).IsName("FlyAttack"))
            _controller.Animator.SetTrigger("FlyAttack");

        _enteredRange = false;
        _attempts = 0;
        _controller.CurrentState = _currentState;
        _controller.RB.gravityScale = 0;
        _controller.RB.velocity = Vector2.zero;
        direction = (_controller.Player.position - _controller.transform.position).normalized;
    }
    void Initialize(EnemyStateManager enemy)
    {
        _stateManager = enemy;
        _behavior = (WitchBehavior)_controller.EnemyBehavior;

        SetStats();
    }
    void SetStats()
    {
        _speed = _controller.EnemyStat.Speed;
    }

    public override void Update()
    {
        float xScale = _controller.RB.velocity.x < 0 ? Mathf.Abs(_controller.transform.localScale.x) : _controller.RB.velocity.x > 0 ? -Mathf.Abs(_controller.transform.localScale.x) : _controller.transform.localScale.x;
        _controller.transform.localScale = new Vector3(xScale, _controller.transform.localScale.y, _controller.transform.localScale.z);

        Collider2D collider = _controller.IsPlayerAhead();
        if (collider != null && !GameManager.Instance.SpecialSkill)
        {
            if (!collider.GetComponent<PlayerController>().IsHit)
            {
                if (_canPlaySound)
                    //TODO: Change SFX
                    EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Witch"].Clips["Attack"], false);
                _canPlaySound = false;

                collider.GetComponent<PlayerController>().HitWithDamage(_controller.EnemyStat.AttackDamage, _controller.gameObject);
                _controller.TryToChangeState(_behavior.FallbackState, _currentState);
                return;
            }

        }
        else if (collider == null)
            _canPlaySound = true;

        float distance = Vector2.Distance(_controller.Player.position, _controller.transform.position);
        if (distance <= _maxRange && !_enteredRange)
            _enteredRange = true;
        if(distance > _maxRange && !_enteredRange)
        {
            direction = (_controller.Player.position - _controller.transform.position).normalized;
        }
        if (distance > _maxRange && _enteredRange)
        {
            _attempts++;
            _enteredRange = false;
            if (_attempts == 3)
                _controller.TryToChangeState(_behavior.FallbackState, _currentState);

            direction = (_controller.Player.position - _controller.transform.position).normalized;
        }

        _controller.RB.velocity = direction * _speed;
    }
    public override void ExitState()
    {
    }
}
