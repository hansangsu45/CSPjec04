using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICircleMove : MonoBehaviour
{
    public float speed;
    public float radius;
    public RectTransform point;
    private RectTransform rectTrm;

    private float dx, dy, r;

    public float offset;



    private void Awake()
    {
        rectTrm = GetComponent<RectTransform>();
        dx = rectTrm.anchoredPosition.x;
        dy = rectTrm.anchoredPosition.y;
        r = Mathf.Sqrt(dx * dx + dy * dy);
        offset *= 3.141592f / 2;

    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        //float x = Mathf.Cos(Time.time) * radius;
        //float y = Mathf.Sin(Time.time) * radius;

        //rectTrm.anchoredPosition = new Vector2(x + dx, y + dy);

        float x = Mathf.Cos(Time.time + offset) * r;
        float y = Mathf.Sin(Time.time + offset) * r;

        rectTrm.anchoredPosition = new Vector2(x, y);
    }
}
