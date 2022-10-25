using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletManager : Singleton<BulletManager>
{
    // #################### Dictionary #########################
    private static Dictionary<BulletID, Stack<GameObject>> pooledBullets = new();
    private Dictionary<BulletID, List<GameObject>> bulletsInScene = new();

    //##################### Bullet Stack #######################
    [SerializeField] private BulletData[] poolingBullet;

    private Stack<GameObject> player_Bullet = new();
    private Stack<GameObject> enemy_Bullet = new();
    private Stack<GameObject> bossBullet_Circle = new();
    private Stack<GameObject> bossBullet_Big = new();
    private Stack<GameObject> bossBullet_Mid = new();
    private Stack<GameObject> bossBullet_Football = new();
    private Stack<GameObject> bossBullet_Spinning = new();
    private Stack<GameObject> goblet = new();

    protected override void Awake()
    {
        base.Awake();
        // 모든 Bullet 회수용 딕셔너리/리스트 초기화 
        for(int i = 0; i < poolingBullet.Length; i++)
        {
            bulletsInScene[(BulletID)i] = new List<GameObject>();
        }
    }


    protected override void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        ReturnAllBullets();
        if (pooledBullets.Count < 1)
        {
            pooledBullets.Add(BulletID.PLAYER, player_Bullet);
            pooledBullets.Add(BulletID.ENEMY, enemy_Bullet);
            pooledBullets.Add(BulletID.CIRCLE, bossBullet_Circle);
            pooledBullets.Add(BulletID.BIG, bossBullet_Big);
            pooledBullets.Add(BulletID.MID, bossBullet_Mid);
            pooledBullets.Add(BulletID.FOOTBALL, bossBullet_Football);
            pooledBullets.Add(BulletID.SPINNING, bossBullet_Spinning);
            pooledBullets.Add(BulletID.GOBLET, goblet);


            for (int i = 0; i < poolingBullet.Length; i++)
            {
                for (int j = 0; j < poolingBullet[i].bulletSize; j++)
                {
                    GameObject obj = Instantiate(poolingBullet[i].prefab, this.transform);
                    pooledBullets[(BulletID)i].Push(obj);
                    obj.SetActive(false);
                }
            }
        }
    }

    public GameObject GetPooledBullet(BulletID id)
    {
        GameObject obj;
        if (pooledBullets[id].Count > 0)
        {
            obj = pooledBullets[id].Pop();
        }
        else
        {
            obj = Instantiate(poolingBullet[(int)id].prefab, this.transform);
            pooledBullets[id].Push(obj);
            obj.SetActive(false);
        }

        bulletsInScene[id].Add(obj);
        return obj;
    }

    public void ReturnBullet(BulletID id, GameObject uselessBullet)
    {
        bulletsInScene[id].Remove(uselessBullet);
        pooledBullets[id].Push(uselessBullet);
        uselessBullet.transform.rotation = Quaternion.identity;
        uselessBullet.transform.position = Vector3.zero;
        uselessBullet.SetActive(false);
    }

    /// <summary>
    /// 씬 이동 시 모든 총알을 수거하는 함수 
    /// </summary>
    private void ReturnAllBullets()
    {
        for (int i = 0; i < bulletsInScene.Count; i++)
        {
            int bulletCount = bulletsInScene[(BulletID)i].Count;
            for (int j = 0; j < bulletCount; j++)
            {
                Debug.Log($"{(BulletID)i}, {j}");
                //ReturnBullet((BulletID)i, bulletsInScene[(BulletID)i][j]);
            }
        }
    }
}
