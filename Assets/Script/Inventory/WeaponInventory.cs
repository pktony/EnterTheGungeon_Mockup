using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory
{
    // ############################ Variables ##########################
    WeaponSlot[] weaponSlots = null;

    // ############################ Constants ##########################
    public const int DEFAULT_WEAPON_SLOT_SIZE = 2;

    //############################# Properties #########################
    public WeaponSlot this[int index] { get => weaponSlots[index]; }

    public int slotCount => weaponSlots.Length;
    public WeaponSlot[] Slots => weaponSlots;

    //############################# Methods ############################
    public WeaponInventory(uint size = DEFAULT_WEAPON_SLOT_SIZE)
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
    public bool RemoveItem(uint slotIndex)
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
    public WeaponSlot FindEmptySlot()
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

    private bool IsValidIndex(uint index) { return index < weaponSlots.Length; }
}
