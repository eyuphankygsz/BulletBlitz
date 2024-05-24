using UnityEngine;

public class PlayerGroundIdleState : PlayerGroundState
{
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    public override void EnterState(PlayerStateManager player)
    {
        if (_controller.IsOnGround())
            _controller.RB.velocity = new Vector2(0, 0);
    }
    public override void StateUpdate()
    {
        _controller.Animator.SetFloat("MoveSpeed", 0);
        _controller.GroundRootState.StateUpdate();
    }

    public override void StateFixedUpdate()
    {

        if (_controller.IsOnGround())
            _controller.RB.velocity = new Vector2(0, 0);
        else
            _controller.RB.velocity = new Vector2(0, _controller.RB.velocity.y);

    }
    public override void ExitState()
    {
        _controller.GroundRootState.ExitState();
    }
}
