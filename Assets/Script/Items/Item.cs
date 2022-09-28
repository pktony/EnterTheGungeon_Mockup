using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, ILootable
{
    protected Player player;

    private void Start()
    {
        //player = GameManager.Inst.Player;
    }

    public virtual void LootAction()
    {
    }
}
