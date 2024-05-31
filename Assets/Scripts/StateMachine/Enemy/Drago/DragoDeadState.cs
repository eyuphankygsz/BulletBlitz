using UnityEngine;

public class DragoDeadState : EnemyBaseState
{
    EnemyController.States _currentState = EnemyController.States.Dead;


    //GoatBehavior _behavior;

    public DragoDeadState(EnemyController controller) : base(controller) { }

    EnemyStateManager _stateManager;
    public override void EnterState(EnemyStateManager enemy)
    {
        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Drago"].Clips["Death"], false);
        _controller.CurrentState = _currentState;
        _controller.DeadEvents();

        GameObject.Instantiate(_controller.DestroyParticle, _controller.transform.position, Quaternion.identity);

        _controller.DropItems();
        _controller.gameObject.SetActive(false);

    }

    public override void Update() { }
    public override void ExitState(){}

}
