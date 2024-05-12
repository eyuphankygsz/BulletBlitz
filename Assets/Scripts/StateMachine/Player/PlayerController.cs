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

    private Interactable _interactable = null;
    private Interactable _holdingObject = null;
    [SerializeField] private Transform _holdPoint;

    //--------------------------------------


    //--------Ground Check---------
    private float _oldY;
    public float YDirection { get; private set; }

    private bool _sloope;
    public bool IsGroundSloope { get { return _sloope; } }

    public bool CanCheckGround = true;

    private float _groundCheckCounter;
    private bool _isGroundCheckCounting;
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

    private PlayerJumpController _jumper;
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
    private RaycastHit2D[] _collectableHits = new RaycastHit2D[10];

    [SerializeField] public GameObject WeaponParent;

    private void Awake()
    {
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
        HitCooldownCountdown();
        CheckYDirection();
        CheckHits();
    }

    public bool IsOnGround()
    {
        if (!CanCheckGround) return false;

        int hits = Physics2D.BoxCastNonAlloc(_groundCheckTranform.position, _groundCheckSize, 0, Vector2.down, _groundHit, 0, _groundLayerMask);
        if (hits > 0)
        {
            Vector3 closestPoint = _groundHit[0].collider.ClosestPoint(transform.position);
            Vector3 position = transform.position;
            position.y = closestPoint.y + _feetOffset;

            if (position.y > transform.position.y + 0.1f)
                return true;

            transform.position = position;
            _jumper.StopJump();
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
        int collectableHits = Physics2D.BoxCastNonAlloc(_wallCheckTranform.position + (Vector3.up * _wallCheckOffset), _wallCheckSize, 0, Vector2.down, _collectableHits, 0, GameManager.Instance.CollectableLayer);
        for (int i = 0; i < collectableHits; i++)
            Hit(_collectableHits[i].collider);

        int trapHits = Physics2D.BoxCastNonAlloc(_wallCheckTranform.position + (Vector3.up * _wallCheckOffset), _wallCheckSize, 0, Vector2.down, _collectableHits, 0, GameManager.Instance.TrapsLayer);
        if (trapHits == 1)
            _collectableHits[0].collider.GetComponent<TrapsBase>().OnCollide(gameObject);


        int interactableHits = Physics2D.BoxCastNonAlloc(_wallCheckTranform.position + (Vector3.up * _wallCheckOffset), _wallCheckSize, 0, Vector2.down, _collectableHits, 0, GameManager.Instance.InteractableLayer);
        if (interactableHits > 0)
        {
            int closestId = 0;
            float closestDistance = 100;
            for (int i = 0; i < interactableHits; i++)
            {
                if (_collectableHits[i].transform.GetComponent<Interactable>() == _holdingObject)
                    continue;

                float dist = Vector3.Distance(transform.position, _collectableHits[i].transform.position);
                if (dist < closestDistance)
                {
                    closestId = i;
                    closestDistance = dist;
                }
            }
            _collectableHits[closestId].collider.GetComponent<Interactable>().OnTrigger(out _interactable);
            PanelManager.Instance.InteractButton.gameObject.SetActive(true);
        }
        else
            PanelManager.Instance.InteractButton.gameObject.SetActive(false);

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
        _jumper = GetComponent<PlayerJumpController>();
        Animator = GetComponent<Animator>();
    }
    void SetStates()
    {
        GroundRootState = gameObject.AddComponent<PlayerGroundState>();
        JumpRootState = gameObject.AddComponent<PlayerJumpedState>();
        GroundIdleState = gameObject.AddComponent<PlayerGroundIdleState>();
        GroundWalkState = gameObject.AddComponent<PlayerGroundWalkState>();
        JumpIdleState = gameObject.AddComponent<PlayerJumpedIdleState>();
        JumpWalkState = gameObject.AddComponent<PlayerJumpedWalkState>();
        HitRootState = gameObject.AddComponent<PlayerHitState>();
        DeadRootState = gameObject.AddComponent<PlayerDeadState>();
        SpecialSkillState = gameObject.AddComponent<PlayerSpecialSkillState>();
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

        //------------------------------------------
        trigger = PanelManager.Instance.InteractButton;
        if (trigger.triggers.Count > 0)
            trigger.triggers.Clear();

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventdata) => { Interact(); });
        trigger.triggers.Add(entry);

    }
    public void SetStats()
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
    public void StartJump(float yStrength, float xStrength, float gravity)
    {
        _jumper.StartJump(yStrength, xStrength, gravity);
    }
    public void Interact()
    {
        if (_interactable == null)
            return;
        _interactable.OnEnabled();
    }
    public void HoldObject(Interactable obj)
    {
        if (obj != null)
        {
            obj.transform.SetParent(_holdPoint);
            obj.transform.localPosition = Vector3.zero;
        }
        else
        {
            _holdingObject.transform.SetParent(null);
        }
        _holdingObject = obj;
    }
    private void CheckYDirection()
    {
        YDirection = Mathf.Sign(transform.position.y - _oldY);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckTranform.position, _groundCheckSize);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_wallCheckTranform.position + (Vector3.up * _wallCheckOffset), _wallCheckSize);
    }
}
