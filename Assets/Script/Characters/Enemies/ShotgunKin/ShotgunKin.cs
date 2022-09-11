using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class ShotgunKin : Enemy, IHealth
{
    [Header("Bullets")]
    [SerializeField] private uint bulletPerFire = 5;
    [SerializeField] private float fireAngle = 30.0f;
    protected override void Awake()
    {
        base.Awake();

        firePosition = new Transform[bulletPerFire];

        for (int i = 0; i < bulletPerFire; i++)
        {
            firePosition[i] = weapon.transform.GetChild(0).GetChild(i);
            uint bulletNum = bulletPerFire;
            if (bulletPerFire - 1 < 1)
            {
                bulletNum = 2;
            }
            firePosition[i].rotation = Quaternion.Euler(0, 0, (fireAngle * 0.5f) - ((fireAngle / (bulletNum)) * i));
        }
    }

    protected override void Shoot()
    {
        for (int i = 0; i < bulletPerFire; i++)
        {
            GameObject bullet = BulletManager.Inst.GetPooledBullet(BulletManager.PooledBullets[BulletManager.Inst.EnemyBulletID]);
            bullet.transform.position = weapon.transform.position;
            bullet.transform.rotation = firePosition[i].rotation;
            bullet.SetActive(true);
        }
    }
}
