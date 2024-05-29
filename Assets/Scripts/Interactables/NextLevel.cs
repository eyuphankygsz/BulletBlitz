using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : Interactable
{
    private bool _done;
    [SerializeField] private string _sceneName;

    public override void OnEnabled()
    {

    }

    public override bool TriggerEnter(out Interactable interactable, Collider2D collider)
    {
        interactable = null;
        if (_done)
            return false;
        _done = true;

        PlayerPrefs.SetInt("CorruptedSilver", PlayerPrefs.GetInt("CorruptedSilver") + GameManager.Instance.Player.GetComponent<PlayerController>().GetCorruptedSilver);

        Debug.Log(PlayerPrefs.GetInt("CorruptedSilver"));

        if (string.IsNullOrEmpty(_sceneName))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(_sceneName);

        return false;
    }
    public override void TriggerExit(Collider2D collider)
    {
        throw new System.NotImplementedException();
    }
}
