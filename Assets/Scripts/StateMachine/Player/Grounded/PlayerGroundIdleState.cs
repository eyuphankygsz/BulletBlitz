using UnityEngine;

public class PlayerGroundIdleState : PlayerGroundState
{
    public PlayerGroundIdleState(PlayerController controller) : base(controller) { }

    public override void EnterState(PlayerStateManager player) { }
    public override void Update()
    {
        _controller.Animator.SetFloat("MoveSpeed", 0);

        _controller.GroundRootState.Update();
    }
    public override void ExitState()
    {
        _controller.GroundRootState.ExitState();
    }
}
