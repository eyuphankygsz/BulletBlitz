using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    bool _jump;

    private float _gravity;

    private float _velocityY, _velocityX;
    private int _yDirection, _xDirection;

    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    public void StartJump(float yStrength, float xStrength, float gravity)
    {
        _velocityY = yStrength;
        _velocityX = xStrength;

        _yDirection = (int)Mathf.Sign(_velocityY);
        _xDirection = (int)Mathf.Sign(_velocityX);

        _gravity = gravity;

        Debug.Log($"JUMP: Y: {_velocityY} X: {_velocityX} G: {_gravity}");
        _controller.CanCheckGround = false;
        _jump = true;
    }
    public void StopJump()
    {
        _jump = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (!_jump) return;

        _velocityY += _gravity * Time.deltaTime;
        if (_velocityX > 0)
            _velocityX -= _xDirection * Time.deltaTime;

        if ((_velocityY <= 0))
            _controller.CanCheckGround = true;

        Vector3 moveVelocity = new Vector3(_velocityX, _velocityY, 0);
        transform.Translate(moveVelocity * Time.deltaTime);
    }
}
