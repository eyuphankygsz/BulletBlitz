using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserBar : MonoBehaviour
{
    [SerializeField] private Animator[] _laserAnimations;
    private bool[] _laserActive;
    [SerializeField] private UnityEvent[] _activeEvents;
    [SerializeField] private UnityEvent[] _deactiveEvents;
    private void Awake()
    {
        _laserActive = new bool[_laserAnimations.Length];
    }
    public void ChangeLasers(int index)
    {
        _laserActive[index] = !_laserActive[index];
        _laserAnimations[index].SetBool("On", _laserActive[index]);

        for (int i = 0; i < _laserActive.Length; i++)
            if (!_laserActive[i])
            {
                for (int j = 0; j < _deactiveEvents.Length; j++)
                    _deactiveEvents[j].Invoke();
                return;
            }

        for (int i = 0; i < _activeEvents.Length; i++)
            _activeEvents[i].Invoke();
    }
}
