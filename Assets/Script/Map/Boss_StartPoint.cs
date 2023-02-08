using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_StartPoint : MonoBehaviour
{
    private void Start()
    {
        GameManager.Inst.Player.transform.position = this.transform.position;
    }
}
