using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMove : MonoBehaviour
{
    Player player;
    bool isMoving = false;

    private void Start()
    {
        player = GameManager.Inst.Player;
    }

    private void LateUpdate()
    {
        if (!isMoving)
        {
            Vector3 mouseRead = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mouseRead.z = -10f;

            transform.position = (mouseRead * 0.25f + player.transform.position * 0.75f);
        }
    }

    //public IEnumerator MoveCam(Vector2 originalPosition, Vector2 destination, float speed)
    //{
    //    isMoving = true;
    //    while ((destination - originalPosition).sqrMagnitude > 0.1f)
    //    {
    //        transform.position = Vector3.Lerp((Vector2)transform.position, destination, speed * Time.deltaTime);
    //        yield return null;
    //    }
    //    isMoving = false;
    //}
}
