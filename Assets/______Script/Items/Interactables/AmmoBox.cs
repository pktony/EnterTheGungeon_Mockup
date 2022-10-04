using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : Item
{
    public override void LootAction()
    {
        player.CurrentWeapon.remainingBullet = player.CurrentWeapon.maxBulletNum;
        player.W_InvenUI.BulletUI.RefreshBullet_UI();
        GameManager.Inst.SoundManager.PlaySound_Item(Clips_Item.AmmoPickup);
        ItemManager.Inst.ReturnItem(ItemID.AmmoBox, this.gameObject);
    }
}
