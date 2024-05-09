using UnityEngine;

public class PlayerHitState : PlayerBaseState
{

    PlayerController.States _stateEnum = PlayerController.States.Hit;
    bool _isHitting;
    
    private float _jumpHeightY = 3, _jumpHeightX = 2;
    private float _gravity = -9;
    private float _velocity;

    private PlayerStateManager _player;


    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }
    public override void EnterState(PlayerStateManager player)
    {
        if (_player == null)
            _player = player;

        if (_isHitting)
            return;
        else
            _isHitting = true;

        _controller.CanCheckGround = false;

        float directionX = _controller.transform.position.x - _controller.HitObject.transform.position.x >= 0 ? 1 : -1;

        Debug.Log(_jumpOffset);
        _hitStates = JumpStates.Jump;
        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.PlayerSFX.Clips["Hit"], false);

        _controller.CurrentState = _stateEnum;
        _controller.CanCheckGround = false;


        if (_controller.HitObject.tag == "Bullet")
            _controller.HitObject.gameObject.SetActive(false);

        _controller.Animator.SetTrigger("HitTrigger");
        Hit(player);
    }
    public override void StateUpdate()
    {
        switch (_hitStates)
        {
            case JumpStates.Jump:
                Jump();
                break;
            case JumpStates.Fall:
                Fall();
                break;
        }
        _controller.CanCheckGround = false;
    }
    public override void StateFixedUpdate()
    {
        CheckGround();
    }
    private void Jump()
    {
        if (Vector2.Distance(_controller.transform.position, _jumpOffset) < 0.1f || _controller.transform.position == _oldPos)
            _hitStates = JumpStates.Fall;
        else
        {
            _oldPos = _controller.transform.position;

            Vector2 newDirection = (new Vector3(_jumpOffset.x,_jumpOffset.y,0) - _controller.transform.position).normalized;
            float wallDirection = _controller.IsWallAhead();
            if (Mathf.Sign(wallDirection) == Mathf.Sign(newDirection.x) && wallDirection != 0)
                newDirection.x = 0;

            Debug.Log(newDirection);
            _controller.transform.Translate(((Vector2.right * newDirection.x * _speedHeight) +
                 (Vector2.up * newDirection.y * _speedHeight)) * Time.deltaTime);
        }
    }
    private void Fall()
    {
        if (_controller.IsOnGround())
            _hitStates = JumpStates.Done;
        else
            _controller.transform.Translate(Vector2.down * _speedHeight * Time.deltaTime);
    }
    void Hit(PlayerStateManager player)
    {
        switch (_hitStates)
        {
            case JumpStates.Jump:
                Jump();
                break;
            case JumpStates.Fall:
                Fall();
                break;
        }
    }


    void CheckGround()
    {
        if (_controller.IsOnGround() && _controller.CanCheckGround)
            _player.SwitchState(_controller.GroundRootState);
    }
    public override void ExitState()
    {
        _isHitting = false;
    }
}
