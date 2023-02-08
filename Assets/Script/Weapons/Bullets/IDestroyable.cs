using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestroyable
{
    public const float blankExplosionTime = 0.5f;

    public void BlankDestroy();
}
