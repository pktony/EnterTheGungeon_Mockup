using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D))]
public class BulletKing : MonoBehaviour
{
    enum AttackSwitch : byte
    { 
        FireTell = 0,  // Throne 위로 총알 뭉탱이 발사하고, 8개로 나뉜게 다시 8개로 나뉘는 총알 발사
        FireTell_2,  // 360도 일정 각도로 원형 총알 발사
        FireTell_3,  // 360도 일정 각도로 쉼표 총알 발사
        Spin,       // 360도 돌때마다 각도가 변하고, 마지막에 360도 전체에 원형 총알 발사
        GobletThrow // 플레이어 근처에 폭발하는 독약 뿌리기
    }

    Animator bossAnim;
    Animator throneAnim;
    Rigidbody2D rigid;
    Transform shootPositions;

    [Header("AI")]
    [SerializeField] EnemyState status = EnemyState.IDLE; //IDLE, TRACK, ATTACK, DEAD
    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float attackCoolTime = 3f;
    [SerializeField] float attackRange = 10f;

    float currentSpeed = 0f;
    bool isDead = false;
    // ######################### ATTACK #############################
    float attackTimer = 0f;
    AttackSwitch switcher = AttackSwitch.FireTell;

    private void Awake()
    {
        bossAnim = transform.GetChild(0).GetComponent<Animator>();
        throneAnim = transform.GetChild(1).GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        SceneManager.sceneLoaded += OnEntrance;

        InitializeShootPosition();
    }

    void InitializeShootPosition()
    {
        shootPositions = transform.GetChild(4);
        float theta = 0;
        Vector2 pos = Vector2.zero;
        for (int i = 0; i < shootPositions.childCount; i++)
        {
            shootPositions.GetChild(i).position = 1.8f * new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
            shootPositions.GetChild(i).rotation = Quaternion.Euler(0, 0, theta);
            theta += 360 / (shootPositions.childCount);
        }
        // shoot to the up
        // shootPosition[0] is up

    }

    private void OnEntrance(Scene arg0, LoadSceneMode arg1)
    {
        StartCoroutine(EntranceIntro());
    }

    IEnumerator EntranceIntro()
    {
        // 시작 애니메이션 
        yield return null;
        bossAnim.SetTrigger("Intro");
        yield return new WaitForSeconds(bossAnim.GetCurrentAnimatorClipInfo(0).Length);
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
            yield return null;
        }
    }

    void IdleUpdate()
    {
        if (IsInAttackRange())
        {
            ChangeStatus(EnemyState.ATTACK);
        }
        else
        {
            ChangeStatus(EnemyState.TRACK);
        }
    }

    void TrackUpdate()
    {
        if (!IsInAttackRange())
        {
            Move(currentSpeed);
        }
        else
        {
            ChangeStatus(EnemyState.IDLE);
        }
    }

    void Move(float speed)
    {
        rigid.MovePosition(Time.deltaTime * GameManager.Inst.Player.transform.position * speed);
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
        attackTimer += Time.deltaTime;

        if (attackTimer > attackCoolTime)
        {
            int rand = UnityEngine.Random.Range(0, (int)AttackSwitch.GobletThrow);
            Switcher_Attack(rand);
            attackTimer = 0f;
        }
        else
        {
            ChangeStatus(EnemyState.IDLE);
        }
    }

    void Switcher_Attack(int rand)
    {
        //FireTell = 0,  // Throne 위로 총알 뭉탱이 발사하고, 8개로 나뉜게 다시 8개로 나뉘는 총알 발사
        //FireTell_2,  // 360도 일정 각도로 원형 총알 발사
        //FireTell_3,  // 360도 일정 각도로 쉼표 총알 발사
        //Spin,       // 360도 돌때마다 각도가 변하고, 마지막에 360도 전체에 원형 총알 발사
        //GobletThrow // 플레이어 근처에 폭발하는 독약 뿌리기

        bossAnim.SetFloat("switcher", rand);
        throneAnim.SetFloat("switcher", rand);
    }

    public void Shoot_Tell_1()
    {

    }

    public void Shoot_Tell_2()
    {

    }

    public void Shoot_Tell_3()
    {

    }

    public void Spin()
    {

    }

    public void GobletThrow()
    {

    }

    // ############################# Change Status ######################################
    void ChangeStatus(EnemyState newStatus)
    {
        switch (status)
        { // On Status Exit
            case EnemyState.IDLE:
                break;
            case EnemyState.TRACK:
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



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.forward, attackRange);
    }
#endif
}