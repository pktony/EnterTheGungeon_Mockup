using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlankFX : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();   
    }

    public void PlayBlankFX()
    {
        anim.SetTrigger("useBlank");
    }
}
