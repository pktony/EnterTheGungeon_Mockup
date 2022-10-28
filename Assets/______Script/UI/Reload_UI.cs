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

    Animator anim;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        anim = GetComponent<Animator>();

        gauge = transform.GetChild(0).GetComponent<RectTransform>();
        bar = transform.GetChild(1).GetComponent<RectTransform>();

        baseWidth = gauge.sizeDelta.x;
    }

    private void Start()
    {
        Player player = GetComponentInParent<Player>();
        speed = baseWidth / player.CurrentWeapon.reloadingTime;
    }

    public IEnumerator ReloadUI()
    {
        group.alpha = 1f;
        speed = baseWidth / GameManager.Inst.Player.CurrentWeapon.reloadingTime;

        while (GameManager.Inst.Player.IsReloading)
        {
            bar.position += speed * Time.deltaTime * transform.right;
            if (bar.localPosition.x > 0.74f)
            {
                group.alpha = 0f;
                bar.localPosition = new Vector2(-0.75f, 1f);
            }
            yield return null;
        }
    }
}
