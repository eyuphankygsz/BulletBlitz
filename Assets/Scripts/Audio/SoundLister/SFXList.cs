using System;
using System.Collections.Generic;
using UnityEngine;

public class SFXList : MonoBehaviour
{
    [SerializeField] private ClipDictionary _clips;

    private Dictionary<string, AudioClip> _newClip;
    public Dictionary<string, AudioClip> Clips
    {
        get
        {
            if(_newClip == null)
                _newClip = _clips.ToDictionary();
            
            return _newClip;
        }
    }
}

[Serializable]
public class ClipDictionary
{
    [SerializeField] ClipDictionaryItem[] clips;

    public Dictionary<string, AudioClip> ToDictionary()
    {
        Dictionary<string,AudioClip> newDictionary = new Dictionary<string, AudioClip>();
        for (int i = 0; i < clips.Length; i++)
        {
            newDictionary.Add(clips[i].name, clips[i].clip);
        }
        return newDictionary;
    }
}

[Serializable] 
public class ClipDictionaryItem
{
    public string name;
    public AudioClip clip;
}