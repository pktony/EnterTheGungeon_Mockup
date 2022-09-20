using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory
{
    ItemSlot[] slots = null;

    const int ITEMSLOT_SIZE = 7;

    public ItemSlot this[int index] => slots[index];
    public ItemSlot[] Slots => slots;

    public ItemInventory()
    {
        slots = new ItemSlot[ITEMSLOT_SIZE];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
        }
    }

    public void InitializeItemInventory()
    {
        for (int i = 0; i < ITEMSLOT_SIZE; i++)
        {
            slots[i].AssignItem(GameManager.Inst.ItemDataManager[(uint)i]);
            //blank shell, key, Heart, ammo box, Gold Shell
        }
    }

    public bool AddItem(ItemData data)
    {
        bool result = false;

        slots[data.itemID].AssignItem(data);

        result = true;
        return result;
    }

    public bool AddItem(ItemID id)
    {
        return AddItem(slots[(int)id].Data);
    }
}
