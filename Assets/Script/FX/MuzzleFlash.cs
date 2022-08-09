using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    ParticleSystem muzzleFX;
    private void Awake()
    {
        muzzleFX = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        GameManager.Inst.Player.onFireReload += PlayMuzzleFX;
    }

    void PlayMuzzleFX()
    {
        muzzleFX.Play();
    }
}
