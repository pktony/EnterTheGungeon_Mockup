using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, IHealth
{
    protected Animator anim = null;
    protected SpriteRenderer weaponSprite = null;
    protected EnemyWeapon weapon = null;
    protected Transform[] firePosition = null;
    Rigidbody2D rigid = null;


    //############################## VARIABLES #############################
    public EnemyState status = EnemyState.IDLE;
    private bool isDead = false;


    // -------------- Track
    [Header("Ranges")]
    [SerializeField] private float detectRadius = 5.0f;
    [SerializeField] private float attackRange = 3.0f;
    private float detectCoolTime = 2.0f;
    private float detectTimer = 0.0f;
    
    private GameObject target = null;
    private Vector2 trackDirection = Vector2.zero;


    // -------------- Stats
    [Header("Basic Stats")]
    [SerializeField] private int healthPoint = 3;
    [SerializeField] private int maxHealthPoint = 3;
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float attackInterval = 1.0f;
    private float attackTimer = 0.0f;
    // -------------- 

    // ############################### PROPERTIES #########################
    public int HP
    {
        get => healthPoint;
        set
        {
            healthPoint = value;
            if (healthPoint < 1)
            {
                ChangeStatus(EnemyState.DEAD);
            }
        }
    }

    public int MaxHP => maxHealthPoint;

    public Vector2 TrackDirection { get => trackDirection; }

    // ############################### IHealth ###########################
    public Action OnTakeDamage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Action OnHPUp { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<EnemyWeapon>();
        weaponSprite = weapon.GetComponentInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        weaponSprite.color = Color.white;
    }

    void FixedUpdate()
    {
        if (status == EnemyState.TRACK)
        {
            TrackUpdate();
        }
    }

    private void Update()
    {
        CheckStatus();
    }

    //############################## METHODS ################################
    // ------- Status Check -------------
    void CheckStatus()
    {
        switch (status)
        {
            case EnemyState.IDLE:
                IdleUpdate();
                break;
            case EnemyState.PATROL:
                break;
            case EnemyState.TRACK:
                // FixedUpdate
                break;
            case EnemyState.ATTACK:
                AttackUpdate();
                break;
            case EnemyState.DEAD:
                break;
            default:
                break;
        }
    }

    // --------------------- Updates
    bool Search()
    {
        bool result = false;
        Collider2D coll = Physics2D.OverlapCircle(transform.position, detectRadius, LayerMask.GetMask("Player"));

        if (coll != null)
        {
            target = coll.gameObject;
            result = true;
        }
        return result;
    }

    void IdleUpdate()
    {
        if (!isDead && Search())
        {
            status = EnemyState.TRACK;
            return;
        }
    }

    void TrackUpdate()
    {
        if (!isDead && !Search())
        {
            ChangeStatus(EnemyState.IDLE);
            return;
        }
        else
        {
            transform.position = Vector2.MoveTowards
            (transform.position, target.transform.position, moveSpeed * Time.fixedDeltaTime);

            trackDirection = target.transform.position - transform.position;
            anim.SetFloat("trackDirection_X", trackDirection.x);
            anim.SetFloat("trackDirection_Y", trackDirection.y);

            if (InAttackRange())
            {
                ChangeStatus(EnemyState.ATTACK);
                return;
            }
        }
    }

    void AttackUpdate()
    {
        if (InAttackRange())
        {
            attackTimer += Time.deltaTime;
            trackDirection = target.transform.position - transform.position;
            RotateWeapon();

            if (attackTimer > attackInterval)
            {
                anim.SetTrigger("onAttack");
                Shoot();
                attackTimer = 0f;
            }
        }
        else
        {
            detectTimer += Time.deltaTime;
            if (detectTimer > detectCoolTime)
            {
                ChangeStatus(EnemyState.TRACK);
                detectTimer = 0f;
                return;
            }
            return;
        }
    }

    protected virtual void Shoot()
    {
         GameObject bullet = BulletManager.Inst.GetPooledBullet(BulletID.ENEMY);
         bullet.transform.position = weapon.transform.position;
         bullet.transform.rotation = firePosition[0].rotation;
         bullet.SetActive(true);
    }

    bool InAttackRange()
    {
        return
            (transform.position - target.transform.position).sqrMagnitude < attackRange * attackRange;
    }

    void RotateWeapon()
    {
        weapon.transform.right = trackDirection;

        if (trackDirection.x < 0)
        {
            weaponSprite.flipY = true;
        }
        else
        {
            weaponSprite.flipY = false;
        }
    }

    // ---------------- Hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullets"))
        {
            if (status != EnemyState.DEAD)
            {
                HP -= 1;
                anim.SetTrigger("onHit");
            }
        }
    }

    // ----------------- Die
    protected virtual IEnumerator Die()
    {
        rigid.AddForce(-trackDirection, ForceMode2D.Impulse);

        anim.SetTrigger("onDie");
        float randDie = UnityEngine.Random.value;
        anim.SetFloat("RandDie", randDie);
        weaponSprite.color = Color.clear;
        yield return new WaitForSeconds(3.0f);

        uint rand = (uint)UnityEngine.Random.Range(4, 6); // 4: Gold, 5 : silver, 6 : Bronze
        GameObject shell = ItemManager.Inst.GetPooledItem((ItemID)rand);
        shell.transform.position = this.transform.position;
        shell.gameObject.SetActive(true);
        EnemyManager.Inst.ReturnEnemy(EnemyID.SHOTGUNKIN, this.gameObject);
    }


    // ---------------- Status Change
    void ChangeStatus(EnemyState newState)
    {
        // On Status Exit
        switch (status)
        {
            case EnemyState.IDLE:
                break;
            case EnemyState.PATROL:
                break;
            case EnemyState.TRACK:
                break;
            case EnemyState.ATTACK:
                break;
            case EnemyState.DEAD:
                break;
            default:
                break;
        }

        status = newState;

        // On Status Enter
        switch (status)
        {
            case EnemyState.IDLE:
                break;
            case EnemyState.PATROL:
                break;
            case EnemyState.TRACK:
                break;
            case EnemyState.ATTACK:
                attackTimer = attackInterval - 0.5f;
                break;
            case EnemyState.DEAD:
                isDead = true;
                StartCoroutine(Die());
                break;
            default:
                break;
        }

        anim.SetInteger("Status", (int)newState);
    }

    // ########################## Gizmos ###########################
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        if (status == EnemyState.TRACK || status == EnemyState.ATTACK)
        {
            Handles.color = Color.red;
        }

        Handles.DrawWireDisc(transform.position, transform.forward, detectRadius);

        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, attackRange);
    }
}
