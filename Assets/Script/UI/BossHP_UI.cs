using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHP_UI : MonoBehaviour
{
    Slider slider;
    public GameObject hpChangeImage;

    BulletKing king;
    WaitForSeconds waitSeconds;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();

        king = GetComponentInParent<BulletKing>();
        king.OnTakeDamage += RefreshUI;

        waitSeconds = new WaitForSeconds(0.15f);
    }

    private void RefreshUI()
    {
        if (!hpChangeImage.activeSelf)
        {
            StartCoroutine(HP_FX());
        }
        slider.value = (float)king.HP / (float)king.MaxHP;
    }

    IEnumerator HP_FX()
    {
        hpChangeImage.SetActive(true);
        yield return waitSeconds;
        hpChangeImage.SetActive(false);
    }
}
