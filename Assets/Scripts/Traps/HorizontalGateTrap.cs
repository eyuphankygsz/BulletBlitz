using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalGateTrap : TrapsBase
{
    private float _startX;
    private int _direction;
    private bool _isActive = false, _stopped = true;
    [SerializeField] private float _speed;
    [SerializeField] private float _activatedX;

    private void Awake()
    {
        _startX = transform.position.x;
    }
    public override void OnCollide(GameObject affected)
    {

    }
    public void TrapActivity()
    {
        _isActive = !_isActive;
        _stopped = false;
        _direction = _isActive ? Mathf.RoundToInt(Mathf.Sign(_activatedX - transform.position.x)) : Mathf.RoundToInt(Mathf.Sign(_startX - transform.position.x));
    }
    private void Update()
    {
        if (_stopped) return;

        transform.Translate(new Vector3(_speed * _direction, 0, 0) * Time.deltaTime);

        if (_isActive)
        {
            if ((_direction == 1 && transform.position.x > _activatedX) || (_direction == -1 && transform.position.x < _activatedX))
            {
                _stopped = true;
                transform.position = new Vector3(_activatedX, transform.position.y, transform.position.z);
            }
        }
        else if ((_direction == 1 && transform.position.x > _startX) || (_direction == -1 && transform.position.x < _startX))
        {
            _stopped = true;
            transform.position = new Vector3(_startX, transform.position.y, transform.position.z);
        }
    }
    public override void TrapActive()
    {

    }

    public override void TrapDeactive()
    {

    }
}
