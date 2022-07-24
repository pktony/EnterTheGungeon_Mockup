using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour, IBattle
{
    IBattle player;
    Rigidbody2D rigid = null;

    // ############################ Bullet Stats ############################
    [SerializeField] private float bulletSpeed = 5.0f;
    [SerializeField] private uint bulletDamage = 1;

    void Awake()
    {
        player = FindObjectOfType<Player>().GetComponent<IBattle>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        EnableBullet();
    }

    public void Attack(IBattle target)
    {
        target.TakeDamage(1);
    }

    public void TakeDamage(int _) {} //intentionally Blank

    void EnableBullet()
    {
        rigid.velocity = bulletSpeed * transform.right;
    }

      void OnTriggerEnter2D(Collider2D collision)
    {
        BulletManager.Bullet_Inst.ReturnEnemyBullet(this.gameObject);
        if (collision.CompareTag("Player"))
        {
            Attack(player);
        }
    }
}
