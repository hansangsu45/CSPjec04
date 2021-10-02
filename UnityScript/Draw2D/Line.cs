using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer line;
    public EdgeCollider2D edgeCol;
    public Rigidbody2D rigid;

    [HideInInspector] public List<Vector2> points = new List<Vector2>();
    [HideInInspector] public int pointCount = 0;

    float pointsMinDistance = 0.1f;
    float circleColRad;

    public void AddPoint(Vector2 newPoint)
    {
        if(pointCount>=1 && Vector2.Distance(newPoint, GetLastPoint()) < pointsMinDistance)
        {
            return;
        }

        points.Add(newPoint);
        pointCount++;

        CircleCollider2D circleCol = this.gameObject.AddComponent<CircleCollider2D>();
        circleCol.offset = newPoint;
        circleCol.radius = circleColRad;

        line.positionCount = pointCount;
        line.SetPosition(pointCount - 1, newPoint);

        if (pointCount > 1) edgeCol.points = points.ToArray();
    }

    public Vector2 GetLastPoint()
    {
        return (Vector2)line.GetPosition(pointCount - 1);
    }

    public void UsePhysics(bool usePhysics)
    {
        rigid.isKinematic = !usePhysics;
    }

    public void SetLineColor(Gradient colorGradient)
    {
        line.colorGradient = colorGradient;
    }

    public void SetPointsMinDistance(float distance)
    {
        pointsMinDistance = distance;
    }

    public void SetLineWidth(float width)
    {
        line.startWidth = width;
        line.endWidth = width;

        circleColRad = width / 2f;
        edgeCol.edgeRadius = circleColRad;
    }
}
