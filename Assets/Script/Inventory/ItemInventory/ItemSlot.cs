using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    ItemData data;

    uint stackCount = 0;

    public System.Action onItemLoot;

    public uint StackCount
    {
        get => stackCount;
        set
        {
            stackCount = value;
            onItemLoot?.Invoke();  //item Inven RefreshUIs
        }
    }

    public void AssignItem(ItemData newdata)
    {
        data = newdata;
    }

    public uint IncreaseItem(uint num = 1) => StackCount += num;
    
    public uint DecreaseItem(uint number) => StackCount -= number;
}
