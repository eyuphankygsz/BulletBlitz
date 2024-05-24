using UnityEngine;

public class PlayerJumpedWalkState : PlayerJumpedState
{
    bool _initialized;
    
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    public override void EnterState(PlayerStateManager player) { if (!_initialized) SetStats(); }
    void SetStats()
    {

    }
    public override void StateUpdate()
    {
        Move();
        _controller.JumpRootState.StateUpdate();

    }
    void Move()
    {
        float direction = _controller.GetAxis();
        _controller.RB.velocity = new Vector2(direction * _controller.Speed * Time.deltaTime , _controller.RB.velocity.y);
    }
    public override void ExitState()
    {
        _controller.JumpRootState.ExitState();
    }
}
