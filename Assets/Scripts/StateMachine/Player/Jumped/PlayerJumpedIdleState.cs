using UnityEngine;

public class PlayerJumpedIdleState : PlayerJumpedState
{
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    public override void EnterState(PlayerStateManager player)
    {
    }
    public override void StateUpdate()
    {
        _controller.JumpRootState.StateUpdate();
    }
    public override void ExitState()
    {
        _controller.JumpRootState.ExitState();
    }
}
