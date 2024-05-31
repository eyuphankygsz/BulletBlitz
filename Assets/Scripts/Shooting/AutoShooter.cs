using EditorAttributes;
using System.Collections.Generic;
using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    private List<GameObject> _enemies = null;
    [SerializeField] private Transform _closestEnemy;
    [SerializeField] private Transform _lookPoint;
    private Transform _firePoint;
    private Bullet _bulletPrefab;

    private float _currentShootTimer;
    private bool canAim;

    WeaponStat _weaponStat;
    Weapon _weapon;

    public WeaponStat WeaponStat { get { return _weaponStat; } set { _weaponStat = value; } }

    [SerializeField, ColorField(GUIColor.Red)] Bullet[] _bullets;

    private int _currentSkill = 0;
    private int _bulletCounter;
    [SerializeField] private float _offset;
    private PlayerController _controller;
    private bool _isShooting;
    [SerializeField] private Transform _afterEnemiesTransform;

    
    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }
    void Start()
    {
        WeaponManager.Setup(this);

        canAim = true;
        _currentShootTimer = _weaponStat.Timer[0];

        CreateBullets();
        Debug.Log(WeaponStat);
        _controller.SetStats();
    }

    // Update is called once per frame
    public void UpdateShooter()
    {
        if (!canAim)
            return;
        SelectEnemy();
        ShootTimer();
    }

    void SelectEnemy()
    {
        if (_enemies == null || _enemies.Count == 0)
        {
            _closestEnemy = _afterEnemiesTransform;
            canAim = false;
        }

        if (_closestEnemy == null)
            _closestEnemy = _enemies[0].transform;

        if (_closestEnemy != null && !_closestEnemy.gameObject.activeSelf)
            _closestEnemy = _enemies[0].transform;

        if (_enemies != null && _enemies.Count != 0)
            for (int i = 0; i < _enemies.Count; i++)
            {
                float newClose = Vector2.Distance(_enemies[i].transform.position, transform.position);
                if (newClose < Vector2.Distance(_closestEnemy.position, transform.position))
                    _closestEnemy = _enemies[i].transform;
            }

        if (Vector2.Distance(_closestEnemy.position, transform.position) > 5f)
            _closestEnemy = null;

        if (_closestEnemy == null)
            return;
        Vector3 difference = _closestEnemy.position - _lookPoint.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        // Add the offset to the rotation
        rotationZ += _offset;

        // Create a Quaternion from the rotationZ value
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

        // Apply the rotation to the look direction object
        _lookPoint.rotation = rotation;

        SetScale();
    }
    void SetScale()
    {
        float xScale = transform.position.x - _closestEnemy.position.x >= 0 ? -Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
    }

    void ShootTimer()
    {
        _currentShootTimer -= Time.deltaTime;
        Debug.Log(_currentShootTimer);
        if (_currentShootTimer <= 0 && !_isShooting)
        {
            if (_closestEnemy == null)
            {
                _currentShootTimer = WeaponStat.Timer[_currentSkill % _weaponStat.Timer.Length];
                return;
            }
            _isShooting = true;
            _weapon.AnimatorSc.SetTrigger("Shoot");
        }
    }

    public void Shoot()
    {
        if (_closestEnemy == null)
        {
            _currentShootTimer = WeaponStat.Timer[_currentSkill % _weaponStat.Timer.Length];
            _isShooting = false;
            return;
        }

        if (_weaponStat.Speed.Length > 1)
            _currentSkill = _bulletCounter % _weaponStat.Speed.Length;

        for (int i = _bulletCounter; i < _weaponStat.MaxBullet; i++)
            if (_bullets[_bulletCounter % _weaponStat.MaxBullet].gameObject.activeSelf)
                _bulletCounter++;

        _bullets[_bulletCounter++ % _weaponStat.MaxBullet].Setup(
            _firePoint.position,
            _closestEnemy.position,
            _weaponStat.Damage[_currentSkill],
            _weaponStat.Speed[_currentSkill],
            "Player",
            canActivateSkill: true,
            _bullets
            );
        _currentShootTimer = WeaponStat.Timer[(_currentSkill + 1) % _weaponStat.Timer.Length];
        _isShooting = false;
    }

    void CreateBullets()
    {
        _bullets = new Bullet[_weaponStat.MaxBullet];

        for (int i = 0; i < _weaponStat.MaxBullet; i++)
            _bullets[i] = Instantiate(_bulletPrefab, new Vector3(-100, -100, 0), Quaternion.identity);
    }

    public void SetWeapon(Weapon weapon)
    {
        _weapon = weapon;
        _weaponStat = weapon.WeaponStatSO;
        _firePoint = weapon.ShootPoint;
        _weapon.AnimatorSc.SetFloat("ShootSpeed", 1 + (_weaponStat.Speed[0] / 10));
        _bulletPrefab = WeaponStat.Projectile.GetComponent<Bullet>();

        Debug.Log(weapon);
    }
    public void RefreshEnemies()
    {
        _enemies = GameManager.Instance.GetEnemies();
    }
}
