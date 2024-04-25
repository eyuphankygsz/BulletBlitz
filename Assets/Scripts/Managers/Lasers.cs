using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasers : MonoBehaviour
{
    private BoxCollider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }
    public void OnOpen()
    {
        _collider.enabled = true;
    }
    public void OnClose()
    {
        _collider.enabled = false;
    }
}
