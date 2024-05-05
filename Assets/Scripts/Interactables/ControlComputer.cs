using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ControlComputer : Interactable
{
    [SerializeField] private bool _isActive;
    [SerializeField] private UnityEvent _events;
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public override void OnEnabled()
    {
        _isActive = !_isActive;
        _animator.SetBool("IsActive", _isActive);
        _events.Invoke();
    }

    public override void OnTrigger(out Interactable interactable)
    {
        interactable = this;
    }


}
