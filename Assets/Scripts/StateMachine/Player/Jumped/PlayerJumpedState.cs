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

    bool IsMoving() { return _controller.GetAxis() != 0; }

    PlayerController.States _stateEnum = PlayerController.States.Jump;
    protected PlayerStateManager _player;

    public override void EnterState(PlayerStateManager player)
    {
        if (_player == null)
            _player = player;

        _controller.Animator.SetFloat("Jump", 0); 
        _controller.Animator.SetTrigger("JumpTrigger");
        if (_controller.CurrentState == PlayerController.States.Ground)
            _isJumping = false;

        if (_isJumping || (_isJumpedOnce && !_canDoubleJump) || (_canDoubleJump && _isJumpedTwice))
            return;


        _controller.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        _controller.RB.velocity = new Vector2(_controller.RB.velocity.x, 0);

        _isJumpedOnce = false;
        _isJumpedTwice = false;
        _controller.Animator.SetBool("OnGround", false);

        _controller.CurrentState = _stateEnum;
        _controller.CanCheckGround = false;

        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.PlayerSFX.Clips["Jump"], false);
        _canDoubleJump = PlayerPrefs.GetInt("DoubleJump") == 1 ? true : false;

        if (_isJumpedOnce)
            _isJumpedTwice = true;
        _isJumpedOnce = true;

        _controller.StartJump(0);


    }
    public override void StateUpdate()
    {
        if (_controller.TryToChangeState(NewState(), _stateEnum))
            return;
        _controller.Animator.SetFloat("Jump", _controller.RB.velocity.y, 0.1f, Time.deltaTime);

        if (_controller.IsOnGround())
            _controller.TryToChangeState(_controller.GroundRootState, _stateEnum);
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

    public override void StateFixedUpdate() { }
}
