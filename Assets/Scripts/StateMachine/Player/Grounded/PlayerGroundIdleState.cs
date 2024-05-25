using UnityEngine;

public class PlayerGroundIdleState : PlayerGroundState
{
    private PlayerController _controller;
    private bool _stopMoving;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    public override void EnterState(PlayerStateManager player) { }
    public override void StateUpdate()
    {
        _controller.Animator.SetFloat("MoveSpeed", 0);
        _controller.GroundRootState.StateUpdate();
    }

    public override void StateFixedUpdate()
    {

        if (_controller.IsOnGround())
        {
            if (_stopMoving)
            {
                _controller.RB.constraints = RigidbodyConstraints2D.FreezeAll;
                _controller.RB.velocity = new Vector2(0, 0);
            }
            _stopMoving = true;
        }
        else
        {
            _controller.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
            _stopMoving = false;
            _controller.RB.velocity = new Vector2(0, _controller.RB.velocity.y);
        }

    }
    public override void ExitState()
    {
        _controller.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
        _stopMoving = false;
        _controller.GroundRootState.ExitState();
    }
}
