using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot
{
    WeaponData weaponData;

    //########################## Variables ##############################
    

    //########################## Properties #############################
    public WeaponData WeaponSlotData { get => weaponData; 
        set
        { 
            weaponData = value;
            onSlotWeaponChange?.Invoke();
        }
    }

    //########################## Deligates ##############################
    public System.Action onSlotWeaponChange;

    //########################## Methods ################################
    public void AssignWeapon(WeaponData weaponData)
    {
        WeaponSlotData = weaponData;
    }

    public void ClearWeapon() { WeaponSlotData = null; }

    public bool IsEmpty() { return weaponData == null; }
}
