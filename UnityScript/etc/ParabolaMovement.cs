using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaMovement : MonoBehaviour
{
    private float tx, ty, tz;
    public float g = 9.8f;

    private float elapsed;
    private float maxHeight;
    private float speed;
    private Vector3 startPos;
    private Vector3 targetPos;

    private float dat;

    private bool isMoving = false;

    public void Set(Vector3 start, Vector3 target, float maxHeight, float speed)
    {
        this.startPos = start;
        this.targetPos = target;
        this.maxHeight = maxHeight;
        this.speed = speed;
        transform.position = start;

        float dh = target.y - startPos.y;
        float mh = this.maxHeight - start.y;
        ty = Mathf.Sqrt(2 * g * mh);

        float a = g;
        float b = -2 * ty;
        float c = 2 * dh;

        dat = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

        tx = -(startPos.x - targetPos.x) / dat;
        tz = -(startPos.z - targetPos.z) / dat;

        elapsed = 0f;
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            elapsed += Time.deltaTime * speed;

            float tx = startPos.x + this.tx * elapsed;
            float ty = startPos.y + this.ty * elapsed - 0.5f * g * Mathf.Pow(elapsed, 2);
            float tz = startPos.z + this.tz * elapsed;

            Vector3 tpos = new Vector3(tx, ty, tz);

            transform.LookAt(tpos);

            transform.position = tpos;

            if (elapsed >= this.dat)
            {
                transform.position = targetPos;
                isMoving = false;
            }
        }
    }
}
