using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using TMPro.EditorUtilities;

public class BulletKin : Enemy
{
    protected override void Awake()
    {
        base.Awake();

        firePosition = new Transform[1];
        firePosition[0] = weapon.transform.GetChild(0).GetChild(0);
    }

    protected override void Shoot()
    {
        base.Shoot();
        GameManager.Inst.SoundManager.PlaySound_Weapon(Clips_Weapon.Colt_Shot1, source);
    }

    protected override void PlayHitSound()
    {
        GameManager.Inst.SoundManager.PlaySound_BulletKin(Clips_BulletKin.BulletKin_Hurt1, source);
    }

    protected override void DieAnimation()
    {
        base.DieAnimation();
        GameManager.Inst.SoundManager.PlaySound_BulletKin(Clips_BulletKin.BulletKin_Hurt1, source);
    }

    // ----------------- Die
    protected override void ReturnToPool()
    {
        EnemyManager.Inst.ReturnEnemy(EnemyID.BULLETKIN, this.gameObject);
    }
}
