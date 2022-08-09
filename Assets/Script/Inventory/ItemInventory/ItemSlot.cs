using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    ItemData data;

    uint stackCount = 0;

    public void AssignItem(ItemData newdata)
    {
        data = newdata;
    }

    public uint IncreaseItem()
    {
        return stackCount++; 
    }

    public uint DecreaseItem(uint number)
    {
        return stackCount -= number;
    }
}
