using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsButton : MonoBehaviour, ButtonBase
{
    [SerializeField] private GameObject _mainMenuButton;
    public void OnActivate()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            _mainMenuButton.SetActive(false);
        else
            _mainMenuButton.SetActive(true);
    }
}
