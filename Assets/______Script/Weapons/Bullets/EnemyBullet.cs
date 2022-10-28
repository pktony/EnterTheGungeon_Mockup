using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 총알의 부모 클래스 
/// </summary>
public class EnemyBullet : MonoBehaviour, IDestroyable
{
    #region 컴포넌트 ############################################################
    private Rigidbody2D rigid = null;
    private SpriteRenderer spriteRend;
    private WaitForSeconds blankWaitSeconds;
    private GameObject BlankExplosionFX;
    private Collider2D coll;
    #endregion
    #region 변수 ################################################################
    protected BulletType id;
    [SerializeField] private float bulletSpeed = 5.0f;
    #endregion

    #region UNITY EVENT 함수 ####################################################
    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();

        rigid.velocity = Vector2.zero;
        blankWaitSeconds = new WaitForSeconds(IDestroyable.blankExplosionTime);
        BlankExplosionFX = transform.GetChild(0).gameObject;

        id = BulletType.ENEMY;
    }

    private void OnEnable()
    {
        EnableBullet();
    }
    #endregion

    #region PROTECTED 함수 ######################################################
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