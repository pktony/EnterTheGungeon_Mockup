using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Test : MonoBehaviour
{
    WeaponInventory inven;

    private void Awake()
    {
        inven = new WeaponInventory();

        inven.AddItem(WeaponType.PISTOL);
    }
}
