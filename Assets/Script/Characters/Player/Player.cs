using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealth
{
    // ########################### Components ##############################
    SpriteRenderer render;

    // ########################### Variables ###############################
    private Transform firePosition = null;
    WeaponInventory_UI inven_UI;
    WeaponInventory inven;
    ItemInventory inven_item;

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
    private Weapons weaponPocket;
    private int currentWeaponIndex = 0;
    private uint slotNumber = 5;
    private SpriteRenderer weaponSprite;
    private WaitForSeconds autoShootTime;

    private int bulletInMagazine;
    private float reloadTimer = 0f;

    // ############################## Properties ###############################
    public WeaponInventory Inven => inven;
    public ItemInventory Inven_Item => inven_item;
    
    public int HP
    {
        get => healtPoint;
        set
        {
            healtPoint = Mathf.Clamp(value, 0, maxHealthPoint);
            onTakeDamage?.Invoke();     //  Heart_UI.cs 
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
            { // Prevent weapon switch on limit.
                currentWeaponIndex = previousWeaponIndex;
                InitializeCurrentWeapon(currentWeaponIndex);
                inven_UI.BulletUI.RefreshBullet_UI();
                onWeaponChange?.Invoke();
            }
        }
    }

    public int BulletinMag
    {
        get => bulletInMagazine;
        set
        {
            bulletInMagazine = value;
        }
    }

    public bool IsReloading => isReloading;

    // ################################ Deligates #############################
    public System.Action onTakeDamage {get; set;}
    public System.Action onHPUp { get; set; }

    public System.Action onFireReload;
    public System.Action onWeaponChange;

    private void Awake()
    {
        render = GetComponent<SpriteRenderer>();
        blinkTime = new WaitForSeconds(invincibleTime);

        inven_UI = FindObjectOfType<WeaponInventory_UI>();

        weaponPocket = GetComponentInChildren<Weapons>();
        firePosition = weaponPocket.transform.GetChild(0).GetChild(0);
        weaponSprite = weaponPocket.GetComponentInChildren<SpriteRenderer>();

        inven = new WeaponInventory(slotNumber);
        inven.AddItem(WeaponType.PISTOL);
        inven.AddItem(WeaponType.RIFLE);
        inven_UI.InitializeInventory(inven);

        inven_item = new ItemInventory();
        inven_item.InitializeItemInventory();
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
                if (currentWeapon.remainingBullet > currentWeapon.maxBulletMagazine)
                { // Enough bullets
                    bulletInMagazine = currentWeapon.maxBulletMagazine;
                    currentWeapon.remainingBullet -= bulletInMagazine ;
                }
                else
                { // Last magazine
                    if (currentWeapon.maxBulletNum < 0)
                    {// infinite bullets [MaxBulletNum = -1]
                        currentWeapon.remainingBullet = currentWeapon.maxBulletMagazine;
                    }
                    bulletInMagazine = currentWeapon.remainingBullet;
                    currentWeapon.remainingBullet = 0;
                }
                onFireReload?.Invoke();     // Refresh Bullet UIs (Bullet_UI.cs)
                reloadTimer = 0f;
                isReloading = false;
            }
        }
    }

    // ############################## Methods ####################################
    public void InitializeCurrentWeapon(int weaponSlotNumber)
    {
        if (inven_UI.SlotUIs[weaponSlotNumber].Weapon_Slot.WeaponSlotData != null)
        {
            currentWeapon = inven_UI.SlotUIs[weaponSlotNumber].Weapon_Slot.WeaponSlotData;
            weaponSprite.sprite = inven_UI.SlotUIs[weaponSlotNumber].Weapon_Slot.WeaponSlotData.weaponIcon;
            bulletInMagazine = currentWeapon.maxBulletMagazine;
            autoShootTime = new WaitForSeconds(currentWeapon.fireRate);
        }
    }

    public IEnumerator Fire()
    {
        while (bulletInMagazine > 0)
        {
            GameManager.Inst.Control.FireDirection = GameManager.Inst.Control.LookDir.normalized;

            GameObject bullet = BulletManager.Inst.GetPooledBullet(BulletManager.PooledBullets[BulletManager.Inst.PlayerBulletID]);
            bullet.transform.position = firePosition.position;
            bullet.transform.rotation = firePosition.rotation
                * Quaternion.Euler(0f, 0f, Random.Range(-currentWeapon.dispersion, currentWeapon.dispersion));
            bullet.SetActive(true);

            //currentWeapon.remainingBullet -= currentWeapon.bulletPerFire;
            bulletInMagazine -= currentWeapon.bulletPerFire;
            onFireReload?.Invoke();

            yield return autoShootTime;
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
        if (!canDodge)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Invincible");
            dodgeDuration -= Time.deltaTime;
            transform.Translate(dodgeSpeed * Time.deltaTime * dodgeDir);
            if (dodgeDuration < 0f)
            {
                dodgeDuration = 0.7f;
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
        if (collision.CompareTag("EnemyBullets"))
        {
            StartCoroutine(Blink());
            HP -= 1;
        }
    }
}