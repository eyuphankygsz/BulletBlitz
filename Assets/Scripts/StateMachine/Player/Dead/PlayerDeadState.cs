using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{

    private PlayerController.States _stateEnum = PlayerController.States.Dead;
    private PlayerStateManager _player;
    private Vector2 _jumpOffset;
    private JumpStates _deadStates = JumpStates.Jump;
    private float _direction;
    private float _speed = 3;
    
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }
    public override void EnterState(PlayerStateManager player)
    {
        if(_player == null)
            _player = player;

        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.PlayerSFX.Clips["Death"], false);

        _controller.CurrentState = _stateEnum;
        //AddForce
        PlayerPrefs.DeleteKey("CurrentBulletSkill");
        _controller.Animator.SetTrigger("DeadTrigger");

        Vector2 direction = new Vector2(_player.transform.position.x - _controller.HitObject.transform.position.x >= 0 ? 1 : -1, 1);
        _direction = direction.x;
        _jumpOffset = _controller.transform.position + new Vector3(Mathf.Abs(_jumpOffset.x) * Mathf.Sign(direction.x), _jumpOffset.y);
    }


    public override void StateUpdate()
    {
        Dead();
    }
    public override void StateFixedUpdate()
    {
        if(_controller.IsOnGround() && _deadStates == JumpStates.Fall)
            _deadStates = JumpStates.Done;
    }
    private void Jump()
    {
        if (Vector2.Distance(_player.transform.position, _jumpOffset) < 0.1f)
            _deadStates = JumpStates.Fall;
        else
            _controller.transform.Translate((Vector2.right * _direction * _speed) + 
                (Vector2.up * _speed) * Time.deltaTime);
    }
    private void Dead()
    {
        switch(_deadStates)
        {
            case JumpStates.Jump:
                Jump();
                break;
        }
    }
    public override void ExitState() { }
}


enum JumpStates
{
    Jump,
    Fall,
    Done
}