using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reload_UI : MonoBehaviour
{
    CanvasGroup group;

    RectTransform gauge;
    RectTransform bar;

    float baseWidth;
    float speed;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();

        gauge = transform.GetChild(0).GetComponent<RectTransform>();
        bar = transform.GetChild(1).GetComponent<RectTransform>();

        baseWidth = gauge.sizeDelta.x;
    }

    private void Start()
    {
        speed = baseWidth / GameManager.Inst.Player.CurrentWeapon.reloadingTime;
    }

    public IEnumerator ReloadUI()
    {
        while (GameManager.Inst.Player.IsReloading)
        {
            group.alpha = 1f;
            speed = baseWidth / GameManager.Inst.Player.CurrentWeapon.reloadingTime;
            bar.position += speed * Time.deltaTime * transform.right;
            yield return null;
            if (bar.localPosition.x > 0.74f)
            {
                group.alpha = 0f;
                bar.localPosition = new Vector2(-0.75f, 1f);
            }
        }
    }
}
