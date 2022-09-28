using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GameManager.Inst.Player;
    }

    private void LateUpdate()
    {
        Vector3 mouseRead = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mouseRead.z = -10f;

        transform.position = (mouseRead * 0.25f + player.transform.position * 0.75f);

        //transform.rotation *= Quaternion.Euler(shake);
    }

    Vector3 CameraShake()
    {
        Vector3 shakeVector = Vector3.zero;



        return shakeVector;
    }
}
