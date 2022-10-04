using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataManager : MonoBehaviour
{
    public WeaponData[] weaponDatas;    // Add weapon Data in inspector window

    public WeaponData this[uint i]
    {
        get => weaponDatas[i];
    }

    public WeaponData this[WeaponType code]
    {
        get => weaponDatas[(int)code];
    }

    public int Length
    {
        get => weaponDatas.Length;
    }
}
