using UnityEngine;

/// <summary>
/// 카메라 진동을 위한 클래스 
/// </summary>
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

    /// <summary>
    /// 카메라 진동 실행 함수 
    /// </summary>
    /// <param name="_shakeTime"> 진동 시간 </param>
    /// <param name="_shakePower"> 진동 세기 </param>
    public static void ShakeCamera(float _shakeTime, float _shakePower)
    {
        if (!isShake)
        {
            isShake = true;
            shakeTime = _shakeTime;
            shakePower = _shakePower;
            initialPosition = mainCam.transform.position;
        }
    }

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
