using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{
    private void LateUpdate()
    {
        this.transform.position = Mouse.current.position.ReadValue();
    }
}
