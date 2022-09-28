using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using Unity.VisualScripting;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class BulletKing : MonoBehaviour, IHealth
{
    enum AttackSwitch : byte
    {
        FireTell = 0,  // Throne 위로 총알 뭉탱이 발사하고, 8개로 나뉜게 다시 8개로 나뉘는 총알 발사
        FireTell_2,  // 360도 일정한 간격으로 원형 총알 발사
        FireTell_3,  // 360도 일정 각도로 쉼표 총알 발사 (쉼표 총알은 
        Spin,       // 360도 돌때마다 각도가 변하고, 마지막에 360도 전체에 원형 총알 발사
        GobletThrow // 플레이어 근처에 폭발하는 독약 뿌리기
    }

    #region COMPONENT
    Animator bossAnim;
    Animator throneAnim;
    Transform canvas;
    GameObject intro;
    GameObject healthBar;
    Rigidbody2D rigid;
    Transform shootPositions;
    #endregion

    #region VARIABLE
    [Header("AI")]
    [SerializeField] EnemyState status = EnemyState.IDLE; //IDLE, TRACK, ATTACK, DEAD
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float attackCoolTime = 3f;
    [SerializeField] float attackRange = 10f;

    float currentSpeed = 0f;
    bool isDead = false;
    bool isMove = false;
    WaitForSeconds updateSeconds;
    float updateInterval = 1.0f;
    // ######################### ATTACK #############################
    float attackTimer = 0f;
    BulletKing_AnimationHelper animHelp_King;
    BulletKing_AnimationHelper_Throne animHelp_Throne;
    Intro_AnimationHelper animHelp_Intro;

    // ######################## Basic Stat ##########################
    private int hp;
    private const int maxHP = 100;

    public Action OnTakeDamage { get; set; }
    public Action OnHPUp { get; set; }
    #endregion

    #region Property
    public int HP
    {
        get => hp;
        set
        {
            hp = Mathf.Min(value, maxHP);
            hp = Mathf.Max(0, value);
            OnTakeDamage?.Invoke();
        }
    }

    public int MaxHP => maxHP;
    #endregion

    private void Awake()
    {
        bossAnim = transform.GetChild(0).GetComponent<Animator>();
        throneAnim = transform.GetChild(1).GetComponent<Animator>();
        canvas = transform.GetChild(3);
        healthBar = canvas.GetChild(0).gameObject;
        intro = canvas.GetChild(1).gameObject;
        rigid = GetComponent<Rigidbody2D>();
        animHelp_King = GetComponentInChildren<BulletKing_AnimationHelper>();
        animHelp_Throne = GetComponentInChildren<BulletKing_AnimationHelper_Throne>();
        animHelp_King.onTell1Attack = Shoot_Tell_1;
        animHelp_King.onTell2Attack = Shoot_Tell_2;
        animHelp_King.onGobletAttack = GobletThrow;
        animHelp_Throne.onSpinAttack_Even = Spin_Even;
        animHelp_Throne.onSpinAttack_Odd = Spin_Odd;

        animHelp_Intro = GetComponentInChildren<Intro_AnimationHelper>();
        animHelp_Intro.onIntroEnd += OnIntroEnd;
        intro.SetActive(false);

        updateSeconds = new WaitForSeconds(updateInterval);

        SceneManager.sceneLoaded += OnEntrance;

        InitializeShootPosition();
    }

    private void Start()
    {
        
    }

    void InitializeShootPosition()
    {
        shootPositions = transform.GetChild(4);

        float theta = 0;
        for (int i = 0; i < shootPositions.childCount; i++)
        {
            theta += 360 / (shootPositions.childCount - 1);
            shootPositions.GetChild(i).position = 1f *
                new Vector2(Mathf.Cos(Mathf.Deg2Rad * theta), Mathf.Sin(Mathf.Deg2Rad * theta));
            shootPositions.GetChild(i).rotation = Quaternion.Euler(0, 0, theta);
        }
    }

    private void OnEntrance(Scene arg0, LoadSceneMode arg1)
    {
        StartCoroutine(EntranceIntro());
    }

    IEnumerator EntranceIntro()
    {
        bossAnim.SetTrigger("Intro");
        yield return new WaitForSeconds(bossAnim.GetCurrentAnimatorClipInfo(0).Length + 2.0f);
        intro.SetActive(true);
    }

    void OnIntroEnd()
    {
        healthBar.SetActive(true);
        HP = MaxHP;
        intro.SetActive(false);
        ChangeStatus(EnemyState.TRACK);
        StartCoroutine(StatusUpdate());
    }

    IEnumerator StatusUpdate()
    {
        while (true)
        {
            switch (status)
            {
                case EnemyState.IDLE:
                    IdleUpdate();
                    break;
                case EnemyState.TRACK:
                    TrackUpdate();
                    break;
                case EnemyState.ATTACK:
                    AttackUpdate();
                    break;
                case EnemyState.DEAD:
                    break;
            }
            yield return updateSeconds;
        }
    }

    private void FixedUpdate()
    {
        if(isMove)
        {
            Move(currentSpeed);
        }
    }

    void IdleUpdate()
    {
        if (IsInAttackRange())
        {
            attackTimer += updateInterval;
            if (attackTimer > attackCoolTime)
            {
                ChangeStatus(EnemyState.ATTACK);
                return;
            }
        }
        else
        {
            ChangeStatus(EnemyState.TRACK);
        }
    }

    void TrackUpdate()
    {
        if(!IsInAttackRange())
        {
            isMove = true;
            attackTimer += updateInterval;
        }
        else
        {
            ChangeStatus(EnemyState.ATTACK);
        }
    }

    void Move(float speed)
    {
        Vector2 dir = GameManager.Inst.Player.transform.position - transform.position;
        dir = dir.normalized;
        rigid.MovePosition((Vector2)transform.position + Time.deltaTime * dir * speed);
    }

    bool IsInAttackRange()
    {
        bool result = false;
        if ((transform.position - GameManager.Inst.Player.transform.position).sqrMagnitude < attackRange * attackRange)
        {
            result = true;
        }

        return result;
    }

    void AttackUpdate()
    {
        if (attackTimer > attackCoolTime)
        {
            int rand = UnityEngine.Random.Range(0, (int)AttackSwitch.GobletThrow + 1);
            Switcher_Attack(rand);
            if (rand == (int)AttackSwitch.Spin)
                attackTimer = -7f;  // Spin animation duration + alpha
            else
                attackTimer = 0f;
        }
        else
        {
            ChangeStatus(EnemyState.IDLE);
        }

        if (!IsInAttackRange())
            ChangeStatus(EnemyState.TRACK);
    }

    void Switcher_Attack(int randNumber)
    {
        //FireTell = 0,  // Throne 위로 큰 총알을 발사하고, 8개로 나뉜게 다시 8개로 나뉘는 총알 발사
        //FireTell_2,  // 360도 일정한 각도로 원형 총알 발사
        //FireTell_3,  // 플레이어 방향으로 미식축구공 모양 총알 발사
        //FireTell_4   // 일정 간격으로 360도 발사
        //Spin,       // 360도 돌때마다 각도가 변하고, 마지막에 360도 전체에 원형 총알 발사
        //GobletThrow // 플레이어 근처에 폭발하는 독약 뿌리기
        bossAnim.SetTrigger("onAttack");
        throneAnim.SetTrigger("onAttack");
        bossAnim.SetInteger("Switcher", randNumber);
        throneAnim.SetInteger("Switcher", randNumber);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBullets"))
        {
            HP -= 3;
            bossAnim.SetTrigger("onHit"); 
        }
    }


    #region PROJECTILE METHOD
    private void Shoot_Tell_1()
    {
        GameObject bigBullet = BulletManager.Inst.GetPooledBullet(BulletID.BIG);
        bigBullet.transform.position = shootPositions.GetChild(10).position;
        bigBullet.transform.rotation = shootPositions.GetChild(10).rotation;
        bigBullet.SetActive(true);
    }

    private void Shoot_Tell_2()
    {
        for (int i = 0; i < shootPositions.childCount; i++)
        {
            GameObject spearBullet = BulletManager.Inst.GetPooledBullet(BulletID.SPINNING);
            spearBullet.transform.position = shootPositions.GetChild(i).position;
            spearBullet.transform.rotation = shootPositions.GetChild(i).rotation;
            spearBullet.SetActive(true);
        }
    }

    private void Shoot_Tell_3()
    {
        Vector2 dir = GameManager.Inst.Player.transform.position - transform.position;

        GameObject obj = BulletManager.Inst.GetPooledBullet(BulletID.FOOTBALL);
        obj.transform.position = dir.normalized;
        //obj.transform.LookAt(GameManager.Inst.Player.transform.position);
        obj.SetActive(true);
    }

    private void Spin_Even()
    {
        for (int i = 0; i < shootPositions.childCount; i ++)
        {
            if(i%2==0)
            {
                GameObject circleBullet = BulletManager.Inst.GetPooledBullet(BulletID.CIRCLE);
                circleBullet.transform.position = shootPositions.GetChild(i).position;
                circleBullet.transform.rotation = shootPositions.GetChild(i).rotation;
                circleBullet.SetActive(true);
            }
        }
    }

    private void Spin_Odd()
    {
        for (int i = 0; i < shootPositions.childCount; i++)
        {
            if (i % 2 != 0)
            {
                GameObject circleBullet = BulletManager.Inst.GetPooledBullet(BulletID.CIRCLE);
                circleBullet.transform.position = shootPositions.GetChild(i).position;
                circleBullet.transform.rotation = shootPositions.GetChild(i).rotation;
                circleBullet.SetActive(true);
            }
        }
    }

    public GameObject goblet;
    public void GobletThrow()
    {
        Instantiate(goblet, transform.position, quaternion.identity);
        goblet.SetActive(true);
    }
    #endregion

    #region Status Change
    void ChangeStatus(EnemyState newStatus)
    {
        switch (status)
        { // On Status Exit
            case EnemyState.IDLE:
                break;
            case EnemyState.TRACK:
                isMove = false;
                break;
            case EnemyState.ATTACK:
                break;
            case EnemyState.DEAD:
                break;
        }

        status = newStatus;

        switch (status)
        { // On Status Enter
            case EnemyState.IDLE:
                currentSpeed = 0f;
                break;
            case EnemyState.TRACK:
                currentSpeed = moveSpeed;
                isMove = true;
                break;
            case EnemyState.ATTACK:
                currentSpeed = 0f;
                break;
            case EnemyState.DEAD:
                isDead = true;
                break;
        }

        bossAnim.SetInteger("Status", (int)status);
        throneAnim.SetInteger("Status", (int)status);
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.forward, attackRange);
    }
#endif
}