using UnityEngine;

public class PlayerJumpedWalkState : PlayerJumpedState
{
    private float _speed = 3;
    bool _initialized;
    
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    public override void EnterState(PlayerStateManager player) { if (!_initialized) SetStats(); }
    void SetStats()
    {
        //if (!PlayerPrefs.HasKey("Speed_Player"))
        //    PlayerPrefs.SetFloat("Speed_Player", 3);

        //_speed = PlayerPrefs.GetFloat("Speed_Player");
    }
    public override void StateUpdate()
    {
        Move();
        _controller.JumpRootState.StateUpdate();

    }
    public override void StateFixedUpdate()
    {
        _controller.JumpRootState.StateFixedUpdate();
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
