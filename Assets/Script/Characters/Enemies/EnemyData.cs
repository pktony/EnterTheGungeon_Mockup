using UnityEngine;

/// <summary>
/// 적 정보를 담은 클래스 
/// </summary>
[System.Serializable]
public class EnemyData
{
    public string EnemyName = "Enemy";
    public int EnemyId = 0;
    public GameObject prefab = null;
    public int poolSize = 0;
}
