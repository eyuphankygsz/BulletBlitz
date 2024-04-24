using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpedState : PlayerBaseState
{
    public PlayerJumpedState(PlayerController controller) : base(controller) { }

    bool _canDoubleJump = false;
    bool _isJumpedOnce = false;
    bool _isJumpedTwice = false;

    bool _isJumping = false;

    private JumpStates _jumpStates = JumpStates.Jump;
    private float _jumpOffset;
    private float _speed = 6;
    private float _multiplier = 3;
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

        _multiplier = 3;
        _isJumpedOnce = false;
        _isJumpedTwice = false;

        _controller.Animator.SetBool("OnGround", false);

        _controller.Animator.SetTrigger("JumpTrigger");
        _controller.CurrentState = _stateEnum;
        _controller.IsJumping = true;

        _jumpStates = JumpStates.Jump;
        _jumpOffset = _controller.transform.position.y + 1.5f;

        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.PlayerSFX.Clips["Jump"], false);
        _canDoubleJump = PlayerPrefs.GetInt("DoubleJump") == 1 ? true : false;

        if (_isJumpedOnce)
            _isJumpedTwice = true;
        _isJumpedOnce = true;


    }
    public override void Update()
    {

        switch (_jumpStates)
        {
            case JumpStates.Jump:
                Jump();
                break;
            case JumpStates.Fall:
                Fall();
                break;
        }

        if (_controller.TryToChangeState(NewState(), _stateEnum))
            return;
        _controller.IsJumping = false;
        _controller.Animator.SetFloat("Jump", _controller.YDirection, 0.1f, Time.deltaTime);

        if (_controller.IsOnGround())
            _controller.TryToChangeState(_controller.GroundRootState, _stateEnum);

    }

    PlayerBaseState NewState()
    {
        if (IsMoving())
            return _controller.JumpWalkState;

        return _controller.JumpIdleState;
    }

    private void Jump()
    {
        if (_jumpOffset - _controller.transform.position.y < 0.1f)
        {
            _jumpStates = JumpStates.Fall;
        }
        else
        {
            float jumpForce = Mathf.Sqrt(6 * _speed * _multiplier);
            _controller.transform.Translate(Vector2.up * jumpForce * Time.deltaTime);
            if(_controller.transform.position.y > _jumpOffset)
                _controller.transform.position = new Vector3(_controller.transform.position.x, _jumpOffset,_controller.transform.position.z);
            _multiplier -= 15 * Time.deltaTime;
            if (_multiplier < 0) _multiplier = 0;
        }

    }
    private void Fall()
    {
        float jumpForce = Mathf.Sqrt(6 * _speed * _multiplier);
        _controller.transform.Translate(Vector2.down * jumpForce * Time.deltaTime);
        _multiplier += 8 * Time.deltaTime;
        if (_multiplier > 3) _multiplier = 3;
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
