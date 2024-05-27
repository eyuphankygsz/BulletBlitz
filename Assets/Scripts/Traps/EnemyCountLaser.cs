using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCountLaser : Lasers
{
    [SerializeField]
    private List<GameObject> _enemies;

    public void RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
        if (_enemies.Count == 0)
            CertainActivation(true);
    }
   
}
