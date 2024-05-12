using UnityEngine;
using UnityEngine.Events;

public abstract class ComputerBehaviorBase : MonoBehaviour
{
    protected Animator _animator;
    protected bool _isActive;
    [SerializeField] protected bool _bothWayEvents;
    [SerializeField] protected UnityEvent[] _activateEvents, _deactivateEvents;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public virtual void GetBehavior()
    {
        if (!_isActive || !_bothWayEvents)
            OnActivate();
        else
            OnDeactivate();
    }
    protected void ComputerSettings(bool isActive)
    {
        _isActive = isActive;
        _animator.SetBool("IsActive", isActive);
    }
    public abstract bool GrantAction();
    protected abstract void OnActivate();
    protected abstract void OnDeactivate();
}
