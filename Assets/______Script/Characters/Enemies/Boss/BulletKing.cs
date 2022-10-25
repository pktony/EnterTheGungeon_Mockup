using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class BulletKing : MonoBehaviour, IHealth
{
    GameManager gameManager;

    enum AttackSwitch : byte
    {
        FireTell = 0,  // Throne 위로 총알 뭉탱이 발사하고, 8개로 나뉜게 다시 8개로 나뉘는 총알 발사
        FireTell_2,  // 360도 일정한 간격으로 원형 총알 발사
        FireTell_3,  // 360도 일정 각도로 쉼표 총알 발사 (쉼표 총알은 
        Spin,       // 360도 돌때마다 각도가 변하고, 마지막에 360도 전체에 원형 총알 발사
        GobletThrow // 플레이어 근처에 폭발하는 독약 뿌리기
    }

    #region COMPONENT ##########################################################
    private Animator bossAnim;
    private Animator throneAnim;
    private Transform canvas;
    private GameObject intro;
    private GameObject healthBar;
    private Door door;
    private Rigidbody2D rigid;
    private Transform shootPositions;
    private AudioSource source;
    #endregion

    #region VARIABLE ###########################################################
    [Header("AI")]
    [SerializeField] EnemyState status = EnemyState.IDLE;
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float attackCoolTime = 3f;
    [SerializeField] float attackRange = 10f;

    private float currentSpeed = 0f;
    private bool isMove = false;
    private WaitForSeconds updateSeconds;
    private float updateInterval = 1.0f;
    // ######################### ATTACK #############################
    private float attackTimer = 0f;
    private Transform[] evenShootPos;
    private Transform[] oddShootPos;
    public GameObject goblet;

    BulletKing_AnimationHelper animHelp_King;
    BulletKing_AnimationHelper_Throne animHelp_Throne;
    Intro_AnimationHelper animHelp_Intro;

    // ######################## Basic Stat ##########################
    private int hp;
    private const int maxHP = 100;
    #endregion

    #region 델리게이트 ###########################################################
    public Action<int, int> OnTakeDamage { get; set; } // < hp, maxHP>
    public Action<int> OnHPUp { get; set; }
    #endregion

    #region 프로퍼티 #############################################################
    public int HP
    {
        get => hp;
        set
        {
            hp = Mathf.Min(value, maxHP);
            hp = Mathf.Max(0, value);
            if(hp < 1)
                ChangeStatus(EnemyState.DEAD);
            else
                OnTakeDamage?.Invoke(hp, maxHP);    // UI 갱신 
        }
    }

    public int MaxHP => maxHP;
    #endregion

    #region UNITY EVENT 함수 ####################################################
    private void Awake()
    {
        bossAnim = transform.GetChild(0).GetComponent<Animator>();
        throneAnim = transform.GetChild(1).GetComponent<Animator>();
        canvas = transform.GetChild(3);
        healthBar = canvas.GetChild(0).gameObject;
        intro = canvas.GetChild(1).gameObject;
        rigid = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();

        animHelp_King = GetComponentInChildren<BulletKing_AnimationHelper>();
        animHelp_Throne = GetComponentInChildren<BulletKing_AnimationHelper_Throne>();
        animHelp_King.onTell1Attack = Shoot_Tell_1;
        animHelp_King.onTell2Attack = Shoot_Tell_2;
        animHelp_King.onGobletAttack = GobletThrow;
        animHelp_Throne.onSpinAttack_Even = Spin_Even;
        animHelp_Throne.onSpinAttack_Odd = Spin_Odd;

        animHelp_Intro = GetComponentInChildren<Intro_AnimationHelper>();
        animHelp_Intro.onIntroEnd += OnIntroEnd;

        door = FindObjectOfType<Door>();
        intro.SetActive(false);

        updateSeconds = new WaitForSeconds(updateInterval);

        SceneManager.sceneLoaded += OnEntrance;

        InitializeShootPosition();
    }

    private void Start()
    {
        gameManager = GameManager.Inst;
    }
    #endregion

    #region PUBLIC 함수 #########################################################
    public void GobletThrow()
    {
        Instantiate(goblet, transform.position, Quaternion.identity);
        goblet.SetActive(true);
        SoundManager.Inst.PlaySound_Boss(Clips_Boss.Boss_Throw, source);
    }
    #endregion

    #region PRIVATE 함수 #######################################################
    /// <summary>
    /// 원 방정식으로 ShootPositions 일정 간격으로 배치 
    /// </summary>
    void InitializeShootPosition()
    {
        shootPositions = transform.GetChild(4);
        int halfCount = (int)(shootPositions.childCount * 0.5f);    //40개 
        evenShootPos = new Transform[halfCount];
        oddShootPos = new Transform[shootPositions.childCount - halfCount];

        float theta = 0;
        int j = 0;
        int k = 0;
        for (int i = 0; i < shootPositions.childCount; i++)
        {
            theta += 360 / (shootPositions.childCount - 1);
            Transform shootPos = shootPositions.GetChild(i);
            shootPos.localPosition = 1f *
                new Vector2(Mathf.Cos(Mathf.Deg2Rad * theta), Mathf.Sin(Mathf.Deg2Rad * theta));
            shootPos.rotation = Quaternion.Euler(0, 0, theta);

            // 짝수 홀수번째 발사위치 위치 캐싱
            if(i%2 == 0)
            {
                evenShootPos[j] = shootPos;
                j++;
            }
            else
            {
                oddShootPos[k] = shootPos;
                k++;
            }
        }
    }

    private void OnEntrance(Scene arg0, LoadSceneMode arg1)
    {
        StartCoroutine(CheckDoorTrigger());
    }

    /// <summary>
    /// 문이 열렸는지 확인하는 함수 
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckDoorTrigger()
    {
        while (true)
        {
            if(door.IsDoorOpen)
            {
                StartCoroutine(EntranceIntro());
                break;
            }
            yield return null;
        }
    }

    private IEnumerator EntranceIntro()
    {
        bossAnim.SetTrigger("Intro");
        yield return new WaitForSeconds(bossAnim.GetCurrentAnimatorClipInfo(0).Length + 4.0f);
        intro.SetActive(true);
        SoundManager.Inst.PlaySound_Boss(Clips_Boss.Boss_Intro, source);
    }

    private void OnIntroEnd()
    {
        healthBar.SetActive(true);
        HP = MaxHP;
        intro.SetActive(false);
        gameManager.Control.EnableInput();
        ChangeStatus(EnemyState.TRACK);
        StartCoroutine(StatusUpdate());
    }

    private IEnumerator StatusUpdate()
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

    private void IdleUpdate()
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

    private void TrackUpdate()
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

    private void Move(float speed)
    {
        Vector2 dir = gameManager.Player.transform.position - transform.position;
        dir = dir.normalized;
        rigid.MovePosition((Vector2)transform.position + Time.deltaTime * dir * speed);
    }

    private bool IsInAttackRange()
    {
        return (transform.position - gameManager.Player.transform.position)
            .sqrMagnitude < attackRange * attackRange;
    }

    private void AttackUpdate()
    {
        if (attackTimer > attackCoolTime)
        {
            int rand = UnityEngine.Random.Range(0, (int)AttackSwitch.GobletThrow + 1);
            Switcher_Attack(rand);
            if (rand == (int)AttackSwitch.Spin)
            {
                attackTimer = -7f;  // Spin animation duration + alpha
                SoundManager.Inst.PlaySound_Boss(Clips_Boss.Boss_Spin, source);
            }
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

    /// <summary>
    /// 랜덤 공격패턴 
    /// </summary>
    /// <param name="randNumber"></param>
    private void Switcher_Attack(int randNumber)
    {
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

     // Throne 위로 큰 총알을 발사하고, 8개로 나뉜게 다시 8개로 나뉘는 총알 발사
    private void Shoot_Tell_1()
    {
        GameObject bigBullet = BulletManager.Inst.GetPooledBullet(BulletID.BIG);
        bigBullet.transform.SetPositionAndRotation(
            shootPositions.GetChild(10).position, shootPositions.GetChild(10).rotation);
        bigBullet.SetActive(true);

        SoundManager.Inst.PlaySound_Boss(Clips_Boss.Boss_Shot0, source);
    }
    // 360도 일정한 각도로 원형 총알 발사
    private void Shoot_Tell_2()
    {
        for (int i = 0; i < shootPositions.childCount; i++)
        {
            GameObject spearBullet = BulletManager.Inst.GetPooledBullet(BulletID.SPINNING);
            spearBullet.transform.SetPositionAndRotation(
                shootPositions.GetChild(i).position, shootPositions.GetChild(i).rotation);
            spearBullet.SetActive(true);
        }
        SoundManager.Inst.PlaySound_Boss(Clips_Boss.Boss_Shot1, source);
    }

    // 플레이어 방향으로 미식축구공 모양 총알 발사
    private void Shoot_Tell_3()
    {
        Vector2 dir = GameManager.Inst.Player.transform.position - transform.position;

        GameObject obj = BulletManager.Inst.GetPooledBullet(BulletID.FOOTBALL);
        obj.transform.position = dir.normalized;
        //obj.transform.LookAt(GameManager.Inst.Player.transform.position);
        obj.SetActive(true);
    }

    // 360도 돌때마다 각도가 변하고, 마지막에 360도 전체에 원형 총알 발사
    private void Spin_Even()
    {
        GameObject circleBullet;
        for (int i = 0; i < evenShootPos.Length; i ++)
        {
            circleBullet = BulletManager.Inst.GetPooledBullet(BulletID.CIRCLE);
            circleBullet.transform.SetPositionAndRotation(
                evenShootPos[i].position, evenShootPos[i].rotation);
            circleBullet.SetActive(true);
        }
    }

    private void Spin_Odd()
    {
        GameObject circleBullet;
        for (int i = 0; i < oddShootPos.Length; i++)
        {
            circleBullet = BulletManager.Inst.GetPooledBullet(BulletID.CIRCLE);
            circleBullet.transform.SetPositionAndRotation(
                oddShootPos[i].position, oddShootPos[i].rotation);
            circleBullet.SetActive(true);
        }
    }

    private void ChangeStatus(EnemyState newStatus)
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
                SoundManager.Inst.PlaySound_Boss(Clips_Boss.Boss_Explode);
                Destroy(this.gameObject, 5f);
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