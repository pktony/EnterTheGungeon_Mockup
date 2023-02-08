using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public WeaponData weaponData;

    public override void LootAction()
    {
        int sameCount = 0;
        for (int i = 0; i < player.Inven.slotCount; i++)
        {// Check if Weapon == in slot weapons
            if (player.Inven.Slots[i].WeaponSlotData == weaponData)
            {
                sameCount++;
            }
        }

        if (sameCount < 1)
        {
            player.Inven.AddItem(weaponData);
            this.gameObject.tag = "Player";
            Destroy(this.gameObject);
            int index = player.CurrentWeaponIndex;
            player.CurrentWeaponIndex++;
            if (index == player.CurrentWeaponIndex)
            {
                player.CurrentWeaponIndex--;
            }
            //GameManager.Inst.SoundManager.PlaySound_Item()
        }
    }
}
