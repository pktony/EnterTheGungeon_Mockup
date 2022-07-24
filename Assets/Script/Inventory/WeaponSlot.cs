using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot
{
    WeaponData weaponData;

    //########################## Variables ##############################
    

    //########################## Properties #############################
    WeaponData WeaponSlotData { get => weaponData; set { weaponData = value; } }


    //########################## Methods ################################
    public void AssignWeapon(WeaponData weaponData)
    {
        WeaponSlotData = weaponData;
    }

    public void ClearWeapon() { WeaponSlotData = null; }

    public bool IsEmpty() { return weaponData == null; }

}
