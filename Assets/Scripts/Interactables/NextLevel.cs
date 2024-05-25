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

    public override bool TriggerEnter(out Interactable interactable, Collider2D collider)
    {
        interactable = null;
        if (_done)
            return false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        return false;
    }
    public override void TriggerExit(Collider2D collider)
    {
        throw new System.NotImplementedException();
    }
}
