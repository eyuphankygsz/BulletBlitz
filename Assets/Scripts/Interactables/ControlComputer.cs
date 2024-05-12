using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ControlComputer : Interactable
{
    private ComputerBehaviorBase _computerBehaviour;

    private void Awake()
    {
        _computerBehaviour = GetComponent<ComputerBehaviorBase>();
    }
    public override void OnEnabled()
    {
        if (_computerBehaviour.GrantAction())
            _computerBehaviour.ComputerBehavior();
    }

    public override void OnTrigger(out Interactable interactable)
    {
        interactable = this;
    }


}
