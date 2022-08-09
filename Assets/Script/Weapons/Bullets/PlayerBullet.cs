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
        //GameObject obj = FXManager.Inst.GetFX(FXManager.Inst.PooledFx[(int)FxID.BULLETEXPLOSION]);
        //Vector2 contact = collision.ClosestPoint(transform.position);
        //obj.transform.right = GetExplosionAngle(contact);

        //collision.GetContacts();
        //collision.gameObject.GetComponent<collider>
        BulletManager.Inst.ReturnBullet(BulletManager.PooledBullets[BulletManager.Inst.PlayerBulletID], this.gameObject);
    }

    private Vector2 GetExplosionAngle(Vector2 contact)
    {// bulletexplosion FX는 원래 오른쪽을 바라보고 있다.
        Vector2 ballVector = rigid.velocity;

        return Vector2.zero;
    }
}