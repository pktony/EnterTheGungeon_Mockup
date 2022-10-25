using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Range(2f, 5f)]
    [Header("몬스터 간 스폰 간격 ")]
    public float spawnOffset;
    [Range(1, 3)]
    public int minSpawn = 2;
    [Range(4, 6)]
    public int maxSpawn = 4;
    public GameObject chestPrefab;
    [Range(0f, 1f)]
    public float chestSpawnProbability = 0.1f;

    int monsterPopulation = 0;

    private void Start()
    {
        monsterPopulation = Random.Range(minSpawn, maxSpawn + 1);
        RandomSpawnMonster();
        SpawnChest();
    }

    /// <summary>
    /// 랜덤으로 몬스터를 스폰하는 함수
    /// </summary>
    private void RandomSpawnMonster()
    {
        for (int i = 0; i < monsterPopulation; i++)
        {
            int monsterType = Random.Range(0, (int)EnemyID.BULLETKIN + 1);
            GameObject obj = EnemyManager.Inst.GetEnemy((EnemyID)monsterType);
            obj.transform.position =
                (Vector2)transform.position + Random.insideUnitCircle * spawnOffset;
            obj.SetActive(true);
        }
    }

    /// <summary>
    /// 상자를 스폰하는 함수 
    /// </summary>
    private void SpawnChest()
    {
        float rand = Random.value;
        if (rand < chestSpawnProbability)
        {
            GameObject obj = Instantiate(chestPrefab, transform);
            obj.transform.position = (Vector2)transform.position + Vector2.up * 3.0f;
        }
    }
}
