using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot_UI : MonoBehaviour
{
    WeaponSlot weaponSlot;

    Image weaponImage;

    public WeaponSlot Weapon_Slot => weaponSlot;

    private void Awake()
    {
        weaponImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void Initialize( WeaponSlot slot)
    {
        weaponSlot = slot;
        weaponSlot.onSlotWeaponChange = Refresh_Image;
    }

    public void Refresh_Image()
    {
        if (weaponSlot.WeaponSlotData != null)
        {
            weaponImage.sprite = weaponSlot.WeaponSlotData.weaponIcon;
            weaponImage.color = Color.white;
        }
    }
}
