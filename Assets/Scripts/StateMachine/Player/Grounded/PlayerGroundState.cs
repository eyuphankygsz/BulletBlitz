using UnityEngine;

public class PlayerGroundState : PlayerBaseState
{

    public PlayerGroundState(PlayerController controller) : base(controller) { }

    PlayerController.States _stateEnum = PlayerController.States.Ground;
    protected float _speed = 3;

    protected PlayerStateManager _player;

    public override void EnterState(PlayerStateManager player)
    {
        if(_player == null)
            _player = player;

        _controller.CurrentState = _stateEnum;
        _controller.Animator.SetBool("OnGround",true);

    }

    public override void Update()
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
        if (!_controller.IsOnGround())
        {
            _controller.transform.Translate(Vector2.down * _speed * 2 * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.Space))
            _controller.TryToJump();
    }
    public override void ExitState() { }
}