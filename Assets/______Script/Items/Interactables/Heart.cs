using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Item
{
    public override void LootAction()
    {
        player.HP++;
        GameManager.Inst.SoundManager.PlaySound_Item(Clips_Item.Heart);
        ItemManager.Inst.ReturnItem(ItemID.Heart, this.gameObject);
    }
}