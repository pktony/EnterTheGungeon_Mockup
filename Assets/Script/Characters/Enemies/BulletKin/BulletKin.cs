using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class BulletKin : Enemy, IHealth
{
    protected override void Awake()
    {
        base.Awake();

        firePosition = new Transform[1];
        firePosition[0] = weapon.transform.GetChild(0).GetChild(0);
    }

    // ----------------- Die
    protected override IEnumerator Die()
    {
        //rigid.AddForce(-trackDirection * 100f, ForceMode2D.Impulse);
        anim.SetTrigger("onDie");
        float randDie = UnityEngine.Random.value;
        anim.SetFloat("RandDie", randDie);
        weaponSprite.color = Color.clear;
        yield return new WaitForSeconds(3.0f);

        uint rand = (uint)UnityEngine.Random.Range(4, 6); // 4: Gold, 5 : silver, 6 : Bronze
        GameObject shell = ItemManager.Inst.GetPooledItem((ItemID)rand);
        shell.transform.position = this.transform.position;
        shell.gameObject.SetActive(true);
        EnemyManager.Inst.ReturnEnemy(EnemyID.BULLETKIN, this.gameObject);
    }
}
