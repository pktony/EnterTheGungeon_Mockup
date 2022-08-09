using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FXData
{
    public uint ID = 0;
    public string FxName = "";
    public GameObject prefab;
    public uint poolSize = 10;
}
