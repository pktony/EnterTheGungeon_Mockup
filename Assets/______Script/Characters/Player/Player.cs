using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealth
{
    #region 컴포넌트 #############################################################
    SpriteRenderer render;
    AudioSource source;
    #endregion

    #region 변수 ###############################################################
    WeaponInventory_UI inven_UI;
    WeaponInventory inven;
    ItemInventory inven_item;
    Reload_UI reloadUI;

    // -- HP
    private int healthPoint = 6;
    private int maxHealthPoint = 12;

    // -- Hit
    private bool isHit = false;
    [SerializeField] private float invincibleTime = 2.0f;
    private float blinkTimer = 0.0f;
    WaitForSeconds blinkTime;

    // -- Dodge 
    [SerializeField] private float dodgeSpeed = 5.0f;
    private float dodgeDuration = 0.7f;
    private float dodgeTimer;
    public bool canDodge = true;
    public Vector2 dodgeDir = Vector2.zero;

    // -- Weapon
    [HideInInspector] public bool hasWeapon = true;
    private bool isReloading = false;
    private WeaponData currentWeapon;
    private WeaponPocket weaponPocket;
    private int currentWeaponIndex = 0;
    private uint weaponSlotNumber = 3;
    private WaitForSeconds autoShootTime;
    private AudioClip weaponShootClip;
    private float shootVolume;

    private int bulletInMag;
    private float reloadTimer = 0f;
    #endregion

    #region 프로퍼티 #############################################################
    public WeaponInventory Inven => inven;
    public WeaponInventory_UI W_InvenUI => inven_UI;
    public ItemInventory Inven_Item => inven_item;
    public WeaponPocket WeaponPoc => weaponPocket;
    
    public int HP
    {
        get => healthPoint;
        set
        {
            int prevValue = value;
            healthPoint = Mathf.Clamp(value, 0, maxHealthPoint);
            if (value < prevValue)
            {
                OnTakeDamage?.Invoke(healthPoint, maxHealthPoint);     //  Heart_UI.cs
            }
            else
            {
                OnHPUp?.Invoke(healthPoint);
            }
        }
    }
    public int MaxHP => maxHealthPoint;
    public WeaponData CurrentWeapon 
    {
        get => currentWeapon;
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

    public int BulletInMag => bulletInMag;

    public bool IsReloading => isReloading;
    public WeaponPocket Weaponpocket => weaponPocket;
    #endregion

    #region 델리게이트 ###########################################################
    public System.Action<int, int> OnTakeDamage {get; set;} // < hp, maxhp>
    public System.Action<int> OnHPUp { get; set; }
    public System.Action onFireReload;
    public delegate IEnumerator onReloadStart();
    public System.Action onWeaponChange;
    #endregion

    #region UNITY EVENT 함수 ####################################################
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        render = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
        blinkTime = new WaitForSeconds(invincibleTime);

        inven_UI = FindObjectOfType<WeaponInventory_UI>();
        reloadUI = transform.GetChild(1).GetComponentInChildren<Reload_UI>();

        weaponPocket = GetComponentInChildren<WeaponPocket>();

        inven = new WeaponInventory(weaponSlotNumber);
        inven.AddItem(WeaponType.PISTOL);
        inven.AddItem(WeaponType.RIFLE);
        inven_UI.InitializeInventory(inven);

        inven_item = new ItemInventory();
        inven_item.InitializeItemInventory();
        inven_item.Slots[(int)ItemType.BlankShell].IncreaseItem(3);
    }

    private void Start()
    {
        weaponPocket.InitializeWeaponAnimator(0);
    }

    private void Update()
    {
        if (isHit)
        {
            blinkTimer += Time.deltaTime;
            render.color = new Color(1, 1, 1, 1 - Mathf.Cos(blinkTimer * 0.2f * Mathf.Rad2Deg));
        }
    }
    #endregion

    #region PUBLIC 함수 #########################################################
    public void InitializeCurrentWeapon(int weaponSlotNumber)
    {
        if (inven_UI.SlotUIs[weaponSlotNumber].Weapon_Slot.WeaponSlotData != null)
        {
            currentWeapon = inven_UI.SlotUIs[weaponSlotNumber].Weapon_Slot.WeaponSlotData;
            bulletInMag = currentWeapon.bulletPerMagazine;
            weaponPocket.InitializeWeaponAnimator(weaponSlotNumber);
            weaponPocket.PlayIdleAnimation();
            autoShootTime = new WaitForSeconds(currentWeapon.fireRate);
            weaponShootClip = CurrentWeapon.fireSound;
            shootVolume = currentWeapon.fireVolume;
        }
    }

    public IEnumerator Fire()
    {
        while (bulletInMag > 0)
        {
            GameObject bullet = BulletManager.Inst.GetPooledBullet(BulletType.PLAYER);
            bullet.transform.position = weaponPocket.SetupFirePosition();
            bullet.transform.rotation = weaponPocket.SetupFireRotation()
                * Quaternion.Euler(0f, 0f, Random.Range(-currentWeapon.dispersion, currentWeapon.dispersion));
            bullet.SetActive(true);
            
            bulletInMag -= currentWeapon.bulletPerFire;
            CameraShake.ShakeCamera(0.1f, 0.5f);
            source.PlayOneShot(weaponShootClip, shootVolume * GameManager.Inst.Volume_VFX);
            onFireReload?.Invoke();

            yield return autoShootTime;
        }
    }

    /// <summary>
    /// 장전 명령을 내리는 함수 
    /// </summary>
    public void Reload()
    {
        if (!isReloading)
        {
            weaponPocket.PlayReloadAnimation();
            StartCoroutine(ReloadAmmo());
            SoundManager.Inst.PlaySound_Weapon(Clips_Weapon.Deagle_Reload);
        }
    }

    public void Dodge()
    {
        if (!canDodge)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Invincible");
            dodgeTimer += Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position,
                (Vector2)transform.position + dodgeDir, dodgeSpeed * Time.deltaTime);
            if (dodgeTimer > dodgeDuration)
            {
                dodgeTimer = 0;
                this.gameObject.layer = LayerMask.NameToLayer("Player");
                canDodge = true;
            }
        }
    }
    #endregion

    #region PRIVATE 함수 ########################################################
    /// <summary>
    /// ReloadUI를 재생하는 함수 
    /// </summary>
    /// <returns></returns>
    IEnumerator ReloadAmmo()
    {
        reloadTimer = 0f;
        isReloading = true;
        onReloadStart startReload = reloadUI.ReloadUI;
        StartCoroutine(startReload());

        while (isReloading)
        { 
            reloadTimer += Time.deltaTime;
            ReplenishAmmo();
            yield return null;
        }
    }

    /// <summary>
    /// 실제 총알 개수를 refresh하는 함수
    /// </summary>
    void ReplenishAmmo()
    {
        if (reloadTimer > currentWeapon.reloadingTime)
        {
            // remainingBullet
            if (currentWeapon.bulletsInPocket > currentWeapon.bulletPerMagazine)
            { // 재장전할 총알이 충분하다 
                currentWeapon.bulletsInPocket -= (currentWeapon.bulletPerMagazine);
                bulletInMag = currentWeapon.bulletPerMagazine;
            }
            else
            { // Last magazine
                if (currentWeapon.maxBulletNum < 0)
                {// infinite bullets [MaxBulletNum = -1]
                    currentWeapon.bulletsInPocket = currentWeapon.bulletPerMagazine;
                }
                bulletInMag = currentWeapon.bulletsInPocket;
                currentWeapon.bulletsInPocket = 0;
            }
            onFireReload?.Invoke();     // Refresh Bullet UIs (Bullet_UI.cs)
            reloadTimer = 0f;
            isReloading = false;
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
            if (!isHit)
            {
                StartCoroutine(Blink());
                HP -= 1;
            }
        }
    }
    #endregion
}