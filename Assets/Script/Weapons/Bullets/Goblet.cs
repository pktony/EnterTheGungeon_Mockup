using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Goblet : MonoBehaviour
{
    private GameObject goblet;
    private GameObject goblet_Broken;
    private GameObject wine;
    private GameObject flame;

    private Vector2 throwPosition;
    private Vector2 throwDir;
    private bool isThrown = false;
    private float timer = 0f;

    private const float EXPLOSION_DISTANCE = 0.1f;

    public float gobletSpeed = 3.0f;
    public float splashDuration = 3.0f;

    private void Awake()
    {
        goblet = transform.GetChild(0).gameObject;
        goblet_Broken = transform.GetChild(1).gameObject;
        wine = transform.GetChild(2).gameObject;
        flame = transform.GetChild(3).gameObject;
    }
    
    private void OnEnable()
    {
        if(GameManager.Inst != null)
        {
            throwPosition = GameManager.Inst.Player.transform.position;
            throwDir = throwPosition - (Vector2)transform.position;
            throwDir = throwDir.normalized;
            isThrown = true;
            goblet.SetActive(true);
            timer = 0f;
        }
    }

    private void Update()
    {
        if(isThrown)
        {
            transform.position += Time.deltaTime * gobletSpeed * (Vector3)throwDir;
            if((throwPosition - (Vector2)transform.position).sqrMagnitude < EXPLOSION_DISTANCE * EXPLOSION_DISTANCE)
            {
                goblet.SetActive(false);
                goblet_Broken.SetActive(true);
                wine.SetActive(true);
                isThrown = false;
            }
        }
        else
        {
            timer += Time.deltaTime;
            if(timer > splashDuration)
            {
                wine.SetActive(false);
                BulletManager.Inst.ReturnBullet(BulletID.GOBLET, this.gameObject);
            }
        }
    }
}
