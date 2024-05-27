using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidDeadState : EnemyBaseState
{
    EnemyController.States _currentState = EnemyController.States.Dead;

    public KidDeadState(EnemyController controller) : base(controller) { }

    public override void EnterState(EnemyStateManager enemy)
    {
        EnemySoundHolder.Instance.PlayAudio(EnemySoundHolder.Instance.EnemySFXDictionary["Kid"].Clips["Death"], false);
        _controller.CurrentState = _currentState;
        _controller.DeadEvents();

        GameObject.Instantiate(_controller.DestroyParticle, _controller.transform.position, Quaternion.identity);

        _controller.DropItems();
        _controller.gameObject.SetActive(false);
    }

    public override void ExitState() { }

    public override void Update() { }
}
