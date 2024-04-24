using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Transform _weaponTransform;
    public void Start()
    {
        ChangeWeapon();
    }
    public void ChangeWeapon()
    {
        WeaponManager.Setup(_weaponTransform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerPrefs.SetString("CurrentWeapon",
                PlayerPrefs.GetString("CurrentWeapon") == "Pistol" ? "AssaultRifle" : "Pistol"
                );
            ChangeWeapon();
        }
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
