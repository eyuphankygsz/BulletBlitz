using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract bool TriggerEnter(out Interactable interactable);
    public abstract void TriggerExit();
    public abstract void OnEnabled();
}
