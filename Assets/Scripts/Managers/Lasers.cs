using System.Collections;
using UnityEngine;

public class Lasers : MonoBehaviour
{
    private const string ANM_CLOSE = "LaserClose";
    private const string ANM_OPEN = "LaserOpen";

    private BoxCollider2D _collider;
    private Coroutine _timerCoroutine;
    private WaitForSeconds _delay;
    [SerializeField] private float _time;
    private bool _locked, _on;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _delay = new WaitForSeconds(_time);
    }
    public void OnOpen()
    {
        _animator.SetBool("On", true);
    }
    public void OnClose()
    {
        _animator.SetBool("On", false);
    }
    public void EnableCollider()
    {
        _collider.enabled = true;
    }
    public void DisableCollider()
    {
        _collider.enabled = false;
    }
    public void Lock()
    {
        _locked = !_locked;
        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);
    }
    public void TryToOpen(bool withTime)
    {
        if (_locked)
            return;

        if (withTime)
        {
            OnClose();
            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);
            _timerCoroutine = StartCoroutine(WaitPortal());
        }
        else
        {
            OpenCloseHandler();
        }
    }
    private void OpenCloseHandler()
    {
        _on = !_on;
        if (_on)
            OnClose();
        else
            OnOpen();
    }
    public void CertainActivation(bool open)
    {
        if (open)
        {
            _on = true;
            OnClose();
        }
        else
        {
            _on = false;
            OnOpen();
        }
    }

    IEnumerator WaitPortal()
    {
        yield return _delay;
        OnOpen();
    }
}
