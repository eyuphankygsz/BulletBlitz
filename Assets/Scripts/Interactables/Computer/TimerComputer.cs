using System.Collections;
using TMPro;
using UnityEngine;

public sealed class TimerComputer : ComputerBehaviorBase
{
    private Coroutine _computerRoutine;
    private Coroutine _eventRoutine;
    [SerializeField] private float _checkSeconds;
    private WaitForSeconds _checkDelay = new WaitForSeconds(1);
    [SerializeField] private float _lineDelay;

    [SerializeField] private GameObject _timerCanvas;
    [SerializeField] private TextMeshProUGUI _timerTxt;

    protected override void OnActivate()
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
            for (int i = 0; i < _activateEvents.Length; i++)
                _activateEvents[i].Invoke();
    }
    protected override void OnDeactivate()
    {
        _timerCanvas.SetActive(false);
        return;
    }

    public override bool GrantAction()
    {
        return !_isActive;
    }
    private IEnumerator InvokeLine()
    {
        for (int i = 0; i < _activateEvents.Length; i++)
        {
            _activateEvents[i].Invoke();
            yield return new WaitForSeconds(_lineDelay);
        }
    }
    private IEnumerator WaitToDeactivate()
    {
        _timerCanvas.SetActive(true);
        for (int i = 0; i < _checkSeconds; i++)
        {
            _timerTxt.text = (_checkSeconds - i).ToString();
            yield return _checkDelay;
        }

        _timerCanvas.SetActive(false);
        _isActive = false;
        _animator.SetBool("IsActive", _isActive);
    }
}
