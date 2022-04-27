using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Color wallColor, floorColor, startColor, endColor, moveColor;

    public GameObject nodePrefab;
    public Transform nodesParent;
    public GameObject startBtn, endBtn, bfsBtn;
    public Text resTx;

    public int x = 16, y = 8;

    public List<List<Node>> nodes = new List<List<Node>>();
    private List<Node> allNodes = new List<Node>();
    private Stack<Node> nodesStack = new Stack<Node>();

    private Queue<Node> nodeQueue = new Queue<Node>();

    public Node StartPoint => nodes[3][0];
    public Node EndPoint { get; set; }

    private void Awake()
    {
        instance = this;

        for(int i= 0; i < y; i++)
        {
            List<Node> list = new List<Node>();
            for(int j=0; j<x; j++)
            {
                Node node = Instantiate(nodePrefab, nodesParent).GetComponent<Node>();
                node.x = j;
                node.y = i;
                list.Add(node);
                allNodes.Add(node);
            }
            nodes.Add(list);
        }

        StartPoint.SetStartPoint();

        allNodes.ForEach(x => x.SetNeighbor());
    }

    #region prev
    private void GameStart(bool bfs = false)
    {
        resTx.gameObject.SetActive(false);
        nodesStack.Clear();
        nodesStack.Push(StartPoint);

        nodeQueue.Clear();
        nodeQueue.Enqueue(StartPoint);

        Node node = allNodes.Find(x => x.endPoint);
        if (node != null) node.endPoint = false;

        node = nodes[Random.Range(0, 8)][x-1];
        node.SetEndPoint();
        EndPoint = node;

        allNodes.ForEach(x =>
        {
            x.index = 0;
            x.SetInit(false);
        });

        int wallCnt = Random.Range(25, 41);
        for (int i = 0; i < wallCnt; i++)
        {
            node = allNodes[Random.Range(0, allNodes.Count)];
            if (node.isWall || node.startPoint || node.endPoint)
            {
                i--;
                continue;
            }
            node.SetInit(true);
        }

        endBtn.SetActive(true);
        EndPoint.isWall = false;
        StartPoint.isWall = false;

        if(!bfs)
            StartCoroutine(FindEndPoint());
        else
            StartCoroutine(FindEndPoint2());

    }

    private bool CheckCanGameClear()
    {
        Node node;
        Node prevNode = nodesStack.Peek();
        do
        {
            node = prevNode.GetNextPath;
        } while (node != null && (node.isWall || node == prevNode));

        if(!node)
        {
            bool er = false;
            try
            {
                nodesStack.Pop();
                node = nodesStack.Peek();
            }
            catch { er = true; }

            if(nodesStack.Count == 0 || er)
            {
                StopAllCoroutines();
                End(false);
                return false;
            }

        }
        else
        {
            nodesStack.Push(node);
            
        }
        prevNode.SetInit(false);
        node.SetCurrentNode();


        return node == EndPoint;
    }

    public void OnClickStartBtn()
    {
        bfsBtn.gameObject.SetActive(false);
        startBtn.gameObject.SetActive(false);
        GameStart();
    }
    

    private IEnumerator FindEndPoint()
    {
        WaitForSeconds ws = new WaitForSeconds(0.3f);
        bool find = false;

        while(!find)
        {
            yield return ws;
            find = CheckCanGameClear();
        }

        End(find);
    }


    #endregion
    public void OnClickStopBtn()
    {
        StopAllCoroutines();
        startBtn.SetActive(true);
        bfsBtn.gameObject.SetActive(true);
        endBtn.SetActive(false);
    }
    private void End(bool clear)
    {
        resTx.gameObject.SetActive(true);
        resTx.text = clear ? "찾았다" : "못찾았다";
        OnClickStopBtn();
    }

    #region bfs
    public void OnClickStartBtn2()
    {
        bfsBtn.gameObject.SetActive(false);
        startBtn.gameObject.SetActive(false);
        GameStart(true);
    }

    private IEnumerator FindEndPoint2()
    {
        WaitForSeconds ws = new WaitForSeconds(0.2f);
        bool find = false;

        StartPoint.ParentNode = null;
        StartPoint.IsPass = true;
        allNodes.ForEach(x=>x.IsPass = false);

        int x, y, nx, ny;
        

        int[] dx = { 1, 0, 0, -1 };
        int[] dy = { 0, -1, 1, 0 };
        Node node;

        while(nodeQueue.Count > 0)
        {
            yield return null;

            node = nodeQueue.Dequeue();
            x = node.x;
            y = node.y;

            if(node == EndPoint)
            {
                find = true;
                break;
            }

            for(int i=0; i<4; i++)
            {
                nx = x + dx[i];
                ny = y + dy[i];
                
                Debug.Log(nx + " " + ny); 

                if(nx < this.x && ny < this.y && nx >=0 && ny >=0)
                {
                    if (!nodes[ny][nx].isWall && !nodes[ny][nx].IsPass)
                    {
                        nodes[ny][nx].ParentNode = node;
                        nodeQueue.Enqueue(nodes[ny][nx]);
                        nodes[ny][nx].IsPass = true;
                    }
                }
            }
        }

        if(find)
        {
            Debug.Log("찾음");
            
            List<Node> li = new List<Node>();
            Node pn = EndPoint;
            Node pre = null;
            li.Add(pn);

            StartPoint.ParentNode = null;
            while (pn != null)
            {
                yield return null;
                pn = pn.ParentNode;
                
                if(pn)
                   li.Add(pn);
            }

            testList = li;

            for(int i=li.Count-1; i>=0; i--)
            {
                if (pre)
                {
                    pre.SetInit(false);
                }

                pre = li[i];
                li[i].SetCurrentNode();
               
                yield return ws;
            }
        }

        End(find);
    }

    public List<Node> testList;

    #endregion
}
