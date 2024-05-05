using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract void OnTrigger(out Interactable interactable);
    public abstract void OnEnabled();
}
