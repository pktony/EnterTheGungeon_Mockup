using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Door : MonoBehaviour
{
    Animator anim;
    Collider2D coll;
    CameraMove mainCam;
    PlayableDirector timeline;

    Vector2 bossPosition;

    //public float camSpeed = 3.0f;

    bool isDoorOpen = false;
    public bool IsDoorOpen => isDoorOpen;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        timeline = GetComponent<PlayableDirector>();
    }

    private void Start()
    {
        mainCam = FindObjectOfType<CameraMove>();
        bossPosition = new Vector2(1.5f, 10f);
    }

    IEnumerator OpenDoor()
    {
        GameManager.Inst.Control.DisableInput();
        CameraShake.ShakeCamera(2.0f, 2.0f);
        yield return new WaitForSeconds(1.0f);
        anim.SetTrigger("onOpen");
        coll.enabled = false;
        yield return new WaitForSeconds(2.0f);
        timeline.Play();
        yield return new WaitForSeconds(1.0f);
        isDoorOpen = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(OpenDoor());
        }
    }
}
