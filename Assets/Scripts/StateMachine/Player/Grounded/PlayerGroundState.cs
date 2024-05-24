using UnityEngine;

public class PlayerGroundState : PlayerBaseState
{

    PlayerController.States _stateEnum = PlayerController.States.Ground;

    protected PlayerStateManager _player;
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }
    public override void EnterState(PlayerStateManager player)
    {
        if (_player == null)
            _player = player;

        _controller.CurrentState = _stateEnum;
        _controller.Animator.SetBool("OnGround", true);

    }

    public override void StateUpdate()
    {
        _controller.TryToChangeState(NewState(), _stateEnum);
        CheckGround();
    }

    PlayerBaseState NewState()
    {
        if (_controller.GetAxis() != 0 && !_controller.IsGroundSloope)
            return _controller.GroundWalkState;

        return _controller.GroundIdleState;
    }


    void CheckGround()
    {
        if (_controller != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _controller.TryToJump();
        }
    }
    public override void ExitState() { }

    public override void StateFixedUpdate() { }
}