using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    PlayerInput input = null;
    Animator anim = null;
    Weapons weapon = null;
    Player player = null;

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
        weapon = FindObjectOfType<Weapons>();
        weaponSprite = weapon.GetComponentInChildren<SpriteRenderer>();

        player = GetComponent<Player>();
        
        //weapon_Anim = weapon.GetComponent<Animator>();

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

        RotateWeapon();     // Only when weapon is equipped
    }

    void Move(float speed)
    {
        // #################### Move ########################################
        transform.position += Time.deltaTime * speed * inputDir;
    }

    void RotateWeapon()
    {
        weapon.transform.right = lookDir;

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
        weapon.transform.localPosition = pos;
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
            if (player.Inven.Slots[player.CurrentWeaponIndex].WeaponSlotData == null)
            {
                player.CurrentWeaponIndex++;
            }
        }
        else
        {// Next weapon
            player.CurrentWeaponIndex++;
            if (player.Inven.Slots[player.CurrentWeaponIndex].WeaponSlotData == null)
            {
                player.CurrentWeaponIndex--;
            }
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
            }
            else
            {
                player.Reload();
            }
        }
        else if (context.canceled)
        {
            StopCoroutine(autoFire);
        }
    }

    private void OnBlankUse(InputAction.CallbackContext _)
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullets");
        foreach (GameObject bullet in bullets)
        {
            BulletManager.Inst.ReturnBullet(BulletManager.PooledBullets[(int)BulletID.ENEMY], bullet);
        }
    }

    private void OnInteraction(InputAction.CallbackContext _)
    {
        Collider2D obj = Physics2D.OverlapCircle(transform.position, interactRange, LayerMask.GetMask("DroppedWeapons"));
        if (obj != null)
        {
            int sameCount = 0;
            Weapon newWeapon = obj.GetComponent<Weapon>();
            for (int i = 0; i < player.Inven.slotCount; i++)
            {// Check if Weapon == in slot weapons
                if (player.Inven.Slots[i].WeaponSlotData == newWeapon.weaponData)
                {
                    sameCount++;
                }
            }

            if (sameCount < 1)
            {
                player.Inven.AddItem(newWeapon.weaponData);
                Destroy(obj);
                int index = player.CurrentWeaponIndex;
                player.CurrentWeaponIndex++;
                if (index == player.CurrentWeaponIndex)
                {
                    player.CurrentWeaponIndex--;
                }
            }
        }
        else
        {
            Collider2D items = Physics2D.OverlapCircle(transform.position, interactRange, LayerMask.GetMask("Items"));
            if (items != null)
            {
                if (items.CompareTag("BlankShell"))
                {
                    player.Inven_Item.Slots[(int)ItemID.BlankShell].IncreaseItem();
                }
                else if (items.CompareTag("Ammo"))
                {
                    player.Inven_Item.Slots[(int)ItemID.AmmoBox].IncreaseItem();
                }
                else if (items.CompareTag("Key"))
                {
                    player.Inven_Item.Slots[(int)ItemID.Key].IncreaseItem();
                }
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
            if (player.CurrentWeaponIndex == 0)
            {
                player.CurrentWeaponIndex++;
                return;
            }
            player.CurrentWeaponIndex--;
        }
    }


}
