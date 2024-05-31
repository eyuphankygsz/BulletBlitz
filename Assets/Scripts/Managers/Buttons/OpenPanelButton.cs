using UnityEngine;

public class OpenPanelButton : MonoBehaviour, ButtonBase
{
    [SerializeField]
    private GameObject[] _toOpen, _toClose;

    public void OnActivate()
    {
        for (int i = 0; i < _toClose.Length; i++)
            PanelManager.Instance.ClosePanel(_toClose[i]);
     
        for (int i = 0; i < _toOpen.Length; i++)
            PanelManager.Instance.OpenPanel(_toOpen[i]);
    }
}
