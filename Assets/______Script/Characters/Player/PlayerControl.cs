using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerControl : MonoBehaviour
{
    PlayerInput input = null;
    Animator anim = null;
    AudioSource source_Footstep;
    WeaponPocket weaponPocket;
    Player player = null;
    BlankFX blankFX;

    PlayerMove moveMode = PlayerMove.WALK;

    //################ Move ##############################
    [SerializeField] private float moveSpeed = 3.0f;
    Vector3 inputDir = Vector3.zero;
    AudioClip footstepClip;

    //################ Look ##############################
    Vector2 lookDir = Vector2.zero;

    //################ Weapon ##############################
    private SpriteRenderer weaponSprite = null;
    [SerializeField] private Vector2 weaponRightOffset;

    //################ Fire ##############################
    IEnumerator autoFire;

    //################ Dodge #############################
    //################ Loot ##############################
    [SerializeField] private float interactRange = 2.0f;
    private Collider2D[] lootColliders = new Collider2D[4];
    //################ Camera #############################
    private Vector3 camPosition = Vector3.zero;

    #region 프로퍼티 #############################################################
    public Vector2 LookDir => lookDir;

    public PlayerMove MoveMode
    {
        get => moveMode;
        set
        {
            moveMode = value;
        }
    }
    #endregion

    #region 델리게이트 ###########################################################
    public System.Action onLoot;
    #endregion

    #region UNITY EVENT 함수 ####################################################
    private void Awake()
    {
        input = new();
        anim = GetComponent<Animator>();
        source_Footstep = GetComponent<AudioSource>();

        player = GetComponent<Player>();
        weaponPocket = player.Weaponpocket;
        weaponSprite = weaponPocket.GetComponentInChildren<SpriteRenderer>();
        blankFX = GetComponentInChildren<BlankFX>();

        anim.SetBool("hasWeapon", player.hasWeapon);
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Look.performed += OnLookInput;
        input.Player.Move.performed += OnMoveInput;
        input.Player.Move.canceled += OnMoveInput;
        input.Player.Dodge.performed += OnDodgeInput;
        input.Player.Reload.performed += OnReloadInput;
        input.Player.ChangeWeapons.performed += OnChangeWeapon;
        input.Player.Shoot.performed += OnShoot;
        input.Player.Shoot.canceled += OnShoot;
        input.Player.Blank.performed += OnBlankUse;
        input.Player.Interact.performed += OnInteraction;
        input.Player.DropWeapon.performed += OnWeaponDrop;
    }

    private void OnDisable()
    {
        input.Player.Look.performed -= OnLookInput;
        input.Player.Move.performed -= OnMoveInput;
        input.Player.Move.canceled -= OnMoveInput;
        input.Player.Dodge.performed -= OnDodgeInput;
        input.Player.Reload.performed -= OnReloadInput;
        input.Player.Shoot.performed -= OnShoot;
        input.Player.Shoot.canceled -= OnShoot;
        input.Player.Blank.performed -= OnBlankUse;
        input.Player.Interact.performed -= OnInteraction;
        input.Player.DropWeapon.performed -= OnWeaponDrop;
        input.Player.Disable();
    }

    private void Start()
    {
        footstepClip = SoundManager.Inst.clips_Player[(int)Clips_Player.Footstep1].clip;
    }

    private void Update()
    {
        if (inputDir.sqrMagnitude > 0)
        {
            if (moveMode == PlayerMove.DODGE)
            {
                player.Dodge();
                return;
            }
            else if(moveMode == PlayerMove.WALK)
            {
                anim.SetFloat("Speed", 1.0f);
                Move(moveSpeed);        // Leave parameter in case of RUN state
            }
        }
        else
        {
            moveMode = PlayerMove.IDLE;
            anim.SetFloat("Speed", 0f);
        }

        RotateWeapon();
    }
    #endregion

    #region PUBLIC 함수 ########################################################
    public void DisableInput()
    {
        input.Player.Disable();
        inputDir = Vector3.zero;
    }

    public void EnableInput()
    {
        input.Player.Enable();
    }

    /// <summary>
    /// 애니메이션 이벤트 함수
    /// </summary>
    public void PlayFootstepSound()
    {
        source_Footstep.PlayOneShot(footstepClip,
            GameManager.Inst.Volume_Master * GameManager.Inst.Volume_VFX);
    }
    #endregion

    void Move(float speed)
    {
        transform.position += Time.deltaTime * speed * inputDir;
    }
    private void RotateWeapon()
    {
        weaponPocket.transform.right = lookDir;

        if (lookDir.x < 0)
        {// Left
            weaponSprite.flipY = true;
            MirrorWeaponPosition(-weaponRightOffset);
        }
        else
        {// Right
            weaponSprite.flipY = false;
            MirrorWeaponPosition(weaponRightOffset);
        }
    }

    private void MirrorWeaponPosition(Vector3 pos)
    {
        weaponPocket.transform.localPosition = pos;
    }

    void OnLookInput(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>();

        lookDir = (Camera.main.ScreenToWorldPoint(mouseInput) - transform.position).normalized;
        
        camPosition = Camera.main.ScreenToWorldPoint(mouseInput);
        camPosition.z = -10.0f;

        anim.SetFloat("Mouse_X", lookDir.x);
        anim.SetFloat("Mouse_Y", lookDir.y);
    }

    void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputDir = context.ReadValue<Vector3>().normalized;
            moveMode = PlayerMove.WALK;
            anim.SetFloat("Keyboard_X", inputDir.x);
            anim.SetFloat("Keyboard_Y", inputDir.y);
        }
    }

   
    void OnDodgeInput(InputAction.CallbackContext _)
    {
        if(moveMode == PlayerMove.WALK)
        { // Dodge only when moving
            anim.SetTrigger("OnDodge");
            player.dodgeDir = inputDir;
            anim.SetFloat("dodgeDir_X", inputDir.x);
            anim.SetFloat("dodgeDir_Y", inputDir.y);

            player.canDodge = false;
            moveMode = PlayerMove.DODGE;
        }
    }

    void OnChangeWeapon(InputAction.CallbackContext number)
    {
        float scroll = number.ReadValue<float>();

        if (scroll < 0)
        {// Previous weapon
            player.CurrentWeaponIndex--;
        }
        else
        {// Next weapon
            player.CurrentWeaponIndex++;
        }
    }

    void OnReloadInput(InputAction.CallbackContext _)
    {
        if (!player.IsReloading)
        {
            player.Reload();
        }
    }

    void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (player.BulletInMag > 0)
            {
                autoFire = player.Fire();
                StartCoroutine(autoFire);
                player.WeaponPoc.PlayFireAnimation();
            }
            else
            {
                player.Reload();
            }
        }
        else if (context.canceled)
        {
            StopCoroutine(autoFire);
            player.WeaponPoc.PlayIdleAnimation();
        }
    }

    private void OnBlankUse(InputAction.CallbackContext _)
    {
        if (player.Inven_Item.Slots[(int)ItemType.BlankShell].StackCount > 0)
        {
            player.Inven_Item.Slots[(int)ItemType.BlankShell].StackCount--;
            blankFX.PlayBlankFX();
            SoundManager.Inst.PlaySound_Player(Clips_Player.blank);
            BulletManager.Inst.ReturnAllBullets();
        }
    }

    private void OnInteraction(InputAction.CallbackContext _)
    {
        if(Physics2D.OverlapCircleNonAlloc(transform.position,
            interactRange, lootColliders, LayerMask.GetMask("Items")) > 0)
        {
            //가장 가까이 있는 아이템 찾기 
            float closest = float.MaxValue;
            foreach(Collider2D item in lootColliders)
            {
                if (item != null)
                {
                    float temp = (item.transform.position - transform.position).sqrMagnitude;
                    if (temp < closest)
                    {
                        closest = temp;
                        lootColliders[0] = item;
                    }
                }
            }

            // Lootable 이면 루팅
            if(lootColliders[0].TryGetComponent<ILootable>(out var lootable))
            {
                lootable.LootAction();
            }
            Array.Clear(lootColliders, 0, lootColliders.Length); // 결과 초기화 
        }
    }

    private void OnWeaponDrop(InputAction.CallbackContext context)
    {
        int fullCount = 0;
        for (int i = 0; i < player.Inven.slotCount; i++)
        {
            if (!player.Inven.Slots[i].IsEmpty())
            {
                fullCount++;
            }
        }

        if (fullCount > 1)
        {// Have at least one weapon
            GameObject obj = Instantiate(player.CurrentWeapon.weaponPrefab);
            obj.transform.position = transform.position + new Vector3(0.5f, 0 , 0);
            player.Inven.RemoveItem((uint)player.CurrentWeaponIndex);
            obj.gameObject.tag = "Weapons";
            if (player.CurrentWeaponIndex == 0)
            {
                player.CurrentWeaponIndex++;
                return;
            }
            player.CurrentWeaponIndex--;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.forward, interactRange);
    }
#endif
}
