using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_Character : MonoBehaviour
{
    public Animator anim;

    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void DodgeRoll()
    {
        anim.SetTrigger("DodgeRoll");
    }

}
