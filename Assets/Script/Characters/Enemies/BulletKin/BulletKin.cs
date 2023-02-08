using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletKin : Enemy
{
    SoundManager soundManager;

    protected override void Awake()
    {
        base.Awake();

        firePosition = new Transform[1];
        firePosition[0] = weapon.transform.GetChild(0).GetChild(0);
    }

    private void Start()
    {
        soundManager = SoundManager.Inst;
    }

    protected override void Shoot()
    {
        base.Shoot();
        soundManager.PlaySound_Weapon(Clips_Weapon.Colt_Shot1, source);
    }

    protected override void PlayHitSound()
    {
        soundManager.PlaySound_BulletKin(Clips_BulletKin.BulletKin_Hurt1, source);
    }

    protected override void DieAnimation()
    {
        base.DieAnimation();
        soundManager.PlaySound_BulletKin(Clips_BulletKin.BulletKin_Hurt1, source);
    }

    // ----------------- Die
    protected override void ReturnToPool()
    {
        EnemyManager.Inst.ReturnEnemy(EnemyID.BULLETKIN, this.gameObject);
    }
}
