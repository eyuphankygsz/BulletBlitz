using System.Collections;
using UnityEngine;

public class Lasers : MonoBehaviour
{
    private BoxCollider2D _collider;
    private Coroutine _timerCoroutine;
    private WaitForSeconds _delay;
    [SerializeField] private float _time;
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _delay = new WaitForSeconds(_time);
    }
    private void OnOpen()
    {
        _animator.SetTrigger("Open");
        _collider.enabled = true;
    }
    private void OnClose()
    {
        _animator.SetTrigger("Close");
        _collider.enabled = false;
    }
    public void TryToOpen(bool withTime)
    {

        if (withTime)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("LaserClose"))
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
        if (_collider.enabled)
            OnClose();
        else
            OnOpen();
    }

    IEnumerator WaitPortal()
    {
        yield return _delay;
        OnOpen();
    }
}
