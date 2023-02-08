using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet_Spinning : EnemyBullet
{
    protected override void Awake()
    {
        base.Awake();

        id = BulletType.SPINNING;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {// Layer Setting : Enemy - EnemyBullet
        BulletManager.Inst.ReturnBullet(BulletType.SPINNING, this.gameObject);
    }
}