using UnityEngine;

public class GhostAttackState : EnemyBaseState
{
    private EnemyController.States _currentState = EnemyController.States.Attack;

    private GhostBehavior _behavior;
    private EnemyStateManager _stateManager;

    private float _maxRange = 2f;
    private float _speed;
    private float _backToIdleTimer, _defaultBackToIdleTime;

    private bool _attacked;

    private Vector3 direction;

    private bool _moveAway;
    private float _moveAwayTimer;

    private bool _canPlaySound = true;

    private float _minX, _maxX, _minY, _maxY;
    public GhostAttackState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        if (_stateManager == null)
            Initialize(enemy);

        _controller.CurrentState = _currentState;
    }
    void Initialize(EnemyStateManager enemy)
    {
        _stateManager = enemy;
        _behavior = (GhostBehavior)_controller.EnemyBehavior;

        _minX = _controller.MinX;
        _maxX = _controller.MaxX;
        _minY = _controller.MinY;
        _maxY = _controller.MaxY;

        SetStats();
    }
    void SetStats()
    {
        _speed = _controller.EnemyStat.Speed;
        _defaultBackToIdleTime = _controller.EnemyStat.ReMoveDefaultTime;
        _backToIdleTimer = _defaultBackToIdleTime;
        _moveAwayTimer = 1;
    }

    public override void Update()
    {
        float horizontal;

        float xScale = _controller.RB.velocity.x < 0 ? Mathf.Abs(_controller.transform.localScale.x) : _controller.RB.velocity.x > 0 ? -Mathf.Abs(_controller.transform.localScale.x) : _controller.transform.localScale.x;
        _controller.transform.localScale = new Vector3(xScale, _controller.transform.localScale.y, _controller.transform.localScale.z);
        Collider2D collider = _controller.IsPlayerAhead();
        if (_controller.IsEnemyAhead("Ghost"))
        {
            if (!_moveAway)
            {
                _moveAway = true;
                _moveAwayTimer = 1;
                _controller.RB.velocity = Vector2.zero;
            }

            Vector2 direction = (_controller.EnemyObject.transform.position - _controller.transform.position).normalized;
            _controller.RB.AddForce(-direction * 100);
            horizontal = 0;
            return;
        }
        else
        {
            if (_moveAway)
            {
                if (collider != null && !GameManager.Instance.SpecialSkill)
                {
                    if (_canPlaySound)
                        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Ghost"].Clips["Attack"], false);
                    _canPlaySound = false;

                    collider.GetComponent<PlayerController>().HitWithDamage(_controller.EnemyStat.AttackDamage, _controller.gameObject);
                }
                else if (collider == null)
                    _canPlaySound = true;

                MoveAwayTimer();
                if (_controller.transform.position.x > _maxX + 5
                    || _controller.transform.position.y > _maxY + 5
                    || _controller.transform.position.x < _minX - 5
                    || _controller.transform.position.y < _minY - 5)
                    _controller.transform.position =
                                          new Vector3(Random.Range(_minX, _maxX), Random.Range(0, 2) == 0 ? _minY : _maxY);
                return;
            }
            horizontal = _speed;
        }


        if (collider != null && !GameManager.Instance.SpecialSkill)
        {
            if (_canPlaySound)
                EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Ghost"].Clips["Attack"], false);
            _canPlaySound = false;

            collider.GetComponent<PlayerController>().HitWithDamage(_controller.EnemyStat.AttackDamage, _controller.gameObject);
        }
        else if (collider == null)
            _canPlaySound = true;

        float distance = Vector2.Distance(_controller.Player.position, _controller.transform.position);
        if (distance > _maxRange && !_attacked)
            direction = (_controller.Player.position - _controller.transform.position).normalized;
        else
        {
            if (!_attacked)
            {
                if (distance <= 0.4f)
                    _attacked = true;
            }
            else
            {
                direction = (_controller.Player.position - _controller.transform.position).normalized;
                direction.x = -direction.x;
                direction.y = Mathf.Abs(direction.y);
                BackToIdleTimer();
            }
        }

        float x = _controller.transform.position.x;
        float y = _controller.transform.position.y;
        //float y = Mathf.Clamp(y, _minY, _maxY);

        _controller.RB.velocity = direction * horizontal;


    }
    void BackToIdleTimer()
    {
        _backToIdleTimer -= Time.deltaTime;

        if (_backToIdleTimer <= 0)
        {
            _attacked = false;
            _controller.TryToChangeState(_behavior.IdleState, _currentState);
        }
    }

    void MoveAwayTimer()
    {
        _moveAwayTimer -= Time.deltaTime;
        if (_moveAwayTimer <= 0)
            _moveAway = false;
    }
    public override void ExitState()
    {
        _backToIdleTimer = _defaultBackToIdleTime;
        _attacked = false;
    }
}
