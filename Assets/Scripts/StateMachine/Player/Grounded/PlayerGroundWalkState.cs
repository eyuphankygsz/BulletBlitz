using UnityEngine;

public class PlayerGroundWalkState : PlayerGroundState
{
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    public override void StateUpdate()
    {
        if (_controller.GetAxis() != 0)
            _controller.Animator.SetFloat("MoveSpeed", 1);


        _controller.GroundRootState.StateUpdate();
    }
    public override void StateFixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        float direction = _controller.GetAxis();
        _controller.RB.velocity= new Vector2(direction * _controller.Speed * Time.deltaTime , _controller.RB.velocity.y);

    }

    public override void EnterState(PlayerStateManager player)
    {
        _controller.RB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void ExitState() =>
        _controller.GroundRootState.ExitState();

}
