using UnityEngine;

public class CrowDeadState : EnemyBaseState
{


    EnemyController.States _currentState = EnemyController.States.Attack;

    public CrowDeadState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Crow"].Clips["Death"], false);
        _controller.CurrentState = _currentState;

        GameObject.Instantiate(_controller.DestroyParticle, _controller.transform.position, Quaternion.identity);

        _controller.DropItems();
        _controller.gameObject.SetActive(false);
    }

   
    public override void Update() { }
    
    public override void ExitState() { }

}
