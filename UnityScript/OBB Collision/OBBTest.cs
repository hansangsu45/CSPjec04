using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBBTest : MonoBehaviour
{
    public Transform other;

    private void Start()
    {
        Debug.Log(WidthVector(other));
        Debug.Log(HeightVector(other));
        Debug.Log(Normal(HeightVector(transform)));
        Debug.Log(DistanceVector(other.position));
        Debug.Log(AbsDotVector(Normal(WidthVector(transform)), DistanceVector(other.position)));
    }

    private void Update()
    {
        if(IsOBBCollision(other))
        {
            Debug.Log("OBB Ãæµ¹");
        }
    }

    private float AbsDotVector(Vector2 a, Vector2 b)
    {
        return Mathf.Abs(a.x * b.x + a.y * b.y);
    }

    private Vector2 Normal(Vector2 v)
    {
        float mag = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2));
        return new Vector2(v.x / mag, v.y / mag);
    }

    private Vector2 DistanceVector(in Vector2 other)
    {
        return new Vector2(
            transform.position.x - other.x,
            transform.position.y - other.y);
    }

    private Vector2 WidthVector(Transform tr)
    {
        return new Vector2
            (tr.localScale.x * Mathf.Cos((tr.eulerAngles.z - 90) * Mathf.Deg2Rad) * 0.5f,
            tr.localScale.x * Mathf.Sin((tr.eulerAngles.z) * Mathf.Deg2Rad) * 0.5f);
    }

    private Vector2 HeightVector(Transform tr)
    {
        return new Vector2
           (tr.localScale.y * Mathf.Cos(tr.eulerAngles.z * Mathf.Deg2Rad) * 0.5f,
           tr.localScale.y * Mathf.Sin(tr.eulerAngles.z * Mathf.Deg2Rad) * 0.5f);
    }

    private bool IsOBBCollision(Transform tr)
    {
        Vector2 dist = DistanceVector(tr.transform.position);
        List<Vector2> lList = new List<Vector2>()
        {
            WidthVector(transform), HeightVector(transform), WidthVector(tr), HeightVector(tr)
        };

        for(int i=0; i < lList.Count; i++)
        {
            if(!CheckLCol(lList, Normal(lList[i]), dist))
            {
                return false;
            }

        }

        return true;
    }

    private bool CheckLCol(in List<Vector2> lList, Vector2 l, Vector2 dist)
    {
        float f = 0f;

        for(int i=0; i < lList.Count; i++)
        {
            f += AbsDotVector(lList[i], l);
        }

        return AbsDotVector(dist, l) < f;
    }
}
