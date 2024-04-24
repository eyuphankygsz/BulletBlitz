using UnityEngine;

public abstract class PlayerBaseState
{
    protected PlayerController _controller;

    public PlayerBaseState(PlayerController controller)
    {
        _controller = controller;
    }

    public abstract void EnterState(PlayerStateManager player);
    public abstract void Update();
    public abstract void ExitState();
}
