using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    public override void LootAction()
    {
        player.Inven_Item.Slots[(int)ItemID.Key].IncreaseItem();
        ItemManager.Inst.ReturnItem(ItemID.Key, this.gameObject);
    }
}
