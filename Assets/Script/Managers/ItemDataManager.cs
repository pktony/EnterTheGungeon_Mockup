using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public ItemData[] itemDatas;

    public ItemData this[uint i]
    {
        get => itemDatas[i];
    }

    public ItemData this[ItemType id]
    {
        get=> itemDatas[(int)id];
    }
}
