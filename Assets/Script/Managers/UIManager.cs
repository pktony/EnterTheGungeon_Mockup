using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Inst => instance;

    WeaponInventory_UI invenUI;
    public WeaponInventory_UI InvenUI => invenUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.Initialize();
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }

        }
    }

    private void Initialize()
    {
        invenUI = FindObjectOfType<WeaponInventory_UI>();
    }
}
