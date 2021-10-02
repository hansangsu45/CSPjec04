using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject linePrefab;
    public LayerMask camDrawOverLayer;
    int camDrawOverLayerIndex;

    [Space(30f)]
    public Gradient lineColor;
    public float linePointsMinDistance;
    public float lineWidth;

    Line currentLine;
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
        camDrawOverLayerIndex = LayerMask.NameToLayer("CamDrawLayer");
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            BeginDraw();
        }

        if(currentLine!=null)
        {
            Draw();
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndDraw();
        }
    }

    private void BeginDraw()
    {
        currentLine = Instantiate(linePrefab, this.transform).GetComponent<Line>();

        currentLine.UsePhysics(false);
        currentLine.SetLineColor(lineColor);
        currentLine.SetPointsMinDistance(linePointsMinDistance);
        currentLine.SetLineWidth(lineWidth);
    }

    private void Draw()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.CircleCast(mousePosition, lineWidth / 3f, Vector2.zero, 1f, camDrawOverLayer);

        if (hit) EndDraw();
        else currentLine.AddPoint(mousePosition);
    }

    private void EndDraw()
    {
        if (currentLine != null)
        {
            if(currentLine.pointCount<2)
            {
                Destroy(currentLine.gameObject);
            }
            else
            {
                currentLine.gameObject.layer = camDrawOverLayerIndex;
                currentLine.UsePhysics(true);
                currentLine = null;
            }
        }
    }
}
