using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
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

    protected override void Awake()
    {
        base.Awake();
        pooledItems = new();

        blankShell = new();
        ammoBox = new();
        key = new();
        heart = new();
        bronzeShell = new();
        silverShell = new();
        goldShell = new();

        pooledItems.Add((uint)ItemType.BlankShell, blankShell);
        pooledItems.Add((uint)ItemType.AmmoBox, ammoBox);
        pooledItems.Add((uint)ItemType.Key, key);
        pooledItems.Add((uint)ItemType.Heart, heart);
        pooledItems.Add((uint)ItemType.GoldShell, goldShell);
        pooledItems.Add((uint)ItemType.SilverShell, silverShell);
        pooledItems.Add((uint)ItemType.BronzeShell, bronzeShell);

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

    public GameObject GetPooledItem(ItemType id)
    {
        return GetPooledItem(pooledItems[(uint)id]);
    }

    public void ReturnItem(Stack<GameObject> returningStack, GameObject uselessItem)
    {
        returningStack.Push(uselessItem);
        uselessItem.transform.position = Vector3.zero;
        uselessItem.transform.rotation = Quaternion.identity;
        uselessItem.SetActive(false);
    }

    public void ReturnItem(ItemType id, GameObject uselessItem)
    {
        ReturnItem(pooledItems[(uint)id], uselessItem);
    }
}
