using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract bool TriggerEnter(out Interactable interactable, Collider2D collider);
    public abstract void TriggerExit(Collider2D collider);
    public abstract void OnEnabled();
}
