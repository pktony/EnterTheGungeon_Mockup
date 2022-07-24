using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Scriptable Object/Weapon Data", order = 1)]
public class WeaponData : ScriptableObject
{
    public uint id = 0;
    public string weaponName = "Weapon 1";
    public Sprite weaponIcon;
    public GameObject weaponPrefab;
    public int bulletNum;
}
