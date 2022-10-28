using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Item
{
    public override void LootAction()
    {
        player.Inven_Item.Slots[(int)ItemType.Key].IncreaseItem();
        SoundManager.Inst.PlaySound_Item(Clips_Item.Key_Pickup);
        ItemManager.Inst.ReturnItem(ItemType.Key, this.gameObject);
    }
}
