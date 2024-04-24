using UnityEngine;

public class GoatShootState : EnemyBaseState
{

    private EnemyStateManager _stateManager;
    private GoatBehavior _behavior;
    private EnemyController.States _currentState = EnemyController.States.Attack;

    private float _reMoveTimer, _reMoveDefaultTime, _attackTimer;

    private int _currentSkill = 0, _bulletCount = 0, _bulletCounter = 0;
    private WeaponStat _bulletStat;
    private Bullet[] _bullets;
    private float _newDirection = 0;

    private float _attackRange;

    private Bullet _bulletPrefab;
    public GoatShootState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        if (_stateManager == null)
            Initialize(enemy);
        _controller.CurrentState = _currentState;
        _controller.RB.velocity = Vector3.zero;
        _controller.Animator.SetTrigger("Idle");
    }
    void Initialize(EnemyStateManager enemy)
    {
        _stateManager = enemy;
        _behavior = (GoatBehavior)_controller.EnemyBehavior;


        _controller.Animator.SetFloat("AttackSpeed", _controller.EnemyStat.AttackSpeed);
        _bulletPrefab = _controller.BulletStat.Projectile.GetComponent<Bullet>();
        SetStats();
    }
    void SetStats()
    {
        _reMoveDefaultTime = _controller.EnemyStat.ReMoveDefaultTime;
        _reMoveTimer = _reMoveDefaultTime;

        _attackTimer = _controller.BulletStat.Timer[0];

        _bulletStat = _controller.BulletStat;
        _bullets = new Bullet[_bulletStat.MaxBullet];

        _attackRange = _controller.EnemyStat.AttackRange;
    }


    public override void Update()
    {
        if (_controller.DistanceBetweenPlayer() > _attackRange)
        {
            if (_reMoveTimer <= 0)
            {
                SwitchToPatrol();
            }
            else
                _reMoveTimer -= Time.deltaTime;
        }
        else
            _reMoveTimer = _reMoveDefaultTime;

        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0)
        {
            _controller.Animator.SetTrigger("Attack");
            Shoot();

        }


        LookAtPlayer();

        Vector3 difference = _controller.Player.position - _controller.LookPoint.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        _controller.LookPoint.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ + _controller.LookOffset);
    }

    void Shoot()
    {
        SetRotation();
        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Goat"].Clips["Shoot"] ,false);
        if (_bulletCount != _bulletStat.MaxBullet)
            _bullets[_bulletCount++] = GameObject.Instantiate(_bulletPrefab, _controller.AttackPoint.position, Quaternion.identity);

        if (_bulletStat.Speed.Length > 1)
            _currentSkill = _bulletCounter % _bulletStat.Speed.Length;

        _bullets[_bulletCounter % _bulletStat.MaxBullet].Setup(
            _controller.AttackPoint.position,
            _controller.Player.position,
            _controller.EnemyStat.AttackDamage + _bulletStat.Damage[_currentSkill],
            _controller.EnemyStat.BulletSpeed + _bulletStat.Speed[_currentSkill],
            "Goat",
            canActivateSkill: false,
            otherBullets: null
            );
        _bullets[_bulletCounter++ % _bulletStat.MaxBullet].gameObject.SetActive(true);
        _attackTimer = _controller.BulletStat.Timer[(_currentSkill + 1) % _bulletStat.Timer.Length];
    }

    void SetRotation()
    {
        Vector3 difference = _controller.Player.position - _controller.AttackPoint.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        _controller.AttackPoint.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }

    void SwitchToPatrol()
    {
        _controller.TryToChangeState(_behavior.PatrolState, _currentState);
        _reMoveTimer = _reMoveDefaultTime;
    }
    void LookAtPlayer()
    {
        Vector2 direction = _controller.Player.position - _controller.transform.position;
        if (direction.x < 0)
            _newDirection = 1;
        else if (direction.x > 0)
            _newDirection = -1;

        _controller.transform.localScale = new Vector3(Mathf.Abs(_controller.transform.localScale.x) * _newDirection, _controller.transform.localScale.y, _controller.
            transform.localScale.z);
    }
    public override void ExitState()
    {

    }

}
