using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NextLevel : Interactable
{
    private bool _done;
    [SerializeField] private string _sceneName;
    [SerializeField] private UnityEvent _events;

    public override void OnEnabled()
    {

    }

    public override bool TriggerEnter(out Interactable interactable, Collider2D collider)
    {
        interactable = null;

        if (_done)
            return false;
        _done = true;

        if (_events != null)
            _events.Invoke();

        PlayerPrefs.SetInt("CorruptedSilver", PlayerPrefs.GetInt("CorruptedSilver") + GameManager.Instance.Player.GetComponent<PlayerController>().GetCorruptedSilver);

        Debug.Log(PlayerPrefs.GetInt("CorruptedSilver"));

        if (string.IsNullOrEmpty(_sceneName))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(_sceneName);

        return false;
    }
    public void MainMenu()
    {
        PanelManager.Instance.MainMenuPanel();
    }
    public override void TriggerExit(Collider2D collider)
    {
        throw new System.NotImplementedException();
    }
}
