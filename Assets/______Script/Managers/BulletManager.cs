using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletManager : Singleton<BulletManager>
{
    // #################### Dictionary #########################
    private static Dictionary<uint, Stack<GameObject>> pooledBullets = new();

    //##################### Bullet Stack #######################
    [SerializeField] private BulletData[] poolingBullet;
        // [0] : player Bullet
        // [1] : Enemy Bullet

    private Stack<GameObject> player_Bullet = new();
    private Stack<GameObject> enemy_Bullet = new();
    private Stack<GameObject> bossBullet_Circle = new();
    private Stack<GameObject> bossBullet_Big = new();
    private Stack<GameObject> bossBullet_Mid = new();
    private Stack<GameObject> bossBullet_Football = new();
    private Stack<GameObject> bossBullet_Spinning = new();
    private Stack<GameObject> goblet = new();


    protected override void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        if (pooledBullets.Count < 1)
        {
            pooledBullets.Add(poolingBullet[(int)BulletID.PLAYER].bulletId, player_Bullet);
            pooledBullets.Add(poolingBullet[(int)BulletID.ENEMY].bulletId, enemy_Bullet);
            pooledBullets.Add(poolingBullet[(int)BulletID.CIRCLE].bulletId, bossBullet_Circle);
            pooledBullets.Add(poolingBullet[(int)BulletID.BIG].bulletId, bossBullet_Big);
            pooledBullets.Add(poolingBullet[(int)BulletID.MID].bulletId, bossBullet_Mid);
            pooledBullets.Add(poolingBullet[(int)BulletID.FOOTBALL].bulletId, bossBullet_Football);
            pooledBullets.Add(poolingBullet[(int)BulletID.SPINNING].bulletId, bossBullet_Spinning);
            pooledBullets.Add(poolingBullet[(int)BulletID.GOBLET].bulletId, goblet);


            for (int i = 0; i < poolingBullet.Length; i++)
            {
                for (int j = 0; j < poolingBullet[i].bulletSize; j++)
                {
                    GameObject obj = Instantiate(poolingBullet[i].prefab, this.transform);
                    pooledBullets[(uint)i].Push(obj);
                    obj.SetActive(false);
                }
            }
        }
    }

    public GameObject GetPooledBullet(BulletID id)
    {
        if (pooledBullets[(uint)id].Count > 0)
        {
            return pooledBullets[(uint)id].Pop();
        }
        else
        {
            GameObject obj = Instantiate(poolingBullet[(int)id].prefab, this.transform);
            pooledBullets[(uint)id].Push(obj);
            obj.SetActive(false);
            return obj;
        }
    }

    public void ReturnBullet(BulletID id, GameObject uselessBullet)
    {
        pooledBullets[(uint)id].Push(uselessBullet);
        uselessBullet.transform.rotation = Quaternion.identity;
        uselessBullet.transform.position = Vector3.zero;
        uselessBullet.SetActive(false);
    }
}
