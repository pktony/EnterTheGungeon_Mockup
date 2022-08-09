using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory
{
    ItemSlot[] slots = null;

    const int ITEMSLOT_SIZE = 4;
    // shell, blank shell, ammo box, key

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
        slots[0].AssignItem(GameManager.Inst.ItemDataManager[ItemID.Shell]);
        slots[1].AssignItem(GameManager.Inst.ItemDataManager[1]);
        slots[2].AssignItem(GameManager.Inst.ItemDataManager[2]);
        slots[3].AssignItem(GameManager.Inst.ItemDataManager[3]);
    }
}
