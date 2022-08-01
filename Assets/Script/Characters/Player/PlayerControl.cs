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

    PlayerMove moveMode = PlayerMove.IDLE;

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
    public Vector2 FireDirection { get => fireDirection; }

    //################ Dodge #############################
    //################ Camera #############################
    private Vector3 camPosition = Vector3.zero;
    
    // ################################## Properties ###################################
    public Vector3 CamPosition { get => camPosition; }

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
    }

    private void OnDisable()
    {
        input.Player.Look.performed -= OnLookInput;
        input.Player.Move.performed -= OnMoveInput;
        input.Player.Dodge.performed -= OnDodgeInput;
        input.Player.Reload.performed -= OnReloadInput;
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
        anim.SetTrigger("OnDodge");

        if(moveMode == PlayerMove.WALK)
        { // Dodge only when moving
            player.dodgeDir = inputDir;
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
    void OnShoot(InputAction.CallbackContext _)
    {
        fireDirection = (lookDir + Random.insideUnitCircle * 0.1f).normalized;
        player.Fire();
    }
}
