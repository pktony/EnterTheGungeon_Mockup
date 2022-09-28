using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    static Camera mainCam;

    private static Vector2 initialPosition;
    private static bool isShake = false;

    private float timer = 0f;
    private static float shakeTime;
    private static float shakePower;

    private void Awake()
    {
        mainCam = GetComponent<Camera>();
    }

    public static void ShakeCamera(float _shakeTime, float _shakePower)
    {
        if (!isShake)
        {
            isShake = true;
            shakeTime = _shakeTime;
            shakePower = _shakePower;
            initialPosition = mainCam.transform.position;
        }
            //StartCoroutine(camShake(shakeTime, shakePower)) ;
    }

    //private static IEnumerator camShake(float shakeTime, float shakePower)
    //{
    //    float timer = 0f;
    //    initialPosition = mainCam.transform.position;
    //    while (timer < shakeTime)
    //    {
    //        timer += Time.deltaTime;
    //        mainCam.transform.position = Random.insideUnitCircle * shakePower + initialPosition ;
    //        yield return null;
    //    }

    //    mainCam.transform.position = initialPosition;
    //}

    private void Update()
    {
        if (isShake)
        {
            timer += Time.deltaTime;
            if (timer < shakeTime)
            {
                mainCam.transform.position = Random.insideUnitCircle * shakePower + initialPosition;
            }
            else
            {
                mainCam.transform.position = initialPosition;
                timer = 0f;
                isShake = false;
            }
        }
    }

}
