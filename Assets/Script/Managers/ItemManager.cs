using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // ##################### Singleton #########################
    private static ItemManager instance = null;
    public static ItemManager Inst { get => instance; }

    // #################### Dictionary #########################
    private static Dictionary<uint, Stack<GameObject>> pooledItems;
    public static Dictionary<uint, Stack<GameObject>> PooledItems => pooledItems;

    //##################### Item Stack #######################
    [SerializeField] private ItemData[] poolingItems;


    private Stack<GameObject> blankShell;
    private Stack<GameObject> ammoBox;
    private Stack<GameObject> key;
    private Stack<GameObject> heart;
    private Stack<GameObject> bronzeShell;
    private Stack<GameObject> silverShell;
    private Stack<GameObject> goldShell;

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
        pooledItems = new();

        blankShell = new();
        ammoBox = new();
        key = new();
        heart = new();
        bronzeShell = new();
        silverShell = new();
        goldShell = new();

        pooledItems.Add((uint)ItemID.BlankShell, blankShell);
        pooledItems.Add((uint)ItemID.AmmoBox, ammoBox);
        pooledItems.Add((uint)ItemID.Key, key);
        pooledItems.Add((uint)ItemID.Heart, heart);
        pooledItems.Add((uint)ItemID.GoldShell, goldShell);
        pooledItems.Add((uint)ItemID.SilverShell, silverShell);
        pooledItems.Add((uint)ItemID.BronzeShell, bronzeShell);

        for (uint i = 0; i < poolingItems.Length; i++)
        {
            for (uint j = 0; j < poolingItems[i].poolingSize; j++)
            {
                GameObject obj = Instantiate(poolingItems[i].prefab, this.transform);
                pooledItems[i].Push(obj);
                obj.SetActive(false);   
            }
        }

    }

    public GameObject GetPooledItem(Stack<GameObject> poolingObject)
    {
        if (poolingObject.Count > 0)
        {
            GameObject obj = poolingObject.Pop();
            return obj;
        }
        return null;
    }

    public void ReturnItem(Stack<GameObject> returningStack, GameObject uselessBullet)
    {
        returningStack.Push(uselessBullet);
        uselessBullet.SetActive(false);
    }
}