using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealth, IBattle
{
    // ########################### Components ##############################
    SpriteRenderer render;

    // ################################ Variables ##########################
    public Transform FirePosition = null;
    WeaponInventory_UI inven_UI;

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

    // -- Weapon
    public bool hasWeapon = true;
    bool isReloading = false;
    WeaponData currentWeapon;
    WeaponData[] weaponInven;
    int bulletInMagazine;
    int bulletsRemaining = 5;
    float reloadTimer = 0f;

    public int BulletinMag => bulletInMagazine;


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
    public WeaponData CurrentWeapon => currentWeapon;
    public int BulletsRemaining
    {
        get => bulletsRemaining;
        set
        {
            bulletsRemaining = value;
            // ShowRemainingBullets 델리게이트 만들기
        }
    }

    // ################################ Deligates #############################
    //public System.Action OnShoot;
    public System.Action onTakeDamage {get; set;}
    public System.Action onHPUp { get; set; }

    public System.Action onFireReload;

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

        inven_UI = FindObjectOfType<WeaponInventory_UI>();
    }

    private void Update()
    {
        if (isHit)
        {
            blinkTimer += Time.deltaTime;
            render.color = new Color(1, 1, 1, 1 - Mathf.Cos(blinkTimer * 0.2f * Mathf.Rad2Deg));
        }

        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer > currentWeapon.reloadingTime)
            {
                if (BulletsRemaining > currentWeapon.maxBulletMagazine)
                {
                    bulletInMagazine = currentWeapon.maxBulletMagazine;
                    BulletsRemaining -= currentWeapon.maxBulletMagazine;
                }
                else
                {
                    bulletInMagazine = BulletsRemaining;
                    BulletsRemaining = 0;
                }
                onFireReload?.Invoke();
                reloadTimer = 0f;
                isReloading = false;
            }
        }
    }


    // ############################## Methods ####################################
    public void InitializeCurrentWeapon()
    {
        currentWeapon = inven_UI.SlotUIs[0].Weapon_Slot.WeaponSlotData;
        BulletsRemaining = currentWeapon.maxBulletNum;
        bulletInMagazine = CurrentWeapon.maxBulletMagazine;
    }

    public void Fire()
    {
        if (bulletInMagazine > 0)
        {
            GameObject bullet = BulletManager.Inst.GetPooledBullet(BulletManager.PooledBullets[BulletManager.Inst.PlayerBulletID]);
            bullet.transform.position = FirePosition.position;
            bullet.transform.rotation = FirePosition.rotation;
            BulletsRemaining -= currentWeapon.bulletPerFire;
            bulletInMagazine -= currentWeapon.bulletPerFire;
            onFireReload?.Invoke();
        }
        else
        {
            Reload();
        }
    }

    void Reload()
    {
        if (!isReloading)
        {
            Debug.Log("Reloading");
            reloadTimer = 0f;
            isReloading = true;
        }
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

    void NextWeapon(int index)
    {
        currentWeapon = inven_UI.SlotUIs[index].Weapon_Slot.WeaponSlotData;
    }
}
