using UnityEngine;

public class KidAttackState: EnemyBaseState
{

    EnemyStateManager _stateManager;
    KidBehavior _behavior;
    EnemyController.States _currentState = EnemyController.States.Attack;

    float _reMoveTimer, _reMoveDefaultTime;

    int _attackCount = 0;
    int _attackCounter = 0;

    int _newDirection = 0;

    float _attackRange;
    public KidAttackState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        if (_stateManager == null)
            Initialize(enemy);

        _controller.CurrentState = _currentState;
        _controller.RB.velocity = Vector3.zero;

        if (!_controller.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            _controller.Animator.SetTrigger("Attack");

    }
    void Initialize(EnemyStateManager enemy)
    {
        _stateManager = enemy;
        _behavior = (KidBehavior)_controller.EnemyBehavior;
        SetStats();
    }
    void SetStats()
    {
        _reMoveDefaultTime = _controller.EnemyStat.ReMoveDefaultTime;
        _reMoveTimer = _reMoveDefaultTime;

        _attackCount = _controller.BulletStat.Speed.Length;
        _attackRange = _controller.EnemyStat.AttackRange;
    }


    public override void Update()
    {
        if (!_controller.CanSeePlayer() || _controller.DistanceBetweenPlayer() > _attackRange)
        {
            if (_reMoveTimer <= 0)
                SwitchToPatrol();
            else
                _reMoveTimer -= Time.deltaTime;

            return;
        }
        else
            _reMoveTimer = _reMoveDefaultTime;

        LookAtPlayer();
        if (_controller.CurrentState == _currentState && _controller.CanAttack)
        {
            _controller.CanAttack = false;
            Attack();
        }
    }

    void Attack()
    {
        RaycastHit2D hit = Physics2D.CircleCast(_controller.AttackPoint.position, _controller.AttackRadius, Vector2.down, 1, GameManager.Instance.PlayerLayer);
        if (hit.collider == null)
            return;

        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Kid"].Clips["Attack"], false);

        hit.collider.GetComponent<PlayerController>().HitWithDamage(_controller.EnemyStat.AttackDamage + 
                                                                    _controller.BulletStat.Damage[_attackCounter],_controller.gameObject);
        if (_attackCount > 1)
            _attackCounter = (_attackCounter + 1) % _attackCount;
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
    void SwitchToPatrol()
    {
        _controller.TryToChangeState(_behavior.PatrolState, _currentState);
        _reMoveTimer = _reMoveDefaultTime;
    }
    public override void ExitState() { }

}
