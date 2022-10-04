using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    public override void LootAction()
    {
        player.Inven_Item.Slots[(int)ItemID.Key].IncreaseItem();
        GameManager.Inst.SoundManager.PlaySound_Item(Clips_Item.Key_Pickup);
        ItemManager.Inst.ReturnItem(ItemID.Key, this.gameObject);
    }
}
