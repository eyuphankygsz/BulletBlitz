using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //-------- STATES ENUM ---------
    public enum States
    {
        Patrol,
        Attack,
        Fly,
        Hit,
        Dead,
        Approach,
        Idle
    }
    private States _currentState;
    public States CurrentState { get { return _currentState; } set { _currentState = value; } }

    [SerializeField]
    enum Behavior
    {
        Kid,
        Goat,
        Crow,
        Witch
    }
    [Header("States")]
    [SerializeField] private Behavior _behavior;

    [SerializeField] EnemyStat _enemyStat;
    public EnemyStat EnemyStat { get { return _enemyStat; } }


    EnemyStateManager _enemyStateManager;
    //----------------------------


    //-------- Physics and Controls ---------
    public Rigidbody2D RB { get; private set; }

    [field: SerializeField] public DropItem[] ItemsToDrop { get; private set; }
    //--------------------------------------


    //-------- Ground Check ---------

    public bool IsGroundSloope { get; private set; } = false;
    public bool CanCheckGround { get; private set; } = true;

    float _groundCheckCounter;
    bool _isGroundCheckCounting;

    [Header("Ground Control")]
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] Transform _groundCheckTranform;
    [SerializeField] Vector2 _groundCheckSize;

    //-----------------------------

    bool _canMove = true;
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }

    private EnemyBehaviorBase _enemyBehavior;
    public EnemyBehaviorBase EnemyBehavior { get { return _enemyBehavior; } }

    [SerializeField] SpriteRenderer[] _bodyParts;

    //-------- Hit Control ---------
    [Header("Hit Control")]
    bool _isHitCooldownActive = false;

    [SerializeField] float _defaultHitCooldownTime;
    float _hitCooldownTimer;
    float _colorTime;
    public Collider2D HitObject { get; private set; }

    [field: SerializeField] public GameObject DestroyParticle { get; private set; }
    //----------------------------

    //-------- Wall Control ---------
    bool _isWallCooldownActive = false;


    public bool CanCheckWalls { get { return _canCheckWalls; } }
    bool _canCheckWalls = true;
    [Header("Wall Control")]
    [SerializeField] Transform _wallCheckTransform;
    [SerializeField] Vector2 _wallCheckSize;

    [SerializeField] float _defaultWallCooldownTime;
    float _wallCooldownTimer;
    //----------------------------

    //--------Animation---------
    public Animator Animator { get; private set; }
    //--------------------------

    //-------- Player ---------
    Transform _player;
    public Transform Player { get { return _player; } }
    //-----------------------

    //-------- AI ---------
    [Header("AI")]
    [SerializeField] private LayerMask _obstaclesLayerMask;

    public GameObject EnemyObject { get { return _enemyObject; } }
    GameObject _enemyObject;

    public float FOV { get; private set; }

    [SerializeField] float _maxHeight;
    public Vector3 StartPoint { get { return _startPoint; } }
    [SerializeField] Vector3 _startPoint;
    public Vector3 EndPoint { get { return _endPoint; } }
    [SerializeField] Vector3 _endPoint;

    [SerializeField] Transform _enemyCheckTransform;
    [SerializeField] Vector2 _enemyCheckSize;
    [SerializeField] Vector2 _seenRange;

    public Transform LookPoint { get { return _lookPoint; } }
    [SerializeField] Transform _lookPoint;
    public float LookOffset { get { return _lookOffset; } }
    [SerializeField] float _lookOffset;

    private bool _canTrackPlayer = true;
    public bool CanTrackPlayer { get { return _canTrackPlayer; } }//set { _canTrackPlayer = value; } }

    private bool _isArrivedToDestination = false;
    public bool IsArrivedToDestination { get { return _isArrivedToDestination; } }// set { _isArrivedToDestination = value; } }

    private bool _isTracking;
    public bool IsTracking { get { return _isTracking; } }// set { _isTracking = value; } }

    private RaycastHit2D[] _raycastHits = new RaycastHit2D[10];

    private bool _seen;
    //--------------------

    //-------- Attack ---------
    private bool _canAttack = false;
    public bool CanAttack { get { return _canAttack; } set { _canAttack = value; } }


    public float AttackRadius { get { return _attackRadius; } }
    [Header("Attack Control")]
    [SerializeField] float _attackRadius;
    public WeaponStat BulletStat { get { return _bulletStat; } }
    [SerializeField] WeaponStat _bulletStat;
    public Transform AttackPoint { get { return _attackPoint; } }
    [SerializeField] Transform _attackPoint;

    //------------------------

    //-------- Health ---------
    private float _health;
    public bool Invulnerable { get; private set; }
    //-------------------------
    //-------- Map Control --------
    [field: SerializeField] public float MinX, MaxX, MinY, MaxY;
    //-----------------------------
    private void Awake()
    {
        GameManager.Instance.AddEnemy(gameObject);
        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        SetStats();
        SetBehavior();
    }
    void Start()
    {
        _player = GameManager.Instance.Player.transform;

        _enemyStateManager = GetComponent<EnemyStateManager>();
        _enemyStateManager.SwitchState(_enemyBehavior.StartState());

        IgnoreCollision();
    }

    // Update is called once per frame
    void Update()
    {
        ManageEnemyBehavior();
        CheckGroundCountdown();
        HitCooldownCountdown();
        WallCheckCountdown();
        PlayerSkillCheck();
    }

    void ManageEnemyBehavior()
    {
        _enemyStateManager.CurrentState.Update();
    }

    public bool IsOnGround()
    {
        int hits = Physics2D.BoxCastNonAlloc(_groundCheckTranform.position, _groundCheckSize, 0, Vector2.down, _raycastHits, 0, _groundLayerMask);

        if (hits > 0)
            //IsGroundSloope = Mathf.Abs(hit.collider.gameObject.transform.eulerAngles.z) > 50;
            return true;
        //IsGroundSloope = false;
        return false;
    }
    public bool IsWallAhead()
    {
        int hits = Physics2D.BoxCastNonAlloc(_wallCheckTransform.position, _wallCheckSize, 0, Vector2.down, _raycastHits, 0, _obstaclesLayerMask);
        if (hits > 0)
            return true;
        return false;
    }
    public bool IsEnemyAhead(string tag)
    {
        int hits = Physics2D.BoxCastNonAlloc(_enemyCheckTransform.position, _enemyCheckSize, 0, Vector2.down, _raycastHits, 0, GameManager.Instance.EnemyLayer);

        if (hits > 0)
            if (string.IsNullOrEmpty(tag))
                return true;
            else
                for (int i = 0; i < hits; i++)
                    if (_raycastHits[i].collider.gameObject != gameObject && _raycastHits[i].collider.tag == tag)
                    {
                        _enemyObject = _raycastHits[i].collider.gameObject;
                        return true;
                    }

        return false;
    }
    public void PlayerSkillCheck()
    {
        if (!GameManager.Instance.SpecialSkill || IsPlayerAhead() == null)
            return;

        SkillHit();
    }
    public Collider2D IsPlayerAhead()
    {
        int hits = Physics2D.BoxCastNonAlloc(_enemyCheckTransform.position, _enemyCheckSize, 0, Vector2.down, _raycastHits, 0, GameManager.Instance.PlayerLayer);
        if (hits > 0) return _raycastHits[0].collider;
        else return null;
    }
    public bool CanSeePlayer()
    {
        int hitCount = Physics2D.LinecastNonAlloc(transform.position, Player.transform.position, _raycastHits, _obstaclesLayerMask);
        if (hitCount > 0)
            if (_raycastHits[0].collider != null)
                return false;

        Vector2 direction = (Player.position - _enemyCheckTransform.position).normalized;
        hitCount = Physics2D.RaycastNonAlloc(transform.position,direction,_raycastHits,_enemyStat.FollowRange,GameManager.Instance.PlayerLayer);
        if(hitCount > 0) 
            return true;
        return false;
    }
    public void TryToChangeState(EnemyBaseState state, States currentStateEnum)
    {
        if (CurrentState == currentStateEnum)
            if (_enemyStateManager.CurrentState != state)
            {
                _currentState = currentStateEnum;
                _enemyStateManager.SwitchState(state);
            }
    }
    void IgnoreCollision()
    {
        Physics2D.IgnoreLayerCollision(Player.gameObject.layer, gameObject.layer, true);
        Physics2D.IgnoreLayerCollision(gameObject.layer, gameObject.layer, true);
    }
    void SetBehavior()
    {
        _enemyBehavior = GetComponent<EnemyBehaviorBase>(); 
        _enemyBehavior.SetController(this);
    }
    void SetStats()
    {
        _health = EnemyStat.Health + BulletStat.AdditionalHealth;
        FOV = EnemyStat.FollowRange;
    }
    public float DistanceBetweenPlayer()
    {
        return Mathf.Abs(Vector2.Distance(transform.position, _player.position));
    }
    public bool HasSeenPlayer()
    {
        int hits = Physics2D.BoxCastNonAlloc(_enemyCheckTransform.position, _seenRange, 0, Vector2.down, _raycastHits, 0, GameManager.Instance.PlayerLayer);
        return (hits > 0) || _seen;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hit(collision);
    }
    void SkillHit()
    {
        if (Invulnerable) return;
        _hitCooldownTimer = _defaultHitCooldownTime;
        _colorTime = _hitCooldownTimer - 0.1f;
        _isHitCooldownActive = true;
        LoseHearth(20);
        HitObject = Player.GetComponent<Collider2D>();
        _enemyStateManager.SwitchState(_enemyBehavior.Hit());
    }
    public void Hit(Collider2D collision)
    {
        if (CanHit(collision))
        {
            _seen = true;
            _hitCooldownTimer = _defaultHitCooldownTime;
            _colorTime = _hitCooldownTimer - 0.1f;
            _isHitCooldownActive = true;
            LoseHearth(collision.GetComponent<Bullet>().BulletDamage);
            _enemyStateManager.SwitchState(_enemyBehavior.Hit());
            if (HitObject.tag == "Bullet")
            {
                HitObject.gameObject.SetActive(false);
                HitObject.GetComponent<Bullet>().AddHittedEnemies(gameObject);
            }

        }
    }
    bool CanHit(Collider2D collision)
    {
        if (_isHitCooldownActive || Invulnerable)
            return false;

        if (collision.tag == "Bullet")
        {
            if (collision.GetComponent<Bullet>().From != "Player")
                return false;
        }
        else
            return false;

        HitObject = collision;
        return true;
    }
    void HitCooldownCountdown()
    {
        if (_isHitCooldownActive)
        {

            if (_hitCooldownTimer < _colorTime)
            {
                for (int i = 0; i < _bodyParts.Length; i++)
                    _bodyParts[i].color = _bodyParts[i].color == Color.red ? Color.black : Color.red;
                _colorTime = _hitCooldownTimer - 0.1f;
            }

            _hitCooldownTimer -= Time.deltaTime;
            if (_hitCooldownTimer <= 0)
            {
                _isHitCooldownActive = false;
                for (int i = 0; i < _bodyParts.Length; i++)
                    _bodyParts[i].color = Color.white;
            }
        }
    }
    void WallCheckCountdown()
    {
        if (_isWallCooldownActive)
        {
            if (_wallCooldownTimer <= 0)
            {
                _isWallCooldownActive = false;
                _canCheckWalls = true;
            }
            _wallCooldownTimer -= Time.deltaTime;
        }
    }
    void LoseHearth(float damage)
    {
        if (_health <= 0)
            return;

        _health -= damage;
        if (_health <= 0)
        {
            GameManager.Instance.RemoveEnemy(gameObject);
            _enemyStateManager.SwitchState(_enemyBehavior.Dead());
        }
    }
    public void StartGroundCheckCountdown(float defaultTime)
    {
        CanCheckGround = false;
        _groundCheckCounter = defaultTime;
        _isGroundCheckCounting = true;
    }
    public void StartWallCheckCountdown()
    {
        _canCheckWalls = false;
        _wallCooldownTimer = _defaultWallCooldownTime;
        _isWallCooldownActive = true;
    }
    void CheckGroundCountdown()
    {
        if (_isGroundCheckCounting)
        {
            _groundCheckCounter -= Time.deltaTime;
            if (_groundCheckCounter <= 0)
            {
                _groundCheckCounter = 0;
                _isGroundCheckCounting = false;
                CanCheckGround = true;
            }
        }
    }
    public void AllowToAttack()
    {
        _canAttack = true;
    }
    public void SetInvulnerable(bool value)
    {
        Invulnerable = value;
    }
    public void DropItems()
    {
        for (int i = 0; i < ItemsToDrop.Length; i++)
            if (Random.Range(0, 101) <= ItemsToDrop[i].Luck)
            {
                int quantity = Random.Range(ItemsToDrop[i].MinQuantity, ItemsToDrop[i].MaxQuantity + 1);
                int count = 0;

                for (int j = 0; j < quantity; j++)
                {
                    for (int k = 0; k < GameManager.Instance.Collectables.Count; k++)
                        if (GameManager.Instance.Collectables[k].tag == ItemsToDrop[i].DroppedItem.tag && !GameManager.Instance.Collectables[k].activeSelf)
                        {
                            GameManager.Instance.Collectables[k].transform.position = transform.position;
                            GameManager.Instance.Collectables[k].SetActive(true);
                            GameManager.Instance.Collectables[k].GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)));
                            if (count++ >= quantity)
                                break;
                            continue;
                        }
                    if (count++ >= quantity)
                        break;
                    GameObject newItem = GameObject.Instantiate(ItemsToDrop[i].DroppedItem, transform.position, Quaternion.identity);
                    GameManager.Instance.AddCollectable(newItem);
                    newItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)));
                }
            }
    }
    private void OnDrawGizmos()
    {
        _enemyBehavior?.OnDrawGizmos();
        Gizmos.color = Color.blue;
        if (_groundCheckTranform != null)
            Gizmos.DrawWireCube(_groundCheckTranform.position, _groundCheckSize);
        Gizmos.color = Color.green;
        if (_wallCheckTransform != null)
            Gizmos.DrawWireCube(_wallCheckTransform.position, _wallCheckSize);
        Gizmos.color = Color.red;
        if (_attackPoint != null)
            Gizmos.DrawWireSphere(_attackPoint.position, _attackRadius);
        Gizmos.color = Color.black;
        if (_enemyCheckTransform != null)
        {
            Gizmos.DrawWireCube(_enemyCheckTransform.position, _enemyCheckSize);
            Gizmos.DrawWireCube(_enemyCheckTransform.position, _seenRange);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FOV);
    }
}
