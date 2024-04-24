using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerHitState : PlayerBaseState
{
    public PlayerHitState(PlayerController controller) : base(controller) { }


    PlayerController.States _stateEnum = PlayerController.States.Hit;
    bool _isHitting;
    float _defaultGroundCheckTime = 0.2f;
    private JumpStates _hitStates = JumpStates.Jump;
    private Vector2 _jumpOffset, _jumpMax = new Vector2(0.5f, 0.5f);
    private float _speed = 3;

    private Vector3 _oldPos;
    private PlayerStateManager _player;
    public override void EnterState(PlayerStateManager player)
    {
        if (_player == null)
            _player = player;

        if (_isHitting)
            return;
        else
            _isHitting = true;

        float directionX = _controller.transform.position.x - _controller.HitObject.transform.position.x >= 0 ? 1 : -1;
        _jumpOffset = _controller.transform.position + new Vector3(_jumpMax.x * Mathf.Sign(directionX), _jumpMax.y);
        _oldPos = Vector3.zero;

        Debug.Log(_jumpOffset);
        _hitStates = JumpStates.Jump;
        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.PlayerSFX.Clips["Hit"], false);

        _controller.CurrentState = _stateEnum;
        _controller.IsJumping = true;


        if (_controller.HitObject.tag == "Bullet")
            _controller.HitObject.gameObject.SetActive(false);

        _controller.StartGroundCheckCountdown(_defaultGroundCheckTime);
        _controller.Animator.SetTrigger("HitTrigger");
        Hit(player);
    }
    public override void Update()
    {
        switch (_hitStates)
        {
            case JumpStates.Jump:
                Jump();
                break;
            case JumpStates.Fall:
                Fall();
                break;
        }
        CheckGround();
        _controller.IsJumping = false;
    }

    private void Jump()
    {
        if (Vector2.Distance(_controller.transform.position, _jumpOffset) < 0.1f || _controller.transform.position == _oldPos)
            _hitStates = JumpStates.Fall;
        else
        {
            _oldPos = _controller.transform.position;

            Vector2 newDirection = (new Vector3(_jumpOffset.x,_jumpOffset.y,0) - _controller.transform.position).normalized;
            float wallDirection = _controller.IsWallAhead();
            if (Mathf.Sign(wallDirection) == Mathf.Sign(newDirection.x) && wallDirection != 0)
                newDirection.x = 0;

            Debug.Log(newDirection);
            _controller.transform.Translate(((Vector2.right * newDirection.x * _speed) +
                 (Vector2.up * newDirection.y * _speed)) * Time.deltaTime);
        }
    }
    private void Fall()
    {
        if (_controller.IsOnGround())
            _hitStates = JumpStates.Done;
        else
            _controller.transform.Translate(Vector2.down * _speed * Time.deltaTime);
    }
    void Hit(PlayerStateManager player)
    {
        switch (_hitStates)
        {
            case JumpStates.Jump:
                Jump();
                break;
            case JumpStates.Fall:
                Fall();
                break;
        }
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
}
