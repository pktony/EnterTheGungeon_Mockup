using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    PlayerInput input = null;
    Animator anim = null;
    Weapons weapon = null;

    PlayerMove moveMode = PlayerMove.IDLE;

    //################ Move ##############################
    [SerializeField] private float moveSpeed = 3.0f;
    Vector3 inputDir = Vector3.zero;

    //################ Look ##############################
    Vector2 lookDir = Vector2.zero;

    //################ Weapon ##############################
    private SpriteRenderer weaponSprite = null;
    private uint weaponNum = 0;

    //################ Dodge ##############################
    [SerializeField] private float dodgeDistance = 3.0f;
    private float dodgeDuration = 0.5f;
    Vector2 dodgeDir = Vector2.zero;

    private void Awake()
    {
        input = new();
        anim = GetComponent<Animator>();
        weapon = FindObjectOfType<Weapons>();
        weaponSprite = weapon.GetComponentInChildren<SpriteRenderer>();
        //weapon_Anim = weapon.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Look.performed += OnLookInput;
        input.Player.Move.performed += OnMoveInput;
        input.Player.Dodge.performed += OnDodgeInput;

        input.Player.ChangeWeapons.performed += OnChangeWeapon;
        input.Player.Shoot.performed += OnShoot;
    }

    private void OnDisable()
    {
        input.Player.Look.performed -= OnLookInput;
        input.Player.Move.performed -= OnMoveInput;
        input.Player.Dodge.performed -= OnDodgeInput;
        input.Player.Disable();
    }

    private void Update()
    {
        if (inputDir.sqrMagnitude > 0)
        {
            //float speed = 0.0f;

            if (moveMode == PlayerMove.DODGE)
            {
                Dodge();
                return;
            }
            anim.SetFloat("Speed", 1.0f);
            Move(moveSpeed);        // Leave parameter in case of RUN state
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }

        RotateWeapon();     // Only when weapon is equipped
    }

    void Move(float speed)
    {
        // #################### Move ########################################
        transform.position += Time.deltaTime * speed * inputDir;
    }

    void Dodge()
    {
        dodgeDuration -= Time.deltaTime;
        transform.Translate(dodgeDistance * Time.deltaTime * dodgeDir);

        if (dodgeDuration < 0f)
        {
            moveMode = PlayerMove.WALK;
            dodgeDuration = 0.5f;
        }
    }

    void RotateWeapon()
    {
        weapon.transform.right = lookDir;

        if (lookDir.x < 0)
        {
            weaponSprite.flipY = true;
        }
        else
        {
            weaponSprite.flipY = false;
        }
    }

    void OnLookInput(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>();

        lookDir = (Camera.main.ScreenToWorldPoint(mouseInput) - transform.position).normalized;

        anim.SetFloat("Mouse_X", lookDir.x);
        anim.SetFloat("Mouse_Y", lookDir.y);
    }
    void OnMoveInput(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector3>().normalized;
    }

    void OnDodgeInput(InputAction.CallbackContext _)
    {
        anim.SetTrigger("OnDodge");
        dodgeDir = lookDir;
        moveMode = PlayerMove.DODGE;
    }

    void OnChangeWeapon(InputAction.CallbackContext number)
    {
        weaponNum = (uint)number.ReadValue<int>();
    }

    void OnShoot(InputAction.CallbackContext _)
    {
        weapon.Fire();
    }
}
