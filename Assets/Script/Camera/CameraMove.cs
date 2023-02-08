using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    Player player;

    Camera mainCam;
    Mouse currentMouse;

    /// <summary>
    /// 가중치를 이용한 카메라와 플레이어 간 카메라 중심 설정 
    /// </summary>
    [SerializeField] private float camWeightFactor = 0.75f;

    private void Start()
    {
        player = GameManager.Inst.Player;
        mainCam = Camera.main;
        currentMouse = Mouse.current;
    }

    private void LateUpdate()
    {
        Vector3 mouseRead = mainCam.ScreenToWorldPoint(currentMouse.position.ReadValue());
        mouseRead.z = -10f;

        transform.position = (mouseRead * (1 - camWeightFactor)
            + player.transform.position * camWeightFactor);
    }
}
