using UnityEngine;

public class PlayerJumpedIdleState : PlayerJumpedState
{
    public PlayerJumpedIdleState(PlayerController controller) : base(controller){}

    public override void EnterState(PlayerStateManager player)
    {

        //_controller._jumpedRootState.EnterState(player);
    }
    public override void Update()
    {
        _controller.JumpRootState.Update();
    }
    public override void ExitState()
    {
        _controller.JumpRootState.ExitState();
    }
}
