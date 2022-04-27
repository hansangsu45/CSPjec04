using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [System.Serializable]
    public class Node
    {
        public Tile tile;
        public int G;
        public Node(Tile tile, int G)
        {
            this.tile = tile;
            this.G = G; 
        }
    }

    public static GameManager instance;

    public List<Tile> allTiles;

    public GameObject player;

    private List<List<Tile>> tiles = new List<List<Tile>>();

    private Tile start, end;

    private int g = 1;
    private readonly Vector3 playerPosOffset = new Vector3(0, 1, 0);

    private bool moveStart = false;

    private List<Node> open = new List<Node>();
    private List<Tile> close = new List<Tile>();

    private int x = 12, y = 12;
    private int idx;
    private Transform target;

    private List<Tile> path = new List<Tile>();

    private bool gameEnd = false;

    public Text txt;

    private void Awake()
    {
        instance = this;

        Init();
    }

    private void Init()
    {
        int index = 0;
        for (int i = 0; i < y; i++)
        {
            List<Tile> list = new List<Tile>();
            for(int j=0; j<x; j++)
            {
                list.Add(allTiles[index]);
                allTiles[index].x = j;
                allTiles[index].y = i;
                index++;
            }
            tiles.Add(list);
        }

        start = allTiles.Find(x => x.start);
        end = allTiles.Find(x => x.end);

        player.transform.position = start.transform.position + playerPosOffset;

        txt.text = "길 찾는 중...";
    }

    private void Start()
    {
        StartCoroutine(FindPath());
    }

    private void Update()
    {
        if(moveStart)
        {
            if(Vector3.Distance(player.transform.position, target.position + playerPosOffset) < 0.1f)
            {
                if(--idx >= 0)
                    target = path[idx].transform;
                else
                {
                    moveStart = false;
                    gameEnd = true;
                    txt.text = "도착함. (게임을 종료하시오 ALT + F4)";
                }
            }
            else
            {
                Vector3 dir = ((target.position + playerPosOffset) - player.transform.position).normalized;
                player.transform.position += dir * Time.deltaTime * 1.5f;
            }
        }
    }

    private void NextTiles(Tile tile)
    {
        

        if(tile.x + 1 < x)
        {
            CheckNext(tile, tiles[tile.y][tile.x + 1]);
        }

        if (tile.x - 1 >= 0)
        {
           
            CheckNext(tile, tiles[tile.y][tile.x - 1]);
        }

        if (tile.y + 1 < y)
        {
           
            CheckNext(tile, tiles[tile.y + 1][tile.x]);
        }

        if (tile.y - 1 >= 0)
        {

            CheckNext(tile, tiles[tile.y - 1][tile.x]);
        }
    }

    private void CheckNext(Tile pre, Tile t)
    {
        if (t != pre.parent && !t.isWall && !t.IsTrace)
        {
            if (!CheckSameTile(t))
            {
                open.Add(new Node(t, g));
                t.parent = pre;
                t.IsTrace = true;
            }
        }
    }

    private bool CheckSameTile(Tile t)
    {
        for(int i=0; i<open.Count; i++)
        {
            if(open[i].tile == t)
            {
                if(open[i].G > g)
                {
                    open.Remove(open[i]);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        return false;
    }

    private IEnumerator FindPath()
    {
        bool find = false;

        start.parent = null;
        close.Add(start);
        //start.IsPath = true;
        NextTiles(start);

        while (!find)
        {
            Tile next = GetMinFTileInOpenList();
            close.Add(next);

            Node n = open.Find(x => x.tile == next);
            open.Remove(n);

            NextTiles(next);
            g++;

            if(next == end)
            {
                find = true;
            }

            yield return null;
        }

        txt.text = "길 찾음";
        start.parent = null;
        Tile _t = end;

        while(_t.parent != null)
        {
            path.Add(_t);
            _t = _t.parent;
        }

        idx = path.Count - 1;
        target = path[idx].transform;
        moveStart = true;

        txt.text = "움직이는 중";
    }

   // private float DistanceSqrFromEnd(Vector3 pos) => (end.transform.position - pos).sqrMagnitude; //H^2
    private float DistanceFromEnd(Vector3 pos) => Vector3.Distance(pos, end.transform.position); //H

    private Tile GetMinFTileInOpenList()
    {
        float min = float.MaxValue;
        Tile tile = null;
        float dist;

        for(int i=0; i<open.Count; i++)
        {
            dist = DistanceFromEnd(open[i].tile.transform.position) + open[i].G;
            if (dist< min)
            {
                tile = open[i].tile;
                min = dist;
            }
        }

        return tile;
    }
}
