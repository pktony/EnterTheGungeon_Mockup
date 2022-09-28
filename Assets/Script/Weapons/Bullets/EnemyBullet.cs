using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour, IDestroyable
{
    #region PROTECTED VAIRABLES
    protected BulletID id;
    #endregion

    #region PRIVATE
    private Rigidbody2D rigid = null;
    private SpriteRenderer spriteRend;
    private WaitForSeconds blankWaitSeconds;
    private GameObject BlankExplosionFX;
    private Collider2D coll;
    #endregion


    // ############################ Bullet Stats ############################
    [SerializeField] private float bulletSpeed = 5.0f;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();

        rigid.velocity = Vector2.zero;
        blankWaitSeconds = new WaitForSeconds(IDestroyable.blankExplosionTime);
        BlankExplosionFX = transform.GetChild(0).gameObject;

        id = BulletID.ENEMY;
    }

    private void OnEnable()
    {
        EnableBullet();
    }

    #region PROTECTED METHODS
    protected virtual void EnableBullet()
    {
        rigid.velocity = bulletSpeed * transform.right;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {// Layer Setting : Enemy - EnemyBullet
        StartCoroutine(PlayBlankDestroyAnimation());
    }
    #endregion

    #region Idestroyable
    public void BlankDestroy()
    {
        StartCoroutine(PlayBlankDestroyAnimation());
    }

    IEnumerator PlayBlankDestroyAnimation()
    {
        rigid.velocity = Vector2.zero;

        spriteRend.color = Color.clear;
        BlankExplosionFX.SetActive(true);
        coll.enabled = false;

        yield return blankWaitSeconds;

        spriteRend.color = Color.white;
        BlankExplosionFX.SetActive(false);
        coll.enabled = true;
        BulletManager.Inst.ReturnBullet(id, this.gameObject);
    }
    #endregion
}