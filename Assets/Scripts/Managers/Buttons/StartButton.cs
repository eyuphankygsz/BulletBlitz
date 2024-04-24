using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour, ButtonBase
{
    public void OnActivate()
    {
        int level = PlayerPrefs.HasKey("Level") ? PlayerPrefs.GetInt("Level") : 1;    
        SceneManager.LoadScene(level);
        PanelManager.Instance.ClosePanel(PanelManager.Instance.Panels["MainPanel"]);
        PanelManager.Instance.OpenPanel(PanelManager.Instance.Panels["InGamePanel"]);
    }
}
