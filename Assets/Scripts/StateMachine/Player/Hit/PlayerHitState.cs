using UnityEngine;

public class PlayerHitState : PlayerBaseState
{

    PlayerController.States _stateEnum = PlayerController.States.Hit;
    bool _isHitting;
    
    private float _yVelocity = 3, _xVelocity = 2;
    private float _gravity = -12;

    private PlayerStateManager _player;


    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }
    public override void EnterState(PlayerStateManager player)
    {
        if (_player == null)
            _player = player;

        if (_isHitting)
            return;
        else
            _isHitting = true;

        _controller.CanCheckGround = false;

        float directionX = _controller.transform.position.x - _controller.HitObject.transform.position.x >= 0 ? 1 : -1;

        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.PlayerSFX.Clips["Hit"], false);

        _controller.CurrentState = _stateEnum;
        _controller.CanCheckGround = false;


        if (_controller.HitObject.tag == "Bullet")
            _controller.HitObject.gameObject.SetActive(false);

        _controller.Animator.SetTrigger("HitTrigger");

        _controller.StartJump(_yVelocity, _xVelocity, _gravity);
    }

    public override void StateFixedUpdate()
    {
        CheckGround();
    }


    void CheckGround()
    {
        if (_controller.IsOnGround() && _controller.CanCheckGround)
            _player.SwitchState(_controller.GroundRootState);
    }
    public override void ExitState()
    {
        _isHitting = false;
    }

    public override void StateUpdate()
    {

    }
}
