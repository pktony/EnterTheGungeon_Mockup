using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AudioMenu : MonoBehaviour
{
    Slider slider_Master;
    Slider slider_VFX;
    Slider slider_UI;

    private void Awake()
    {
        Transform panel = transform.GetChild(0).GetChild(0);
        slider_Master = panel.GetChild(0).GetComponentInChildren<Slider>();
        slider_VFX = panel.GetChild(1).GetComponentInChildren<Slider>();
        slider_UI = panel.GetChild(2).GetComponentInChildren<Slider>();

        slider_Master.onValueChanged.AddListener(ChangeVolume_Master);
        slider_Master.onValueChanged.AddListener(ChangeVolume_VFX);
        slider_Master.onValueChanged.AddListener(ChangeVolume_UI);
    }

    private void ChangeVolume_Master(float value)
    {
        GameManager.Inst.Volume_Master = value;
    }

    private void ChangeVolume_VFX(float value)
    {
        GameManager.Inst.Volume_VFX = value;
    }
    private void ChangeVolume_UI(float value)
    {
        GameManager.Inst.Volume_UI = value;
    }
}
