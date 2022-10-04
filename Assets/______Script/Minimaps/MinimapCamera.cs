using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    Transform player;

    [SerializeField] private Vector3 offset = Vector3.zero;

    private void Awake()
    {
        player = FindObjectOfType<Player>().transform;
    }

    private void LateUpdate()
    {
        this.transform.position = player.position + offset;
    }
}
