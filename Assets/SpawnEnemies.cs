using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemy;


    public void Spawn(Vector3 pos)
    {
        Instantiate(enemy, pos, Quaternion.identity);
    }
}
