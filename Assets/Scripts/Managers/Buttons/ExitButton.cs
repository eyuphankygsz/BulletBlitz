using UnityEngine;

public class ExitButton : MonoBehaviour, ButtonBase
{
    public void OnActivate()
    {
        Application.Quit();
    }
}
