using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealth, IBattle
{
    // ########################### Components ##############################
    SpriteRenderer render;

    // ################################ Variables ###########################3
    public Transform FirePosition = null;
    public bool hasWeapon = true;
    

    // -- HP
    private int healtPoint = 6;
    private int maxHealthPoint = 12;

    // -- Hit
    private bool isHit = false;
    [SerializeField] private float invincibleTime = 2.0f;
    [SerializeField] private float blinkTimer = 0.0f;
    WaitForSeconds blinkTime;

    // -- Dodge 
    [SerializeField] private float dodgeSpeed = 5.0f;
    private float dodgeDuration = 0.5f;
    public bool canDodge = true;
    public Vector2 dodgeDir = Vector2.zero;


    // ############################## Properties ###############################
    public int HP
    {
        get => healtPoint;
        set
        {
            healtPoint = Mathf.Clamp(value, 0, maxHealthPoint);
        }
    }
    public int MaxHP => maxHealthPoint;

    // ################################ Deligates #############################
    public System.Action OnShoot;
    public System.Action onTakeDamage { get; set;}
    public System.Action onHPUp { get; set; }

    // ################################ IBattle #################################
    public void TakeDamage(int damage)
    {
        HP -= damage;
        onTakeDamage?.Invoke();
        StartCoroutine(Blink());
    }

    public void Attack(IBattle _)
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        blinkTime = new WaitForSeconds(invincibleTime);
    }

    private void Update()
    {
        if (isHit)
        {
            blinkTimer += Time.deltaTime;
            render.color = new Color(1, 1, 1, 1 - Mathf.Cos(blinkTimer * 0.2f * Mathf.Rad2Deg));
        }
    }


    // ############################## Methods ####################################
    public void Fire()
    {
        GameObject bullet = BulletManager.Bullet_Inst.GetPlayerBullet();
        bullet.transform.position = FirePosition.position;
        bullet.transform.rotation = FirePosition.rotation;
        OnShoot?.Invoke();
    }

    public void Dodge()
    {
        canDodge = false;

        if (!canDodge)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Invincible");
            dodgeDuration -= Time.deltaTime;
            transform.Translate(dodgeSpeed * Time.deltaTime * dodgeDir);
            if (dodgeDuration < 0f)
            {
                dodgeDuration = 0.5f;
                this.gameObject.layer = LayerMask.NameToLayer("Player");
                canDodge = true;
            }
        }
    }

    private IEnumerator Blink()
    {
        isHit = true;
        this.gameObject.layer = LayerMask.NameToLayer("Invincible");
        yield return blinkTime;
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        isHit = false;
        render.color = new Color(1, 1, 1, 1);
    }
}
