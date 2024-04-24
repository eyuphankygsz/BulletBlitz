using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class EnemySoundHolder : MonoBehaviour
{
    public static EnemySoundHolder Instance;
    private List<AudioSource> _sources;
    private AudioSource _bgm;
    private int _sourceCounter, _maxSource = 10;

    [SerializeField] private SFXDictionary sfxDictionary;

    public SFXList PlayerSFX { get; private set; }
    [SerializeField] private GameObject _playerSfx;
    public SFXList TreeSFX { get; private set; }
    [SerializeField] private GameObject _treeSfx;

    public Dictionary<string, SFXList> EnemySFXDictionary { get; private set; }
    private List<string> _createdSfxDicts;

    [SerializeField] private AudioClip _backgroundMusic;
    [field: SerializeField] public AudioMixer GameMixer { get; private set; }

    //TO PLAY ENEMY AUDIO, USE THE FOLLOWING CODE SAMPLE!
    //        SoundManager.Instance.PlayAudio(SoundManager.Instance.EnemySFXDictionary[TAG].Clips[CLIP_NAME] ,false);
    //        SoundManager.Instance.PlayAudio(SoundManager.Instance.PlayerSFX.Clips["Hit"],false);
    void Awake()
    {
        if (FindObjectsOfType<EnemySoundHolder>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SetAudioSources();

    }

    public AudioSource PlayAudio(AudioClip clip, bool isLooping)
    {
        _sourceCounter =
                    (_sourceCounter + 1 == _sources.Count) ? 0 : _sourceCounter + 1;

        if (_sources[_sourceCounter].clip == null || !_sources[_sourceCounter].isPlaying)
        {
            _sources[_sourceCounter].clip = clip;
            _sources[_sourceCounter].loop = isLooping;
            _sources[_sourceCounter].Play();
            return _sources[_sourceCounter];
        }
        else
            return PlayAudio(clip, isLooping, 1);

    }
    AudioSource PlayAudio(AudioClip clip, bool isLooping, int attempt)
    {
        _sourceCounter =
                    (_sourceCounter + 1 == _sources.Count) ? 0 : _sourceCounter + 1;

        if (attempt == _sources.Count)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            _sources.Add(newSource);

            newSource.clip = clip;
            newSource.loop = isLooping;
            return newSource;
        }
        else if (!_sources[_sourceCounter].isPlaying)
        {
            _sources[_sourceCounter].clip = clip;
            _sources[_sourceCounter].loop = isLooping;
            _sources[_sourceCounter].Play();
            return _sources[_sourceCounter];
        }
        else
        {
            return PlayAudio(clip, isLooping, attempt + 1);
        }

    }

    public void StopAllAudio()
    {
        for (int i = 0; i < _sources.Count; i++)
        {
            _sources[i].Stop();
        }
    }

    bool IsSoudActive(string which)
    {
        return PlayerPrefs.GetString(which) == "on";
    }
    void SetAudioSources()
    {
        _sources = new List<AudioSource>();
        for (int i = 0; i < _maxSource; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = GameMixer.FindMatchingGroups("Sfx")[0];
            _sources.Add(source);
        }
        _sourceCounter = 0;

        _bgm = gameObject.AddComponent<AudioSource>();
        _bgm.clip = _backgroundMusic;
        _bgm.loop = true;
        _bgm.volume = 0.5f;
        _bgm.outputAudioMixerGroup = GameMixer.FindMatchingGroups("Music")[0];
        _bgm.Play();
    }

    public void CreateSFXLists()
    {
        _createdSfxDicts = new List<string>();
        EnemySFXDictionary = new Dictionary<string, SFXList>();
        
        List<GameObject> enemies = GameManager.Instance.GetEnemies();
        Dictionary<string, GameObject> sfxdict = sfxDictionary.ToDictionary();

        for (int i = 0; i < enemies.Count; i++)
            if (!_createdSfxDicts.Contains(enemies[i].tag))
            {
                _createdSfxDicts.Add(enemies[i].tag);
                EnemySFXDictionary.Add(enemies[i].tag, Instantiate(sfxdict[enemies[i].tag]).GetComponent<SFXList>());
            }

        PlayerSFX = Instantiate(_playerSfx).GetComponent<SFXList>();
        TreeSFX = Instantiate(_treeSfx).GetComponent<SFXList>();
    }
}
[Serializable]
public class SFXDictionary
{
    [SerializeField] SFXDictionaryItem[] _sfxLists;

    public Dictionary<string, GameObject> ToDictionary()
    {
        Dictionary<string, GameObject> newDictionary = new Dictionary<string, GameObject>();
        for (int i = 0; i < _sfxLists.Length; i++)
        {
            newDictionary.Add(_sfxLists[i].Name, _sfxLists[i].SfxList);
        }
        return newDictionary;
    }
}

[Serializable]
public class SFXDictionaryItem
{
    public string Name;
    public GameObject SfxList;
}
