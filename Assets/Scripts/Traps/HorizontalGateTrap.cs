using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalGateTrap : TrapsBase
{
    private float _startPos;
    private int _direction;
    private bool _isActive = false, _stopped = true;
    [SerializeField] private float _speed;
    [SerializeField] private float _activatedPos;
    [SerializeField] private bool _vertical;

    private void Awake()
    {
        _startPos = (_vertical ? transform.position.y : transform.position.x);
    }
    public override void OnCollide(GameObject affected)
    {

    }
    public void TrapActivity()
    {
        _stopped = false;
        _direction = _isActive ? Mathf.RoundToInt(Mathf.Sign(_activatedPos - transform.position.x)) : Mathf.RoundToInt(Mathf.Sign(_startPos - transform.position.x));
    }
    private void Update()
    {
        if (_stopped) return;

        if (_vertical)
            transform.Translate(new Vector3(0, _speed * _direction, 0) * Time.deltaTime);
        else
            transform.Translate(new Vector3(_speed * _direction, 0, 0) * Time.deltaTime);

        float pos = (_vertical ? transform.position.y : transform.position.x);
        if (_isActive)
        {

            if ((_direction == 1 && pos > _activatedPos) || (_direction == -1 && pos < _activatedPos))
            {
                _stopped = true;
                if (_vertical)
                    transform.position = new Vector3(transform.position.x, _activatedPos, transform.position.z);
                else
                    transform.position = new Vector3(_activatedPos, transform.position.y, transform.position.z);

            }
        }
        else if ((_direction == 1 && pos > _startPos) || (_direction == -1 && pos < _startPos))
        {
            _stopped = true;
            if (_vertical)
                transform.position = new Vector3(transform.position.x, _startPos, transform.position.z);
            else
                transform.position = new Vector3(_startPos, transform.position.y, transform.position.z);
        }
    }
    public override void TrapActive()
    {
        _isActive = true;
        _stopped = false;

        _direction = Mathf.RoundToInt(Mathf.Sign(_activatedPos - (_vertical ? transform.position.y : transform.position.x)));
    }

    public override void TrapDeactive()
    {
        _isActive = false;
        _stopped = false;
        _direction = Mathf.RoundToInt(Mathf.Sign(_startPos - (_vertical ? transform.position.y : transform.position.x)));
    }
}
