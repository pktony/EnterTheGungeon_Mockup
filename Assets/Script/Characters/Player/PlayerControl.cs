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
    WeaponPocket weaponPocket;
    Player player = null;
    BlankFX blankFX;

    [SerializeField] PlayerMove moveMode = PlayerMove.IDLE;

    //################ Move ##############################
    [SerializeField] private float moveSpeed = 3.0f;
    Vector3 inputDir = Vector3.zero;

    //################ Look ##############################
    Vector2 lookDir = Vector2.zero;

    //################ Weapon ##############################
    private SpriteRenderer weaponSprite = null;
    [SerializeField] private Vector2 weaponRight;

    //################ Fire ##############################
    Vector2 fireDirection = Vector2.zero;
    IEnumerator autoFire;

    //################ Dodge #############################
    //################ Loot #############################
    [SerializeField] private float interactRange = 2.0f;
    //################ Camera #############################
    private Vector3 camPosition = Vector3.zero;
    
    // ################################## Properties ###################################
    public Vector3 CamPosition => camPosition;
    public Vector2 FireDirection
    {
        get => fireDirection;
        set
        {
            fireDirection = value;
        }
    }

    public Vector2 LookDir => lookDir;


    // ############################# Delegates ###########################
    public System.Action onLoot;

    private void Awake()
    {
        input = new();
        anim = GetComponent<Animator>();

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
        input.Player.Dodge.performed -= OnDodgeInput;
        input.Player.Reload.performed -= OnReloadInput;
        input.Player.Shoot.performed -= OnShoot;
        input.Player.Shoot.canceled -= OnShoot;
        input.Player.Blank.performed -= OnBlankUse;
        input.Player.Interact.performed -= OnInteraction;
        input.Player.DropWeapon.performed -= OnWeaponDrop;
        input.Player.Disable();
    }

    private void Update()
    {
        if (inputDir.sqrMagnitude > 0)
        {
            if (moveMode == PlayerMove.DODGE)
            {
                player.Dodge();
                if (player.canDodge)
                {
                    moveMode = PlayerMove.WALK;
                }
                return;
            }
            moveMode = PlayerMove.WALK;
            anim.SetFloat("Speed", 1.0f);
            Move(moveSpeed);        // Leave parameter in case of RUN state
        }
        else
        {
            moveMode = PlayerMove.IDLE;
            anim.SetFloat("Speed", 0f);
        }

        RotateWeapon();
    }

    void Move(float speed)
    {
        // #################### Move ########################################
        transform.position += Time.deltaTime * speed * inputDir;
    }

    void RotateWeapon()
    {
        weaponPocket.transform.right = lookDir;

        if (lookDir.x < 0)
        {// Left
            weaponSprite.flipY = true;
            MirrorWeaponPosition(-weaponRight);
        }
        else
        {// Right
            weaponSprite.flipY = false;
            MirrorWeaponPosition(weaponRight);
        }
    }

    void MirrorWeaponPosition(Vector3 pos)
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
        inputDir = context.ReadValue<Vector3>().normalized;

        anim.SetFloat("Keyboard_X", inputDir.x);
        anim.SetFloat("Keyboard_Y", inputDir.y);
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
            if (player.BulletinMag > 0)
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
        if (player.Inven_Item.Slots[(int)ItemID.BlankShell].StackCount > 0)
        {
            player.Inven_Item.Slots[(int)ItemID.BlankShell].StackCount--;
            blankFX.PlayBlankFX();

            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullets");
            if( bullets != null)
            {
                foreach (GameObject bullet in bullets)
                {
                    IDestroyable destroyable = bullet.GetComponent<IDestroyable>();
                    if(destroyable != null)
                    {
                        destroyable.BlankDestroy();
                    }
                }
            }
        }
    }

    private IEnumerator BlankFX()
    {
        GameObject fx = FXManager.Inst.GetFX(FXManager.Inst.PooledFx[(int)FxID.BLANKFX]);
        fx.transform.position = this.transform.position;
        fx.SetActive(true);
        float animTime = fx.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length - 0.2f;
        yield return new WaitForSeconds(animTime);
        FXManager.Inst.ReturnFX(FXManager.Inst.PooledFx[(int)FxID.BLANKFX], fx);
    }

    private void OnInteraction(InputAction.CallbackContext _)
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, interactRange, LayerMask.GetMask("Items"));

        if (items != null)
        {
            float closest = float.MaxValue;
            foreach(Collider2D item in items)
            { 
                float temp = (item.transform.position - transform.position).sqrMagnitude;
                if (temp < closest)
                {
                    closest = temp;
                    items[0] = item;
                }
            }

            ILootable lootable = items[0].GetComponent<ILootable>();
            if(lootable != null)
            {
                lootable.LootAction();
            }
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
