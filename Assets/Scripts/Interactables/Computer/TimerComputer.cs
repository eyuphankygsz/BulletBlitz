using System.Collections;
using UnityEngine;

public sealed class TimerComputer : ComputerBehaviorBase
{
    private Coroutine _computerRoutine;
    private Coroutine _eventRoutine;
    [SerializeField] private float _checkSeconds;
    private WaitForSeconds _checkDelay;
    [SerializeField] private float _lineDelay;

    private void Start()
    {
        _checkDelay = new WaitForSeconds(_checkSeconds);
    }
    public override void ComputerBehavior()
    {
        _isActive = true;
        _animator.SetBool("IsActive", _isActive);

        if (_computerRoutine != null)
            StopCoroutine(_computerRoutine);
        _computerRoutine = StartCoroutine(WaitToDeactivate());

        if (_lineDelay != 0)
        {
            if (_eventRoutine != null)
                StopCoroutine(_eventRoutine);
            _eventRoutine = StartCoroutine(InvokeLine());
        }
        else
            for (int i = 0; i < _events.Length; i++)
                _events[i].Invoke();
    }

    public override bool GrantAction()
    {
        Debug.Log(!_isActive);
        return !_isActive;
    }
    private IEnumerator InvokeLine()
    {
        for (int i = 0; i < _events.Length; i++)
        {
            _events[i].Invoke();
            yield return new WaitForSeconds(_lineDelay);
        }
    }
    private IEnumerator WaitToDeactivate()
    {
        yield return _checkDelay;
        _isActive = false;
        _animator.SetBool("IsActive", _isActive);
    }
}
