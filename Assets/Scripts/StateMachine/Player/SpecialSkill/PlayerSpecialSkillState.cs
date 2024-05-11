using UnityEngine;

public class PlayerSpecialSkillState : PlayerBaseState
{
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    PlayerController.States _stateEnum = PlayerController.States.Special;

    private int direction;
    private float _timer, _maxTime = 1;
    private float _speed = 3;
    public override void EnterState(PlayerStateManager player)
    {
        _controller.CurrentState = _stateEnum;
        _controller.Animator.SetTrigger("SpecialAttack");
        direction = _controller.transform.localScale.x > 0 ? 1 : -1;
        _timer = _maxTime;
        GameManager.Instance.SpecialSkill = true;
    }
    public override void StateUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        _controller.transform.Translate(Vector2.right * direction * 5 * _speed * Time.deltaTime);
        //Check Walls
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            GameManager.Instance.SpecialSkill = false;
            _controller.TryToChangeState(_controller.GroundRootState, _stateEnum);
        }
    }
    public override void ExitState() 
    {
        _controller.Animator.SetTrigger("SpecialStop");
        _controller.GetComponent<PlayerSpecialSkill>().Stop();
    }

}
