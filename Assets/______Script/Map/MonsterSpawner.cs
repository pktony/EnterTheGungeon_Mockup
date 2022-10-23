using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterSpawner : MonoBehaviour
{
    [Range(2f, 5f)]
    public float spawnOffset;

    int monsterPopulation = 0;

    private void Start()
    {
        monsterPopulation = Random.Range(2, 4);

        for (int i = 0; i < monsterPopulation; i++)
        {
            int monsterType = Random.Range(0, (int)EnemyID.BULLETKIN + 1);
            GameObject obj = EnemyManager.Inst.GetEnemy((EnemyID)monsterType);
            obj.transform.position = (Vector2)transform.position + Random.insideUnitCircle * spawnOffset;
            obj.SetActive(true);
        }
    }
}
