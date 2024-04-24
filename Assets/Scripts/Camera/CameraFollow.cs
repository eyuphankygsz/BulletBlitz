using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    Transform _target;
    Vector3 _velocity;
    
    [SerializeField] bool _freezeY;
    [SerializeField] Vector3 _offset;
    [SerializeField] float _xMin,_xMax, _yMin, _yMax; 
    [SerializeField] float _smoothTime;

    // Start is called before the first frame update
    void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_target.localScale.x > 0)
            _offset.x = Mathf.Abs(_offset.x);
        else if (_target.localScale.x < 0)
            _offset.x = -Mathf.Abs(_offset.x);


        Vector3 targetPos = _target.position + _offset;
        targetPos.z = transform.position.z;
        targetPos.x = Mathf.Clamp(targetPos.x, _xMin, _xMax);
        targetPos.y = Mathf.Clamp(targetPos.y, _yMin, _yMax);
        

        if (_freezeY)
            targetPos.y = transform.position.y;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, _smoothTime);
    }

}
