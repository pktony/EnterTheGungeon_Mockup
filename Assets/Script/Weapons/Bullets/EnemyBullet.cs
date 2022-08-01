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
    }

    private void OnEnable()
    {
        EnableBullet();
    }

    public void TakeDamage(int _) {} //intentionally Blank

    void EnableBullet()
    {
        rigid.velocity = bulletSpeed * transform.right;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        BulletManager.Inst.ReturnBullet(BulletManager.PooledBullets[BulletManager.Inst.EnemyBulletID], this.gameObject);
    }
}