using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour, ButtonBase
{
    [SerializeField] private Sprite _on, _off;
    [SerializeField] string _name;

    private void Start()
    {
        SetActivate();
    }

    public void OnActivate()
    {
        if (PlayerPrefs.GetString(_name) == "On")
            PlayerPrefs.SetString(_name, "Off");
        else
            PlayerPrefs.SetString(_name, "On");

        SetActivate();
    }

    void SetActivate()
    {
        if (PlayerPrefs.GetString(_name) == "On")
        {
            transform.GetChild(0).GetComponent<Image>().sprite = _on; 
            EnemySoundHolder.Instance.GameMixer.SetFloat(_name + "Volume", Mathf.Log10(1) * 20);
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().sprite = _off;
            EnemySoundHolder.Instance.GameMixer.SetFloat(_name + "Volume", Mathf.Log10(0.0001f) * 20);
        }


    }
}
