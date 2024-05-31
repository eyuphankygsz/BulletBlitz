using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchBulletAttackState : EnemyBaseState
{
    private EnemyController.States _currentState = EnemyController.States.Attack;
    private WitchBehavior _behavior;

    private EnemyStateManager _stateManager;

    private float _defaultShootTime, _shootTimer;
    private int _currentSkill = 0, _bulletCount = 0, _bulletCounter = 0;

    private WeaponStat _bulletStat;
    private Bullet[] _bullets;
    private float _newDirection = 0;

    private Bullet _bulletPrefab;

    List<Action> _shootActions;

    int _shooterLine = 0;
    int _shootCounter = 0;

    private float _minX, _maxX, _minY, _maxY;

    public WitchBulletAttackState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        if (_stateManager == null)
            Initialize(enemy);

        _controller.Animator.SetTrigger("BulletAttack");
        _controller.CurrentState = _currentState;

        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Witch"].Clips["Hit"], false);
        _controller.SetInvulnerable(true);

        _controller.RB.gravityScale = 0;
        _controller.RB.velocity = Vector3.zero;

        _shootCounter = 0;
    }
    public override void Update()
    {
        LookAtPlayer();
        ShootTimer();

        float x = _controller.transform.position.x;
        float y = _controller.transform.position.y;
        //float y = Mathf.Clamp(y, _minY, _maxY);

        Vector3 clampedPos = new Vector3(Mathf.Clamp(x, _minX, _maxX), Mathf.Clamp(y, _minY, _maxY), 0);
        _controller.transform.position = clampedPos;
    }

    private void ShootTimer()
    {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0)
            Shoot();
    }
    private void Shoot()
    {
        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Witch"].Clips["Shoot"], false);
        _shootCounter++;
        if (_shootCounter >= 5)
            _controller.TryToChangeState(_behavior.FallbackState, _currentState);
        else
            _shootActions[_shooterLine++ % 2].Invoke();
    }

    private void Initialize(EnemyStateManager enemy)
    {
        _stateManager = enemy;
        _behavior = (WitchBehavior)_controller.EnemyBehavior;

        _bulletPrefab = _controller.BulletStat.Projectile.GetComponent<Bullet>();
        _shootActions = new List<Action>
        {
            Shoot1,
            Shoot2
        };
        SetStats();
        CreateAmmo();

        _minX = _controller.MinX;
        _maxX = _controller.MaxX;
        _minY = _controller.MinY;
        _maxY = _controller.MaxY;
    }
    private void SetStats()
    {
        _bulletStat = _controller.BulletStat;

        _defaultShootTime = _bulletStat.Timer[0];
        _shootTimer = _defaultShootTime;
        _bullets = new Bullet[_bulletStat.MaxBullet];
    }

    void Shoot1()
    {
        Vector3 currentBulletPos;
        int shootCount = 6;
        float degree = 360 / shootCount;

        for (int i = 0; i < shootCount; i++)
        {
            currentBulletPos = _controller.transform.position + new Vector3(2 * Mathf.Cos(degree * i * Mathf.Deg2Rad), 2 * Mathf.Sin(degree * i * Mathf.Deg2Rad), 0);


            _bullets[_bulletCounter % _bulletStat.MaxBullet].Setup(
                currentBulletPos,
                _controller.transform.position + new Vector3(4 * Mathf.Cos(degree * i * Mathf.Deg2Rad), 4 * Mathf.Sin(degree * i * Mathf.Deg2Rad), 0),
                _controller.EnemyStat.AttackDamage + _bulletStat.Damage[_currentSkill],
                _controller.EnemyStat.BulletSpeed + _bulletStat.Speed[_currentSkill],
                "Witch",
                canActivateSkill: false,
                otherBullets: null
                );
            _bullets[_bulletCounter++ % _bulletStat.MaxBullet].gameObject.SetActive(true);
            _shootTimer = _controller.BulletStat.Timer[(_currentSkill + 1) % _bulletStat.Timer.Length];

        }

    }
    void Shoot2()
    {
        Vector3 currentBulletPos;
        int shootCount = 10;
        float degree = 360 / shootCount;

        for (int i = 0; i < shootCount; i++)
        {
            currentBulletPos = _controller.transform.position + new Vector3(2 * Mathf.Cos(degree * i * Mathf.Deg2Rad), 2 * Mathf.Sin(degree * i * Mathf.Deg2Rad), 0);

            _bullets[_bulletCounter % _bulletStat.MaxBullet].Setup(
                currentBulletPos,
                _controller.transform.position + new Vector3(4 * Mathf.Cos(degree * i * Mathf.Deg2Rad), 4 * Mathf.Sin(degree * i * Mathf.Deg2Rad), 0),
                _controller.EnemyStat.AttackDamage + _bulletStat.Damage[_currentSkill],
                _controller.EnemyStat.BulletSpeed + _bulletStat.Speed[_currentSkill] + 2,
                "Witch",
                canActivateSkill: false,
                otherBullets: null
                );
            _bullets[_bulletCounter++ % _bulletStat.MaxBullet].gameObject.SetActive(true);
            _shootTimer = _controller.BulletStat.Timer[(_currentSkill + 1) % _bulletStat.Timer.Length];

        }

    }
    private void LookAtPlayer()
    {
        Vector2 direction = _controller.Player.position - _controller.transform.position;
        _newDirection = direction.x > 0 ? -1 : direction.x < 0 ? 1 : _newDirection;

        _controller.transform.localScale = new Vector3(Mathf.Abs(_controller.transform.localScale.x) * _newDirection, _controller.transform.localScale.y, _controller.
            transform.localScale.z);
    }
    void CreateAmmo()
    {
        for (int i = 0; i < _bulletStat.MaxBullet; i++)
            _bullets[_bulletCount++] = GameObject.Instantiate(_bulletPrefab, new Vector3(-10, -10, 0), Quaternion.identity);
    }
    public override void ExitState()
    {
        _controller.SetInvulnerable(false);
    }

}
