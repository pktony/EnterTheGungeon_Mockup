using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_AnimationHelper : MonoBehaviour
{
    public Action onIntroEnd;

    public void ActivateUIs() => onIntroEnd?.Invoke();
}
