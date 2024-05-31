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


        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.PlayerSFX.Clips["Hit"], false);

        _controller.CurrentState = _stateEnum;
        _controller.CanCheckGround = false;


        if (_controller.HitObject.tag == "Bullet")
            _controller.HitObject.gameObject.SetActive(false);

        _controller.Animator.SetTrigger("JumpTrigger");
        _controller.Animator.SetBool("OnGround", false);

        int directionX = _controller.transform.position.x - _controller.HitObject.transform.position.x >= 0 ? 1 : -1;
        _controller.RB.velocity = Vector2.zero;
        _controller.StartJump(directionX);
    }

    void CheckGround()
    {
        if (_controller.IsOnGround() && _controller.CanCheckGround)
        {
            _player.SwitchState(_controller.GroundRootState);
            _controller.Animator.SetBool("OnGround", true);
        }
    }
    public override void ExitState()
    {

        _controller.Animator.SetBool("OnGround", true);
        _isHitting = false;
    }

    public override void StateUpdate()
    {
        CheckGround();
    }

    public override void StateFixedUpdate()
    {

    }
}
