using UnityEngine;

public class PlayerGroundWalkState : PlayerGroundState
{
    public PlayerGroundWalkState(PlayerController controller) : base(controller) { }

    public override void Update()
    {
        if (_controller.GetAxis() != 0)
            _controller.Animator.SetFloat("MoveSpeed", 1);

        Move();

        _controller.GroundRootState.Update();
    }
    private void Move()
    {
        float direction = _controller.GetAxis();
        float wallDistance = _controller.IsWallAhead();

        if (wallDistance != 0)
            if (Mathf.Sign(direction) == Mathf.Sign(wallDistance))
                direction = 0;

        _controller.transform.Translate(direction * Vector2.right * _speed * Time.deltaTime);

    }

    public override void EnterState(PlayerStateManager player) { }

    public override void ExitState() =>
        _controller.GroundRootState.ExitState();

}
