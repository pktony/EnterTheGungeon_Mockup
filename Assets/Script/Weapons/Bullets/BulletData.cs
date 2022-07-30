using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet", menuName = "Scriptable Object/Bullet Data", order = 2 )]
public class BulletData : ScriptableObject
{
    public uint bulletId = 0;
    public string bulletName = "Bullet Name";
    public GameObject prefab;
    public int bulletSize;
}
