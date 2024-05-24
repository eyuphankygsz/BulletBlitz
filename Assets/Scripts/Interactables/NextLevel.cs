using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : Interactable
{
    private bool _done;

    public override void OnEnabled()
    {

    }

    public override bool TriggerEnter(out Interactable interactable)
    {
        interactable = null;
        if (_done)
            return false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        return false;
    }
    public override void TriggerExit()
    {
        throw new System.NotImplementedException();
    }
}
