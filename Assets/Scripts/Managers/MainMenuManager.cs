using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Transform _weaponTransform;
    [SerializeField] private TextMeshProUGUI _corruptedText;
    public void Start()
    {
        Application.targetFrameRate = 60;
        ChangeWeapon();
    }
    public void ChangeWeapon()
    {
        for (int i = 0; i < _weaponTransform.childCount; i++)
        {
            _weaponTransform.GetChild(i).gameObject.SetActive(false);
            if (_weaponTransform.GetChild(i).gameObject.name == PlayerPrefs.GetString("CurrentWeapon"))
                _weaponTransform.GetChild(i).gameObject.SetActive(true);
        }

    }
    private void OnEnable()
    {
        _corruptedText.text = PlayerPrefs.GetInt("CorruptedSilver").ToString();
    }

    public void StartGame()
    {
        PanelManager.Instance.ClosePanel(PanelManager.Instance.Panels["MainPanel"]);
        PanelManager.Instance.OpenPanel(PanelManager.Instance.Panels["InGamePanel"]);
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
