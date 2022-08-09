using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Enemy : MonoBehaviour, IHealth
{
    protected Animator anim = null;
    protected SpriteRenderer weaponSprite = null;
    protected EnemyWeapon weapon = null;
    protected Transform firePosition = null;


    //############################## VARIABLES #############################
    public EnemyState status = EnemyState.IDLE;

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

    [Header("Bullets")]
    [SerializeField] private uint bulletNumber = 5;
    [SerializeField] private float fireAngle = 30.0f;

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
    public Action onTakeDamage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<EnemyWeapon>();
        weaponSprite = weapon.GetComponentInChildren<SpriteRenderer>();
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
                PatrolUpdate();
                break;
            case EnemyState.TRACK:
                // FixedUpdate
                break;
            case EnemyState.DEAD:
                break;
            case EnemyState.ATTACK:
                AttackUpdate();
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
        if (Search())
        {
            status = EnemyState.TRACK;
            return;
        }
    }

    void PatrolUpdate()
    {

    }

    void TrackUpdate()
    {
        if (!Search())
        {
            status = EnemyState.IDLE;
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

    void Shoot()
    {
        for (int i = 0; i < bulletNumber; i++)
        {
            GameObject bullet = BulletManager.Inst.GetPooledBullet(BulletManager.PooledBullets[BulletManager.Inst.EnemyBulletID]);
            bullet.transform.position = weapon.transform.position;
            bullet.transform.rotation = firePosition.rotation;
            bullet.SetActive(true);
        }
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
            HP -= 1;
            anim.SetTrigger("onHit");
        }
    }

    // ----------------- Die
    private IEnumerator Die()
    {
        anim.SetTrigger("onDie");
        float randDie = UnityEngine.Random.value;
        anim.SetFloat("RandDie", randDie);
        weaponSprite.color = Color.clear;
        yield return new WaitForSeconds(3.0f);
        EnemyManager.Inst.ReturnEnemy(EnemyManager.Inst.PooledEnemy[(int)EnemyID.SHOTGUNKIN], this.gameObject);
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
            case EnemyState.DEAD:
                break;
            case EnemyState.ATTACK:
                break;
            default:
                break;
        }

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
                StartCoroutine(Die());
                break;
            default:
                break;
        }

        status = newState;
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
