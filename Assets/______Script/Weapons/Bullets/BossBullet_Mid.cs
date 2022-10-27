using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet_Mid : BossBullet_Big
{
    protected override void Awake()
    {
        base.Awake();

        id = BulletType.MID;
    }

    protected override void EnableBullet()
    {
        base.EnableBullet();
        StartCoroutine(SplitBullets());
    }

    protected override IEnumerator SplitBullets()
    {
        yield return new WaitForSeconds(splitTime);
        float theta = 0f;
        for (int i = 0; i < splitCount; i++)
        {
            GameObject obj = BulletManager.Inst.GetPooledBullet(BulletType.CIRCLE);
            obj.transform.position = this.transform.position;
            obj.transform.rotation = Quaternion.Euler(0f, 0f, theta);
            obj.SetActive(true);
            theta += 360 / splitCount;
        }
        BulletManager.Inst.ReturnBullet(BulletType.MID, this.gameObject);
    }
}