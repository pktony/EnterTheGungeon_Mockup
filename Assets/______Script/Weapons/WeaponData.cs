using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Scriptable Object/Weapon Data", order = 1)]
public class WeaponData : ScriptableObject
{
    public uint id = 0;
    public string weaponName = "Weapon 1";
    public Sprite weaponIcon;
    public GameObject weaponPrefab; //아이템 드랍, 루팅 용 
    public RuntimeAnimatorController weaponAniamtor;

    public Sprite BulletIcon;
    public int maxBulletNum;
    public int maxBulletMagazine;
    public int remainingBullet;
    public int bulletPerFire = 1;

    public float reloadingTime = 1.0f;
    public float fireRate = 1.0f;
    public float dispersion = 0f;
    public AudioClip fireSound;
    public float fireVolume = 1.0f;

    public ParticleSystem muzzleFX;
}
