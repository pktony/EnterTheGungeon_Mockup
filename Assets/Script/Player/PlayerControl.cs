using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    PlayerInput input = null;
    Animator anim = null;

    PlayerMove moveMode = PlayerMove.IDLE;

    //################ Move ##############################
    [SerializeField] private float moveSpeed = 3.0f;
    Vector3 inputDir = Vector3.zero;

    //################ Look ##############################
    Vector2 lookDir = Vector2.zero;


    private void Awake()
    {
        input = new();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Look.performed += OnLookInput;
        input.Player.Move.performed += OnMoveInput;
    }

    private void OnDisable()
    {
        input.Player.Look.performed -= OnLookInput;
        input.Player.Move.performed -= OnMoveInput;
        input.Player.Disable();
    }

    private void Update()
    {
        if (inputDir.sqrMagnitude > 0)
        {
            float speed = 0.0f;

            if (moveMode == PlayerMove.WALK)
            {
                anim.SetFloat("Speed", 1.0f);
                speed = moveSpeed;
            }
            else if (moveMode == PlayerMove.RUN)
            {
                
            }
            Move();
        }
        else
        {
            anim.SetFloat("Speed", 0f);
        }
    }

    void Move()
    {
        // #################### Move ########################################
        transform.position += Time.deltaTime * moveSpeed * inputDir;

        // #################### Look ########################################
        

    }

    void OnLookInput(InputAction.CallbackContext context)
    {
        Vector2 mouseInput = context.ReadValue<Vector2>();

        lookDir = Camera.main.ScreenToWorldPoint(mouseInput);

        lookDir.x = Mathf.Clamp(lookDir.x, -1f, 1f);
        lookDir.y = Mathf.Clamp(lookDir.y, -1f, 1f);

        anim.SetFloat("Mouse_X", lookDir.x);
        anim.SetFloat("Mouse_Y", lookDir.y);
    }
    void OnMoveInput(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector3>().normalized;
        moveMode = PlayerMove.WALK;
    }
}
