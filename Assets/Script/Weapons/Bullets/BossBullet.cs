using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : EnemyBullet
{
    protected override void Awake()
    {
        base.Awake();
        id = BulletType.CIRCLE;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {// Layer Setting : Enemy - EnemyBullet
        BulletManager.Inst.ReturnBullet(BulletType.CIRCLE, this.gameObject);
    }
}