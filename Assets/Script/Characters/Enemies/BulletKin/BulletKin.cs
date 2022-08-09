using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class BulletKin : ShotgunKin, IHealth
{
    protected override void Shoot()
    {
         GameObject bullet = BulletManager.Inst.GetPooledBullet(BulletManager.PooledBullets[BulletManager.Inst.EnemyBulletID]);
         bullet.transform.position = weapon.transform.position;
         bullet.transform.rotation = firePosition[0].rotation;
         bullet.SetActive(true);
    }

    // ----------------- Die
    protected override IEnumerator Die()
    {
        anim.SetTrigger("onDie");
        float randDie = UnityEngine.Random.value;
        anim.SetFloat("RandDie", randDie);
        weaponSprite.color = Color.clear;
        yield return new WaitForSeconds(3.0f);
        EnemyManager.Inst.ReturnEnemy(EnemyManager.Inst.PooledEnemy[(int)EnemyID.BULLETKIN], this.gameObject);
    }
}
