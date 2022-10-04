using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInventory_UI : MonoBehaviour
{
    // ################### VARIABLES #######################
    // weapon ����
    WeaponInventory weaponInven;
    public WeaponInventory WeaponInven => weaponInven;

    public GameObject SlotPrefab;
    WeaponSlot_UI[] slotUIs;

    Bullet_UI bulletUI;
    //################### Properties #######################
    public WeaponSlot_UI[] SlotUIs { get => slotUIs; }
    public Bullet_UI BulletUI { get => bulletUI; }

    //################# Bullet UI ##########################
    TextMeshProUGUI bulletText;

    public void InitializeInventory(WeaponInventory newInven)
    {
        weaponInven = newInven;

        slotUIs = new WeaponSlot_UI[weaponInven.slotCount];

        if (WeaponInventory.DEFAULT_WEAPON_SLOT_SIZE != newInven.slotCount)
        {// Weapon initialization
            for (int i = 0; i < weaponInven.slotCount; i++)
            {
                GameObject obj = Instantiate(SlotPrefab, this.transform);
                obj.name = $"{SlotPrefab.name}_{i}";
                slotUIs[i] = obj.GetComponent<WeaponSlot_UI>();
                slotUIs[i].Initialize(weaponInven[i]);
            }
        }
        else
        {
            slotUIs = transform.parent.GetComponentsInChildren<WeaponSlot_UI>();
            for (int i = 0; i < weaponInven.slotCount; i++)
            {
                slotUIs[i].Initialize(weaponInven[i]);
                slotUIs[i].Weapon_Slot.onSlotWeaponChange += ShowCurrentWeapon;
            }
        }
        RefreshAllSlots();
        GameManager.Inst.Player.InitializeCurrentWeapon(0);
        GameManager.Inst.Player.onWeaponChange = ShowCurrentWeapon;

        bulletUI = FindObjectOfType<Bullet_UI>();
        bulletUI.InitializeBullet_UI();
        bulletUI.RefreshBullet_UI();

        ShowCurrentWeapon();
    }

    void RefreshAllSlots()
    {
        foreach (var slotUI in slotUIs)
        {
            slotUI.Refresh_Image();
        }
    }

    public void ShowCurrentWeapon()
    {
        foreach (WeaponSlot_UI slotUI in SlotUIs)
        {
            slotUI.SlotImg.color = Color.clear;
            slotUI.WeaponImg.color = Color.clear;
        }
        slotUIs[GameManager.Inst.Player.CurrentWeaponIndex].SlotImg.color = Color.white;
        slotUIs[GameManager.Inst.Player.CurrentWeaponIndex].WeaponImg.color = Color.white;
        slotUIs[GameManager.Inst.Player.CurrentWeaponIndex].Rect.anchoredPosition = new Vector2(-50f, 20f);
    }
}
