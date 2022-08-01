using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealth
{
    // ########################### Components ##############################
    SpriteRenderer render;

    // ########################### Variables ###############################
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
    private float dodgeDuration = 0.7f;
    public bool canDodge = true;
    public Vector2 dodgeDir = Vector2.zero;

    // -- Weapon
    [HideInInspector] public bool hasWeapon = true;
    private bool isReloading = false;
    private WeaponData currentWeapon;
    private int currentWeaponIndex = 0;
    private Weapons weaponPocket;
    private SpriteRenderer weaponSprite;

    private int bulletInMagazine;
    private int bulletsRemaining = 5;
    private float reloadTimer = 0f;

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
    public WeaponData CurrentWeapon 
    {
        get => currentWeapon;
        set
        { 
            currentWeapon = value;
        }
    }
    public int CurrentWeaponIndex
    {
        get => currentWeaponIndex;
        set
        {
            int previousWeaponIndex = Mathf.Clamp(value, 0, inven_UI.SlotUIs.Length - 1);
            if (currentWeaponIndex != previousWeaponIndex)
            { // Preven weapon switch on limit.
                currentWeaponIndex = previousWeaponIndex;
                InitializeCurrentWeapon(currentWeaponIndex);
                inven_UI.BulletUI.RefreshBullet_UI();
            }
        }
    }

    public int BulletsRemaining
    {
        get => bulletsRemaining;
        set
        {
            bulletsRemaining = value;
            // ShowRemainingBullets 델리게이트 만들기
        }
    }
    public int BulletinMag => bulletInMagazine;

    public bool IsReloading => isReloading;

    // ################################ Deligates #############################
    //public System.Action OnShoot;
    public System.Action onTakeDamage {get; set;}
    public System.Action onHPUp { get; set; }

    public System.Action onFireReload;


    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        blinkTime = new WaitForSeconds(invincibleTime);

        inven_UI = FindObjectOfType<WeaponInventory_UI>();

        weaponPocket = GetComponentInChildren<Weapons>();
        weaponSprite = weaponPocket.GetComponentInChildren<SpriteRenderer>();
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
                { // Bullets enough
                    bulletInMagazine = currentWeapon.maxBulletMagazine;
                    BulletsRemaining -= currentWeapon.maxBulletMagazine;
                }
                else
                { // Last magazine
                    if (currentWeapon.maxBulletNum < 0)
                    {// infinite bullets [MaxBulletNum = -1]
                        BulletsRemaining = currentWeapon.maxBulletMagazine;
                    }
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
    public void InitializeCurrentWeapon(int weaponSlotNumber)
    {
        currentWeapon = inven_UI.SlotUIs[weaponSlotNumber].Weapon_Slot.WeaponSlotData;
        weaponSprite.sprite = inven_UI.SlotUIs[weaponSlotNumber].Weapon_Slot.WeaponSlotData.weaponIcon;

        BulletsRemaining = CurrentWeapon.maxBulletNum;
        bulletInMagazine = CurrentWeapon.maxBulletMagazine;
    }

    public void Fire()
    {
        if (bulletInMagazine > 0)
        {
            GameObject bullet = BulletManager.Inst.GetPooledBullet(BulletManager.PooledBullets[BulletManager.Inst.PlayerBulletID]);
            bullet.transform.position = FirePosition.position;
            bullet.transform.rotation = FirePosition.rotation * Quaternion.Euler(0, 0, Random.Range(0.8f, 1.2f));
            BulletsRemaining -= currentWeapon.bulletPerFire;
            bulletInMagazine -= currentWeapon.bulletPerFire;
            onFireReload?.Invoke();
        }
        else
        {
            Reload();
        }
    }

    public void Reload()
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullets"))
        {
            HP -= 1;
            onTakeDamage?.Invoke();
            StartCoroutine(Blink());
        }
    }
}
