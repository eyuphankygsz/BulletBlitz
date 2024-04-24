using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioButton : MonoBehaviour, ButtonBase
{
    [SerializeField] private AudioClip _clip;
    public void OnActivate()
    {
        AudioManager.PlayAudio(_clip);
    }
}
