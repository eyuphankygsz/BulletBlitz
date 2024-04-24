using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour, ButtonBase
{
    [SerializeField] string sceneName;
    public void OnActivate()
    {
        if (string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
        else
            SceneManager.LoadScene(sceneName);
    }
}
