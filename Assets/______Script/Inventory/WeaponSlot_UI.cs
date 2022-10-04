using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot_UI : MonoBehaviour
{
    WeaponSlot weaponSlot;

    Image slotImg;
    Image weaponImage;
    RectTransform rect;

    public WeaponSlot Weapon_Slot => weaponSlot;
    public Image SlotImg => slotImg;
    public Image WeaponImg => weaponImage;
    public RectTransform Rect => rect;

    private void Awake()
    {
        slotImg = GetComponent<Image>();
        weaponImage = transform.GetChild(0).GetComponent<Image>();
        rect = GetComponent<RectTransform>();
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
