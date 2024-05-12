using UnityEngine;
using UnityEngine.Events;

public abstract class ComputerBehaviorBase : MonoBehaviour
{
    protected Animator _animator;
    protected bool _isActive;

    [SerializeField] protected UnityEvent[] _events;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public abstract bool GrantAction();
    public abstract void ComputerBehavior();
}
