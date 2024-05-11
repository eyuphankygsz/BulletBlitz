using UnityEngine;

public class PlayerGroundIdleState : PlayerGroundState
{
    private PlayerController _controller;

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


    public override void ExitState()
    {
        _controller.GroundRootState.ExitState();
    }
}
