using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public Transform FirePosition = null;

    public void Fire()
    {
        GameObject bullet = BulletManager.Inst.GetPooledBullet();
        bullet.transform.position = FirePosition.position;
    }
}
