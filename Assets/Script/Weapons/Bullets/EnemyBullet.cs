using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody2D rigid = null;

    // ############################ Bullet Stats ############################
    [SerializeField] private float bulletSpeed = 5.0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;
    }

    private void OnEnable()
    {
        EnableBullet();
    }

    void EnableBullet()
    {
        rigid.velocity = bulletSpeed * transform.right;
    }

    //public void InstantDestroy()
    //{
    //    BulletManager.Inst.ReturnBullet(BulletManager.PooledBullets[(int)BulletID.ENEMY], this.gameObject);
    //}

    void OnTriggerEnter2D(Collider2D collision)
    {// Layer Setting : Enemy - EnemyBullet
        BulletManager.Inst.ReturnBullet(BulletID.ENEMY, this.gameObject);
    }
}