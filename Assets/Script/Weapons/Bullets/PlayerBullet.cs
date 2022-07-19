using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 5.0f;
    Rigidbody2D rigid = null;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

        rigid.isKinematic = false;
    }

    private void OnEnable()
    {
        
    }
}
