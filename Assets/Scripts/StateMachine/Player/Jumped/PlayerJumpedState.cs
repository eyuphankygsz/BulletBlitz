using System.Collections;
using UnityEngine;

public class PlayerJumpedState : PlayerBaseState
{
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    private bool _canDoubleJump = false;
    private bool _isJumpedOnce = false;
    private bool _isJumpedTwice = false;

    private bool _isJumping = false;
    private int _yDirection;

    private float _jumpHeight = 4;
    private float _velocity;
    private float _gravity = -9;

    bool IsMoving() { return _controller.GetAxis() != 0; }

    PlayerController.States _stateEnum = PlayerController.States.Jump;
    protected PlayerStateManager _player;

    public override void EnterState(PlayerStateManager player)
    {
        if (_player == null)
            _player = player;

        _controller.Animator.SetFloat("Jump", 0);
        if (_controller.CurrentState == PlayerController.States.Ground)
            _isJumping = false;

        if (_isJumping || (_isJumpedOnce && !_canDoubleJump) || (_canDoubleJump && _isJumpedTwice))
            return;

        _velocity = _jumpHeight;

        _isJumpedOnce = false;
        _isJumpedTwice = false;
        _controller.Animator.SetBool("OnGround", false);

        _controller.Animator.SetTrigger("JumpTrigger");
        _controller.CurrentState = _stateEnum;
        _controller.CanCheckGround = false;

        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.PlayerSFX.Clips["Jump"], false);
        _canDoubleJump = PlayerPrefs.GetInt("DoubleJump") == 1 ? true : false;

        if (_isJumpedOnce)
            _isJumpedTwice = true;
        _isJumpedOnce = true;



    }
    public override void StateUpdate()
    {
        UpdatePosition();


        if (_controller.TryToChangeState(NewState(), _stateEnum))
            return;
        _controller.Animator.SetFloat("Jump", _yDirection, 0.1f, Time.deltaTime);
    }
    public override void StateFixedUpdate()
    {
        if (_controller.IsOnGround())
            _controller.TryToChangeState(_controller.GroundRootState, _stateEnum);
    }
    private void UpdatePosition()
    {
        _velocity += _gravity * Time.deltaTime;
        if (!_controller.CanCheckGround && _velocity <= 0)
            _controller.CanCheckGround = true;
        Vector3 direction = new Vector3(0, _velocity, 0) * Time.deltaTime;
        _yDirection = (int)Mathf.Sign(direction.y);
        _controller.transform.Translate(direction);
    }
    private PlayerBaseState NewState()
    {
        if (IsMoving())
            return _controller.JumpWalkState;

        return _controller.JumpIdleState;
    }

    public override void ExitState()
    {
        if (_controller.IsHit)
            _isJumping = false;

        if (_isJumping)
            return;
        _isJumpedOnce = false;
        _isJumpedTwice = false;
    }
}
