using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public string EnemyName = "Enemy";
    public int EnemyId = 0;
    public GameObject prefab = null;
    public int poolSize = 0;
}
