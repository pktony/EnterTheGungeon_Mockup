using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletManager : Singleton<BulletManager>
{
    // #################### Dictionary #########################
    private static Dictionary<BulletType, Stack<GameObject>> pooledBullets = new();
    private Dictionary<BulletType, List<GameObject>> bulletsInScene = new();

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
            bulletsInScene[(BulletType)i] = new List<GameObject>();
        }
    }

    protected override void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        ReturnAllBullets();
        if (pooledBullets.Count < 1)
        {
            pooledBullets.Add(BulletType.PLAYER, player_Bullet);
            pooledBullets.Add(BulletType.ENEMY, enemy_Bullet);
            pooledBullets.Add(BulletType.CIRCLE, bossBullet_Circle);
            pooledBullets.Add(BulletType.BIG, bossBullet_Big);
            pooledBullets.Add(BulletType.MID, bossBullet_Mid);
            pooledBullets.Add(BulletType.FOOTBALL, bossBullet_Football);
            pooledBullets.Add(BulletType.SPINNING, bossBullet_Spinning);
            pooledBullets.Add(BulletType.GOBLET, goblet);


            for (int i = 0; i < poolingBullet.Length; i++)
            {
                for (int j = 0; j < poolingBullet[i].bulletSize; j++)
                {
                    GameObject obj = Instantiate(poolingBullet[i].prefab, this.transform);
                    pooledBullets[(BulletType)i].Push(obj);
                    obj.SetActive(false);
                }
            }
        }
    }

    public GameObject GetPooledBullet(BulletType type)
    {
        GameObject obj;
        if (pooledBullets[type].Count > 0)
        {
            obj = pooledBullets[type].Pop();
        }
        else
        {// 비어 있으면 추가로 생성
            obj = Instantiate(poolingBullet[(int)type].prefab, this.transform);
            pooledBullets[type].Push(obj);
            obj.SetActive(false);
            obj = pooledBullets[type].Pop();
        }

        bulletsInScene[type].Add(obj);    // 씬 이동 시 모든 총알 회수를 위한 리스트
        return obj;
    }

    public void ReturnBullet(BulletType type, GameObject uselessBullet)
    {
        bulletsInScene[type].Remove(uselessBullet);
        pooledBullets[type].Push(uselessBullet);
        uselessBullet.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        uselessBullet.SetActive(false);
    }

    /// <summary>
    /// 씬 이동 시 모든 총알을 수거하는 함수
    /// 또는 Blank 사용
    /// </summary>
    public void ReturnAllBullets()
    {
        for (int i = 0; i < bulletsInScene.Count; i++)
        {
            int bulletCount = bulletsInScene[(BulletType)i].Count;
            for (int j = 0; j < bulletCount; j++)
            {
                 if(bulletsInScene[(BulletType)i][j].
                    TryGetComponent<IDestroyable>(out IDestroyable bullet))
                {
                    bullet.BlankDestroy();
                }
            }
            bulletsInScene[(BulletType)i].Clear();
        }
    }
}
