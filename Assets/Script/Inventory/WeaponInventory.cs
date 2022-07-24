using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory
{
    WeaponSlot[] weaponSlots = null;

    // ############################ Variables ##########################


    // ############################ Constants ##########################
    const int WEAPON_SLOT_SIZE = 3;

    //############################# Properties #########################
    public WeaponSlot this[int index] { get => weaponSlots[index]; }

    //############################# Methods ############################
    public WeaponInventory(int size = WEAPON_SLOT_SIZE)
    {
        weaponSlots = new WeaponSlot[size];
        for (int i = 0; i < size; i++)
        {
            weaponSlots[i] = new WeaponSlot();
        }
    }

    /// <summary>
    /// Add Weapon to Weapon Inventory
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True : Can add,  False : Inventory Full</returns>
    
    public bool AddItem(WeaponData data)
    {
        bool result = false;
        WeaponSlot slot = FindEmptySlot();
        if (slot != null)
        {
            slot.AssignWeapon(data);
            result = true;
        }
        else
        {
            Debug.Log($"Failed to add {data.name}");
        }

        return result;
    }
    
    public bool AddItem(uint id)
    {
        return AddItem(GameManager.Inst.Weapon_Data[id]);
    }

    public bool AddItem(WeaponType code)
    {
        return AddItem(GameManager.Inst.Weapon_Data[code]);
    }

    // ------- Remove
    bool RemoveItem(uint slotIndex)
    {
        bool result = false;

        if (IsValidIndex(slotIndex))
        {
            WeaponSlot slot = weaponSlots[slotIndex];
            slot.ClearWeapon();
            result = true;
        }
        else
        {
            Debug.Log("Failed to Remove Weapon");
        }
        return result;
    }



    //######################### Method(Backend) ###########################
    WeaponSlot FindEmptySlot()
    {
        WeaponSlot result = null;
        foreach (WeaponSlot slot in weaponSlots)
        {
            if (slot.IsEmpty())
            {
                result = slot;
                break;
            }
        }
        return result;
    }

    bool IsValidIndex(uint index) { return index < weaponSlots.Length; }
}
