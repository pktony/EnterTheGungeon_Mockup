using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item
{
    public override void LootAction()
    {
        player.HP++;
        ItemManager.Inst.ReturnItem(ItemID.Heart, this.gameObject);
    }
}
