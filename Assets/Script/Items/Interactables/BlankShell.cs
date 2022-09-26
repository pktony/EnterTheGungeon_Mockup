using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankShell : Item
{
    public override void LootAction()
    {
        player.Inven_Item.Slots[(int)ItemID.BlankShell].IncreaseItem();
        ItemManager.Inst.ReturnItem(ItemID.BlankShell, this.gameObject);
    }
}
