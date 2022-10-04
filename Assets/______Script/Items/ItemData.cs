using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Object/Item Data", order = 2)]
public class ItemData : ScriptableObject
{
    public uint itemID;
    public string itemName = "Item";
    public GameObject prefab;

    public uint value = 0;

    public int poolingSize;
}
