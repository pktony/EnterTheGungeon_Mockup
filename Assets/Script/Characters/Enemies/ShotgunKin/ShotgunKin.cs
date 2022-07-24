using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class ShotgunKin : MonoBehaviour, IHealth, IBattle
{
    Animator anim = null;
    SpriteRenderer weaponSprite = null;
    ShotgunKin_Weapon weapon = null;
    public Transform[] firePosition = null;


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
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float attackInterval = 1.0f;
    private float attackTimer = 0.0f;

    [Header("Bullets")]
    [SerializeField] private uint bulletNumber = 5;
    [SerializeField] private float fireAngle = 30.0f;

    // -------------- 

    // ############################### PROPERTIES #########################
    public int HP { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public int MaxHP => throw new System.NotImplementedException();

    public Vector2 TrackDirection { get => trackDirection; }

    // ############################### IBattle ###########################
    public void Attack(IBattle target)
    {
        throw new NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        throw new NotImplementedException();
    }

    // ############################### IHealth ###########################
    public Action onTakeDamage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Action onHPUp { get; set; } //intentionally Blank 

    private void Awake()
    {
        anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<ShotgunKin_Weapon>();
        weaponSprite = weapon.GetComponentInChildren<SpriteRenderer>();

        firePosition = new Transform[bulletNumber];

        for (int i = 0; i < bulletNumber; i++)
        {
            firePosition[i] = weapon.transform.GetChild(0).GetChild(i);
            firePosition[i].rotation = Quaternion.Euler(0, 0, (fireAngle * 0.5f) - ((fireAngle / (bulletNumber - 1)) * i));
        }
    }

    void FixedUpdate()
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
                TrackUpdate();
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


    // --------------------- Status 
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
        if(!Search())
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
            attackTimer += Time.fixedDeltaTime;
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
            detectTimer += Time.fixedDeltaTime;
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
            GameObject bullet = BulletManager.Bullet_Inst.GetEnemyBullet();
            bullet.transform.position = weapon.transform.position;
            bullet.transform.rotation = firePosition[i].rotation;
        }
    }

    bool InAttackRange() { return
            (transform.position - target.transform.position).sqrMagnitude < attackRange * attackRange; }

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
            case EnemyState.DEAD:
                break;
            case EnemyState.ATTACK:
                attackTimer = attackInterval;
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
