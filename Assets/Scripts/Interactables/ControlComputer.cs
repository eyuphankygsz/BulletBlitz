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
            _computerBehaviour.GetBehavior();
    }

    public override bool TriggerEnter(out Interactable interactable, Collider2D collider)
    { 
        interactable = this;
        return false;
    }
    public override void TriggerExit(Collider2D collider) { }

}
