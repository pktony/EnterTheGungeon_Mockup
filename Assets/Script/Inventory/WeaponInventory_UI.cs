using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponInventory_UI : MonoBehaviour
{
    // ################### VARIABLES #######################
    // weapon 정보
    WeaponInventory weaponInven;

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

        // weapon 개수에 따른 초기화
        if (WeaponInventory.WEAPON_SLOT_SIZE != newInven.slotCount)
        {
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
            }
        }
        RefreshAllSlots();
        GameManager.Inst.Player.InitializeCurrentWeapon(0);

        bulletUI = FindObjectOfType<Bullet_UI>();
        bulletUI.RefreshBullet_UI();
    }

    void RefreshAllSlots()
    {
        foreach (var slotUI in slotUIs)
        {
            slotUI.Refresh_Image();
        }
    }
}
