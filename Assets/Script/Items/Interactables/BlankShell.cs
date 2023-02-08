using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankShell : Item
{
    public override void LootAction()
    {
        if(player.Inven_Item.Slots[(int)ItemType.BlankShell].StackCount < 3)
            player.Inven_Item.Slots[(int)ItemType.BlankShell].IncreaseItem();
        //GameManager.Inst.SoundManager.PlaySound_Item(cli)

        ItemManager.Inst.ReturnItem(ItemType.BlankShell, this.gameObject);
    }
}
