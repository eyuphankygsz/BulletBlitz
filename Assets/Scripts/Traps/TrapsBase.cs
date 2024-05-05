using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapsBase : MonoBehaviour
{
    public abstract void TrapActive();
    public abstract void TrapDeactive();
    public abstract void OnCollide(GameObject affected);
}
