using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class ShotgunKin : Enemy
{
    SoundManager soundManager;

    [Header("Bullets")]
    [SerializeField] private uint bulletPerFire = 5;
    [SerializeField] private float fireAngle = 30.0f;

    protected override void Awake()
    {
        base.Awake();
        InitializeShootPositions();
    }

    private void Start()
    {
        soundManager = SoundManager.Inst;
    }

    /// <summary>
    /// 일정한 각도로 발사 위치를 초기화 하는 함수
    /// </summary>
    private void InitializeShootPositions()
    {
        firePosition = new Transform[bulletPerFire];

        uint bulletNum = bulletPerFire;
        if (bulletPerFire - 1 < 1)
        {
            bulletNum = 2;
        }
        for (int i = 0; i < bulletPerFire; i++)
        {
            firePosition[i] = weapon.transform.GetChild(0).GetChild(i);   
            firePosition[i].rotation = Quaternion.Euler(0, 0, (fireAngle * 0.5f) - ((fireAngle / (bulletNum)) * i));
        }
    }

    protected override void Shoot()
    {
        for (int i = 0; i < bulletPerFire; i++)
        {
            GameObject bullet = BulletManager.Inst.GetPooledBullet(BulletType.ENEMY);
            bullet.transform.SetPositionAndRotation(
                weapon.transform.position, firePosition[i].rotation);
            bullet.SetActive(true);
        }

        soundManager.PlaySound_Weapon(Clips_Weapon.Shotgun_Shot1, source);
    }

    protected override void PlayHitSound()
    {
        soundManager.PlaySound_ShotgunKin(Clips_ShotgunKin.Shotgun_Hurt1, source);
    }

    protected override void DieAnimation()
    {
        base.DieAnimation();
        soundManager.PlaySound_ShotgunKin(Clips_ShotgunKin.Shotgun_Death1, source);
    }

    // ----------------- Die
    protected override void ReturnToPool()
    {
        EnemyManager.Inst.ReturnEnemy(EnemyID.SHOTGUNKIN, this.gameObject);
    }
}
