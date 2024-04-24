using UnityEngine;

public class PlayerJumpedWalkState : PlayerJumpedState
{
    private float _speed = 3;
    bool _initialized;
    public PlayerJumpedWalkState(PlayerController controller) : base(controller){}

    public override void EnterState(PlayerStateManager player) { if (!_initialized) SetStats(); }
    void SetStats()
    {
        //if (!PlayerPrefs.HasKey("Speed_Player"))
        //    PlayerPrefs.SetFloat("Speed_Player", 3);

        //_speed = PlayerPrefs.GetFloat("Speed_Player");
    }
    public override void Update()
    {
        Move();
        _controller.JumpRootState.Update();

    }
    void Move()
    {
        float direction = _controller.GetAxis();
        float wallDistance = _controller.IsWallAhead();

        if (wallDistance != 0)
            if (Mathf.Sign(direction) == Mathf.Sign(wallDistance))
                direction = 0;
        _controller.transform.Translate(direction * Vector2.right * _speed * Time.deltaTime);
    }
    public override void ExitState()
    {
        _controller.JumpRootState.ExitState();
    }
}
