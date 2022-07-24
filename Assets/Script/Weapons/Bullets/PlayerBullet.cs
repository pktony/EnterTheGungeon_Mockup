using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBullet : MonoBehaviour
{
    Rigidbody2D rigid = null;

    // ############################ Bullet Stats ############################
    [SerializeField] protected float bulletSpeed = 5.0f;
    [SerializeField] private uint bulletDamage = 1;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        rigid.isKinematic = true;
    }

    private void OnEnable()
    {
        EnableBullet();
    }

    protected virtual void EnableBullet()
    {
        rigid.velocity = GameManager.Inst.Control.FireDirection * bulletSpeed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        BulletManager.Bullet_Inst.ReturnPlayerBullet(this.gameObject);
    }
}
