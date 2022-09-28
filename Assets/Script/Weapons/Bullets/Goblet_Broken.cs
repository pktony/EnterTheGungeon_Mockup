using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblet_Broken : MonoBehaviour
{
    public float flyForce = 3.0f;

    private void Update()
    {
        transform.position += flyForce * Time.deltaTime * (Vector3)Random.insideUnitCircle;
    }
}
