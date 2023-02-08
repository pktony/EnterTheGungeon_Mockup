using System.Collections;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// BulletKin, ShotgunKin 부모클래스 
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, IHealth
{
    #region 컴포넌트 ############################################################
    protected Animator anim = null;
    protected SpriteRenderer weaponSprite = null;
    protected AudioSource source;
    protected Transform[] firePosition = null;
    private Rigidbody2D rigid = null;
    #endregion

    #region 변수 ###############################################################
    public EnemyState status = EnemyState.IDLE;
    private bool isDead = false;
    protected EnemyWeapon weapon = null;
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
    private Collider2D[] searchColls = new Collider2D[2];
    #endregion

    #region 프로퍼티 #############################################################
    public int HP
    {
        get => healthPoint;
        set
        {
            healthPoint = value;
            if (healthPoint < 1 && !isDead)
            {// 1 미만이면 죽는다 
                ChangeStatus(EnemyState.DEAD);
            }
        }
    }

    public int MaxHP => maxHealthPoint;
    public Vector2 TrackDirection { get => trackDirection; }
    #endregion

    #region IHEALTH ############################################################
    public Action<int, int> OnTakeDamage { get; set; }  // <hp, _>
    public Action<int> OnHPUp { get; set; }             // <hp>
    #endregion

    #region UNITY EVENT 함수 ####################################################
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        weapon = GetComponentInChildren<EnemyWeapon>();
        weaponSprite = weapon.GetComponentInChildren<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        weaponSprite.color = Color.white;
    }

    private void FixedUpdate()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullets"))
        {
            if (status != EnemyState.DEAD)
            {
                HP -= 1;
                anim.SetTrigger("onHit");
                PlayHitSound();
            }
        }
    }
    #endregion

    #region PUBLIC 함수 #########################################################
    #endregion

    #region PRIVATE 함수 ########################################################
    /// <summary>
    /// 현재 상태 확인 함수 
    /// </summary>
    private void CheckStatus()
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

    /// <summary>
    /// 플레이어 탐지 함수
    /// 탐지 거리 안에 있으면 타겟 설정
    /// </summary>
    /// <returns>True : 탐지성공 , False : 탐지 실패 </returns>
    private bool Search()
    {
        bool result = false;
        if(Physics2D.OverlapCircleNonAlloc(transform.position,
            detectRadius, searchColls, LayerMask.GetMask("Player")) > 0)
        {
            target = searchColls[0].gameObject;
            Array.Clear(searchColls, 0, searchColls.Length);
            result = true;
        }
        return result;
    }

    /// <summary>
    /// 탐지 성공 시 Track 상태로 변경  
    /// </summary>
    private void IdleUpdate()
    {
        if (!isDead && Search())
        {
            status = EnemyState.TRACK;
            return;
        }
    }

    /// <summary>
    /// 플레이어 방향으로 이동 
    /// 탐지 실패 시 Idle 상태로 전환
    /// </summary>
    private void TrackUpdate()
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

            if (IsInAttackRange())
            {
                ChangeStatus(EnemyState.ATTACK);
                return;
            }
        }
    }

    /// <summary>
    /// 공격 사거리 안에 있으면 공격 실행 
    /// </summary>
    private void AttackUpdate()
    {
        if (IsInAttackRange())
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

    /// <summary>
    /// 공격 사거리 안에 있는가 ?
    /// </summary>
    /// <returns> True : 공격 사거리안에 있음, False : 공격 사거리 밖에 있음 </returns>
    private bool IsInAttackRange()
    {
        return (transform.position - target.transform.position).sqrMagnitude
            < attackRange * attackRange;
    }

    /// <summary>
    /// 무기와 Sprite를 회전에 맞춰 조절하는 함수 
    /// </summary>
    private void RotateWeapon()
    {
        weapon.transform.right = trackDirection;
        weaponSprite.flipY = trackDirection.x < 0;
    }

    /// <summary>
    /// 죽을 때 실행할 함수 
    /// </summary>
    /// <returns></returns>
    private IEnumerator DieProcess()
    {
        DieAnimation();
        yield return new WaitForSeconds(3.0f);
        DropItems();
        ReturnToPool();
    }

    /// <summary>
    /// 상태를 변경할 때 쓰는 함수 
    /// </summary>
    /// <param name="newState"></param>
    private void ChangeStatus(EnemyState newState)
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
                StartCoroutine(DieProcess());
                break;
            default:
                break;
        }

        anim.SetInteger("Status", (int)newState);
    }
    #endregion

    #region PROTECTED 함수 ######################################################
    protected virtual void Shoot()
    {
        GameObject bullet = BulletManager.Inst.GetPooledBullet(BulletType.ENEMY);
        bullet.transform.position = weapon.transform.position;
        bullet.transform.rotation = firePosition[0].rotation;
        bullet.SetActive(true);
    }

    protected virtual void PlayHitSound() { }

    protected virtual void DieAnimation()
    {
        // 넉백
        rigid.AddForce(-trackDirection.normalized, ForceMode2D.Impulse);

        anim.SetTrigger("onDie");
        float randDie = UnityEngine.Random.value;
        anim.SetFloat("RandDie", randDie);
        weaponSprite.color = Color.clear;
    }

    /// <summary>
    /// 랜덤으로 아이템 드랍하는 함수 
    /// </summary>
    protected virtual void DropItems()
    {
        int itemCount = UnityEngine.Random.Range(1, 4);
        for (int i = 0; i < itemCount; i++)
        {
            // 2: Key, 3: Heart, 4: Gold, 5 : silver, 6 : Bronze
            int rand = UnityEngine.Random.Range(2, 6);
            if (rand == 3)  //Heart는 드랍하지 않는다 
                continue;
            GameObject shell = ItemManager.Inst.GetPooledItem((ItemType)rand);
            shell.transform.position = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle;
            shell.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 오브젝트 풀로 반환하는 함수
    /// </summary>
    protected virtual void ReturnToPool() { }

    /// <summary>
    /// 오브젝트 풀에서 가져올 때 초기화 해주는 함수 
    /// </summary>
    protected virtual void ResetEnemy()
    {
        isDead = false;
        rigid.velocity = Vector2.zero;
    }
    #endregion

#if UNITY_EDITOR
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
#endif
}