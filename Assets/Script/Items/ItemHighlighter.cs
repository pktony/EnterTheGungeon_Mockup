using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ItemHighlighter : MonoBehaviour
{
    Material mat;
    CircleCollider2D coll;

    //bool findItems = true;

    private void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
    }

    private Collider2D ClosestObject()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, coll.radius, LayerMask.GetMask("Items"));
        Collider2D closestColl = null;

        float closest = float.MaxValue;
        foreach (Collider2D coll in colls)
        {
            float temp = (coll.transform.position - transform.position).sqrMagnitude;
            if (temp < closest)
            {
                closest = temp;
            }
            closestColl = coll;
        }
        return closestColl;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Key") || collision.CompareTag("Heart") || collision.CompareTag("Ammo") || collision.CompareTag("BlankShell"))
    //    {
    //        if (findItems)
    //        {
    //            Collider2D coll = ClosestObject();
    //            mat = coll.GetComponent<SpriteRenderer>().material;
    //            findItems = false;
    //        }
    //    }
    //}


    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Key") || collision.CompareTag("Heart") || collision.CompareTag("Ammo") || collision.CompareTag("BlankShell"))
    //    {
    //        Material oldMat = null;
    //        if (mat != null)
    //            oldMat = mat;
    //        Collider2D coll = ClosestObject();
    //        mat = coll.GetComponent<SpriteRenderer>().material;
    //        if (mat != null)
    //        {
    //            if (oldMat != null && mat != oldMat)
    //            {
    //                oldMat.SetFloat("_Thickness", 0f);
    //                mat.SetFloat("_Thickness", 0.1f);
    //            }
    //        }
    //    }
    //    //else if (findItems)
    //    //{
    //    //    Collider2D coll = ClosestObject();
    //    //    mat = coll.GetComponent<SpriteRenderer>().material;
    //    //    findItems = false;
    //    //}
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (mat != null)
    //    {
    //        mat.SetFloat("_Thickness", 0f);
    //        mat = null;
    //    }
    //}
}
