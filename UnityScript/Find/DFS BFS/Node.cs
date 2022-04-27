
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public int x, y;
    public bool isWall = false;

    public bool startPoint = false;
    public bool endPoint = false;

    private List<Node> neighbor = new List<Node>();
    public int index = 0;

    public Node ParentNode;
    public bool IsPass { get; set; }

    public void SetInit(bool w)
    {
        if (startPoint || endPoint) return;

        isWall = w;
        GetComponent<Image>().color = isWall ? GameManager.instance.wallColor : GameManager.instance.floorColor; 
    }

    public void SetStartPoint()
    {
        startPoint = true;
        GetComponent<Image>().color = GameManager.instance.startColor;
    }

    public void SetEndPoint()
    {
        endPoint = true;
        GetComponent<Image>().color = GameManager.instance.endColor;
    }

    public void SetCurrentNode()
    {
        if (startPoint) return;

        GetComponent<Image>().color = GameManager.instance.moveColor;
    }

    public void SetNeighbor()
    {
        int maxX = GameManager.instance.x;
        int maxY = GameManager.instance.y;
        if(x+1 < maxX)
        {
            neighbor.Add(GameManager.instance.nodes[y][x + 1]);
        }
        if (y - 1  >= 0)
        {
            neighbor.Add(GameManager.instance.nodes[y-1][x]);
        }
        if (y + 1 < maxY)
        {
            neighbor.Add(GameManager.instance.nodes[y+1][x]);
        }
        if (x - 1 >= 0)
        {
            neighbor.Add(GameManager.instance.nodes[y][x - 1]);
        }
    }

    public Node GetNextPath
    {
        get
        {
            if (index == neighbor.Count) return null;
            else
            {
                index++;
                return neighbor[index - 1];
            }
        }
    }
}
