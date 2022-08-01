using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Test : MonoBehaviour
{
    WeaponInventory inven;
    WeaponInventory_UI inven_UI;

    private void Awake()
    {
        inven_UI = FindObjectOfType<WeaponInventory_UI>();

        inven = new WeaponInventory();

        inven.AddItem(WeaponType.PISTOL);
        inven.AddItem(WeaponType.RIFLE);
        inven_UI.InitializeInventory(inven);

        //inven.PrintInventory();
    }
}
