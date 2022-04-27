using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isWall;

    public bool start, end;

    public int x, y;

    public Tile parent;

    public bool IsTrace { get; set; }

    /*private void Awake()
    {
        IsPath = false;
    }*/
}
