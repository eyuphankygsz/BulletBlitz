using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillerTree : MonoBehaviour
{
    private bool _approach;
    private Transform _player;
    private Vector2 _startPos;
    public void Approach()
    {
        _player = GameManager.Instance.Player.transform.GetChild(7);
        _approach = true;

        float direction = GameManager.Instance.Player.transform.localScale.x > 0 ? 1 : -1;
        float posx = GameManager.Instance.Player.transform.position.x + (20 * direction);
        transform.position = new Vector3(posx, GameManager.Instance.Player.transform.position.y + 2, transform.position.z);
        _startPos = transform.position;
        gameObject.SetActive(true);
    }
    private void Update()
    {
        if (_approach)
        {
            Vector3 target = new Vector3(_player.position.x, _player.position.y + 2, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, 0.1f);
            if (Vector2.Distance(transform.position, target) < 1)
                SkillManager.Instance.OpenCanvas();
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(_startPos.x * -1, _startPos.y), 0.1f);

    }
    public void GoBack()
    {
        _approach = false;
    }
}
