using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet_Big : EnemyBullet
{
    public int splitCount = 8;
    [Range(0f, 0.8f)]
    public float splitTime = 0.5f;

    protected override void Awake()
    {
        base.Awake();

        id = BulletID.BIG;
    }

    protected override void EnableBullet()
    {
        base.EnableBullet();
        StartCoroutine(SplitBullets());
    }

    protected virtual IEnumerator SplitBullets()
    {
        yield return new WaitForSeconds(splitTime);
        float theta = 0f;
        for (int i = 0; i < splitCount; i++)
        {
            GameObject obj = BulletManager.Inst.GetPooledBullet(BulletID.MID);
            obj.transform.position = this.transform.position;
            obj.transform.rotation = Quaternion.Euler(0f, 0f, theta);
            obj.SetActive(true);
            theta += 360 / splitCount;
        }
        BulletManager.Inst.ReturnBullet(BulletID.BIG, this.gameObject);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
    }
}