using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Inst { get => instance; }

    private Player player = null;
    public Player Player { get => player; }
    
    private PlayerControl control = null;
    public PlayerControl Control { get => control; }

    private WeaponDataManager weaponData;
    public WeaponDataManager Weapon_Data { get => weaponData; }

    private ItemDataManager itemDataManager;
    public ItemDataManager ItemDataManager => itemDataManager;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.Initialize();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void Initialize()
    {
        player = FindObjectOfType<Player>();
        control = player.GetComponent<PlayerControl>();
        weaponData = GetComponent<WeaponDataManager>();
        itemDataManager = GetComponent<ItemDataManager>();
    }
}
