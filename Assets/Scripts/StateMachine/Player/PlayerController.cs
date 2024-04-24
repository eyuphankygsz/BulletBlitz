using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //--------STATES ENUM---------
    public enum States
    {
        Ground,
        Jump,
        Hit,
        Dead,
        Special
    }
    private States _currentState;
    public States CurrentState { get { return _currentState; } set { _currentState = value; } }

    PlayerStateManager _playerStateManager;
    //----------------------------


    //--------Physics and Controls---------
    [SerializeField] bool _joystickControl;
    FixedJoystick _joystick;

    //--------------------------------------


    //--------Ground Check---------
    private float _oldY;
    public float YDirection { get; private set; }

    private bool _sloope;
    public bool IsGroundSloope { get { return _sloope; } }

    private bool _canCheckGround = true;
    public bool CanCheckGround { get { return _canCheckGround; } }
    private float _groundCheckCounter;
    private bool _isGroundCheckCounting;

    public bool IsJumping { get; set; } = false;
    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Transform _groundCheckTranform;
    [SerializeField] private Vector2 _groundCheckSize;
    [SerializeField] private float _feetOffset;

    [SerializeField] private LayerMask _wallLayerMask;
    [SerializeField] private Transform _wallCheckTranform;
    [SerializeField] private Vector2 _wallCheckSize;
    [SerializeField] private float _wallCheckOffset;
    //-----------------------------

    private bool _canMove = true;
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }

    [SerializeField] AutoShooter _autoShooter;

    [SerializeField] SpriteRenderer[] _bodyParts;

    //--------States---------
    public PlayerGroundState GroundRootState { get; private set; }
    public PlayerJumpedState JumpRootState { get; private set; }
    public PlayerGroundIdleState GroundIdleState { get; private set; }
    public PlayerGroundWalkState GroundWalkState { get; private set; }
    public PlayerJumpedIdleState JumpIdleState { get; private set; }
    public PlayerJumpedWalkState JumpWalkState { get; private set; }
    public PlayerHitState HitRootState { get; private set; }
    public PlayerDeadState DeadRootState { get; private set; }
    public PlayerSpecialSkillState SpecialSkillState { get; private set; }
    //-----------------------

    //--------Hit Control---------
    public Collider2D PlayerCollider { get; private set; }
    public bool IsHit { get { return _isHit; } }
    private bool _isHit = false;

    private bool _isHitCooldownActive = false;

    [SerializeField] private float _defaultHitCooldownTime;
    private float _hitCooldownTimer;
    private float _colorTime;
    public Collider2D HitObject { get; private set; }
    [SerializeField] private Collider2D _collider;


    //----------------------------

    //--------Animation---------
    public Animator Animator { get; private set; }

    //--------------------------

    //--------Health---------
    private float _health, _maxHealth;
    private Image _healthUI;

    public bool Dead
    {
        get { return _dead; }
        private set
        {
            _dead = value;
            GameManager.Instance.Dead();
        }
    }
    private bool _dead = false;
    //-----------------------

    private RaycastHit2D[] _groundHit = new RaycastHit2D[1];
    private RaycastHit2D[] _wallHit = new RaycastHit2D[1];
    private RaycastHit2D[] _enemyHit = new RaycastHit2D[10];

    [SerializeField] public GameObject WeaponParent;

    private void Awake()
    {
        SetStats();
        SetScripts();
        SetStates();
    }
    void Start()
    {
        SetButtons();
        _playerStateManager.SwitchState(GroundIdleState);
    }

    // Update is called once per frame
    void Update()
    {
        if (_dead)
            return;

        _autoShooter.UpdateShooter();
        CheckGroundCountdown();
        HitCooldownCountdown();
        SetYDirection();
        CheckHits();
    }

    public bool IsOnGround()
    {
        if (IsJumping) return false;

        int hits = Physics2D.BoxCastNonAlloc(_groundCheckTranform.position, _groundCheckSize, 0, Vector2.down, _groundHit, 0, _groundLayerMask);
        if (hits > 0)
        {
            Vector3 closestPoint = _groundHit[0].collider.ClosestPoint(transform.position);
            Vector3 position = transform.position;
            position.y = closestPoint.y + _feetOffset;

            if (position.y > transform.position.y + 0.1f)
                return true;

            transform.position = position;
            return true;
        }
        _sloope = false;
        return false;
    }
    public float IsWallAhead()
    {
        int hits = Physics2D.BoxCastNonAlloc(_wallCheckTranform.position + (Vector3.up * _wallCheckOffset), _wallCheckSize, 0, Vector2.down, _wallHit, 0, _wallLayerMask);
        if (hits > 0)
            return _wallHit[0].point.x - transform.position.x;

        return 0;
    }

    public float GetAxis()
    {
        if (!_canMove)
            return 0;

        if (_joystickControl)
            return _joystick.Horizontal;

        return Input.GetAxisRaw("Horizontal");
    }

    private void SetYDirection()
    {
        YDirection = (transform.position.y - _oldY >= 0) ? 1 : -1;
        _oldY = transform.position.y;
    }
    public bool TryToChangeState(PlayerBaseState state, States currentStateEnum)
    {
        if (CurrentState == currentStateEnum)
            if (_playerStateManager.CurrentState != state)
            {
                _playerStateManager.SwitchState(state);
                return true;
            }
        return false;
    }


    public void Hit(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            collision.gameObject.SetActive(false);
            return;
        }
        if (CanHit(collision) && !_isHitCooldownActive)
        {
            _isHit = true;
            _hitCooldownTimer = _defaultHitCooldownTime;
            _colorTime = _hitCooldownTimer - 0.1f;
            _isHitCooldownActive = true;
            _playerStateManager.SwitchState(HitRootState);
            float damage = 0;
            if (collision.tag == "Bullet")
                damage = collision.GetComponent<Bullet>().BulletDamage;

            LoseHealth(damage);

        }
    }
    private void CheckHits()
    {
        int enemyHits = Physics2D.BoxCastNonAlloc(_wallCheckTranform.position + (Vector3.up * _wallCheckOffset), _wallCheckSize, 0, Vector2.down, _enemyHit, 0, GameManager.Instance.EnemyLayer | GameManager.Instance.CollectableLayer);
        if (enemyHits > 0)
            for (int i = 0; i < enemyHits; i++)
                Hit(_enemyHit[i].collider);

    }
    public void HitWithDamage(int damage, GameObject attacker)
    {
        if (!_isHit && !_isHitCooldownActive)
        {
            _isHit = true;
            _hitCooldownTimer = _defaultHitCooldownTime;
            _colorTime = _hitCooldownTimer - 0.1f;
            _isHitCooldownActive = true;
            HitObject = attacker.GetComponent<Collider2D>();
            _playerStateManager.SwitchState(HitRootState);
            LoseHealth(damage);
        }
    }
    public void LoseHealth(float damage)
    {
        if (_health <= 0 || _dead)
            return;

        _health -= damage;
        if (_health <= 0)
        {
            _health = 0;
            _playerStateManager.SwitchState(DeadRootState);
            Dead = true;

        }
        DisplayHealth();

    }
    bool CanHit(Collider2D collision)
    {
        if (_isHit && GameManager.Instance.SpecialSkill)
            return false;


        if (collision.tag == "Bullet")
            if (collision.GetComponent<Bullet>().From == "Player")
                return false;

        if (collision.tag == "Portal")
        {
            GameManager.Instance.NextScene();
            return false;
        }

        HitObject = collision;
        return true;
    }
    public void StartGroundCheckCountdown(float defaultTime)
    {
        _canCheckGround = false;
        _groundCheckCounter = defaultTime;
        _isGroundCheckCounting = true;
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
                _canCheckGround = true;
            }
        }
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
        else
        {

            _isHit = false;
        }
    }

    void SetScripts()
    {
        PlayerCollider = GetComponent<Collider2D>();
        _playerStateManager = GetComponent<PlayerStateManager>();
        Animator = GetComponent<Animator>();
    }
    void SetStates()
    {
        GroundRootState = new PlayerGroundState(this);
        JumpRootState = new PlayerJumpedState(this);
        GroundIdleState = new PlayerGroundIdleState(this);
        GroundWalkState = new PlayerGroundWalkState(this);
        JumpIdleState = new PlayerJumpedIdleState(this);
        JumpWalkState = new PlayerJumpedWalkState(this);
        HitRootState = new PlayerHitState(this);
        DeadRootState = new PlayerDeadState(this);
        SpecialSkillState = new PlayerSpecialSkillState(this);
    }
    private void SetButtons()
    {
        _joystick = PanelManager.Instance.Joystick;

        EventTrigger trigger = PanelManager.Instance.JumpButton;
        if (trigger.triggers.Count > 0)
            trigger.triggers.Clear();


        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventdata) => { TryToJump(); });
        trigger.triggers.Add(entry);

    }
    private void SetStats()
    {
        _healthUI = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
        _maxHealth = 5 + _autoShooter.WeaponStat.AdditionalHealth;
        _health = _maxHealth;
        DisplayHealth();
    }

    void DisplayHealth()
    {
        _healthUI.fillAmount = (float)_health / _maxHealth;
    }
    public void TryToJump()
    {
        if (IsOnGround() && !GameManager.Instance.SpecialSkill)
        {
            _playerStateManager.SwitchState(JumpRootState);
            return;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckTranform.position, _groundCheckSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_wallCheckTranform.position + (Vector3.up * _wallCheckOffset), _wallCheckSize);
    }
}
