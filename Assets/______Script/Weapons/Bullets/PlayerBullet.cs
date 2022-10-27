using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBullet : MonoBehaviour
{
    Rigidbody2D rigid = null;

    // ############################ Bullet Stats ############################
    [SerializeField] private float bulletSpeed = 5.0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        rigid.isKinematic = true;
    }

    private void OnEnable()
    {
        EnableBullet();
    }

    void EnableBullet()
    {
        rigid.velocity = bulletSpeed * transform.right; // GameManager.Inst.Control.FireDirection;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        BulletManager.Inst.ReturnBullet(BulletType.PLAYER, this.gameObject);
    }
}