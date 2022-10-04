using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Audio Clip", menuName = "Scriptable Object/New Audio Clip", order = 2)]
public class ClipData : ScriptableObject
{
    public AudioClip clip;
    public float clipVolume = 0.5f;
}
