using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform leftUp, leftDown, rightUp, rightDown;
    public GameObject cellPrefab;
    public Sprite[] cellSprs;
    public float dragDist;

    [SerializeField] private CellSc selectedCell;
    [SerializeField] private List<List<CellSc>> cells;
    [SerializeField] int rowOrColCount = 10;
    private Transform board;
    RaycastHit2D hit;
    Sequence seq1, seq2;

    [SerializeField] float changeTime = 0.3f;
    private Vector2 clickVec;
    private bool cellClick=false;
    private int rowMaxIdx;
    private int X, Y;
    private bool isChanging = false;

    private event Action MatchEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        InitSet();
        CreateCell();
    }

    void InitSet()
    {
        rowMaxIdx = rowOrColCount - 1;
        cells = new List<List<CellSc>>();
        board = leftDown.parent.parent;
    }

    void CreateCell()
    {
        float xf = Mathf.Abs(rightUp.localPosition.x - leftUp.localPosition.x) / ((float)rowOrColCount + 2f);
        float yf = Mathf.Abs(leftDown.localPosition.y - leftUp.localPosition.y) / ((float)rowOrColCount + 2f);
        float ff = (float)rowOrColCount + 1f;
        dragDist = xf *14f;

        for (int i=0; i<rowOrColCount; i++)
        {
            List<CellSc> cl = new List<CellSc>();

            for(int j=0; j<rowOrColCount; j++)
            {
                CellSc cell = Instantiate(cellPrefab, board).GetComponent<CellSc>();
                int r = UnityEngine.Random.Range(0, cellSprs.Length);
                cell.SetData(r, cellSprs[r], j, i);

                float x = Vector2.Lerp((Vector2)leftUp.localPosition, (Vector2)rightUp.localPosition, j / ff ).x + xf;
                float y = Vector2.Lerp((Vector2)leftUp.localPosition, (Vector2)leftDown.localPosition, i / ff ).y - yf;
                cell.transform.localPosition = new Vector2(x, y);
                cell.SetPRS();

                cl.Add(cell);
            }

            cells.Add(cl);
        }
    }

    private void Update()
    {
        MouseClick();
    }

    void MouseClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(pos,Vector2.zero);

            if(hit.transform!=null)
            {
                if(hit.transform.CompareTag("Cell"))
                {
                    clickVec = pos;
                    cellClick = true;
                    selectedCell = hit.transform.GetComponent<CellSc>();
                }
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(cellClick)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                float d = Vector2.Distance(pos, clickVec);

                if (d >= dragDist && !isChanging)
                {
                    X = 0;
                    Y = 0;
                    float x = Mathf.Abs(pos.x - clickVec.x);
                    float y = Mathf.Abs(pos.y - clickVec.y);

                    if(x>=y) //가로 변경
                    { 
                        if(pos.x>=clickVec.x)  //오른쪽으로 드래그
                        {
                            if (selectedCell.row == rowMaxIdx) return;

                            X = 1;
                        }
                        else  //왼쪽으로 드래그
                        {
                            if (selectedCell.row == 0) return;

                            X = -1;
                        }
                    }
                    else  //세로 변경
                    {
                        if(pos.y>=clickVec.y)  //위로 드래그
                        {
                            if (selectedCell.col == 0) return;

                            Y = -1;
                        }
                        else  //아래로 드래그
                        {
                            if (selectedCell.col == rowMaxIdx) return;

                            Y = 1;
                        }
                    }

                    TryCellChange(selectedCell, cells[selectedCell.col + Y][selectedCell.row + X]);

                }

                cellClick = false;
            }
        }
    }

    void TryCellChange(CellSc first, CellSc second)
    {
        isChanging = true;
        bool b = CheckThreeMatch(first, second);
        
        seq1 = DOTween.Sequence();
        seq2 = DOTween.Sequence();

        if (b)
        {
            seq1.Append(first.transform.DOLocalMove(second.curPRS.position, changeTime));
            seq2.Append(second.transform.DOLocalMove(first.curPRS.position, changeTime));
        }
        else
        {
            seq1.Append(first.transform.DOLocalMove(second.curPRS.position, changeTime));
            seq2.Append(second.transform.DOLocalMove(first.curPRS.position, changeTime));

            seq1.Append(first.transform.DOLocalMove(first.curPRS.position, changeTime));
            seq2.Append(second.transform.DOLocalMove(second.curPRS.position, changeTime));
        }

        seq2.AppendCallback(() =>
        {
            isChanging = b;
            first.SetPRS(false);
            second.SetPRS(false);

            if (b) EventStart();
        });

        seq1.Play();
        seq2.Play();

        if (b)
        {
            cells[selectedCell.col][selectedCell.row] = second;
            cells[selectedCell.col + Y][selectedCell.row + X] = first;

            int temp1 = first.row, temp2 = first.col;
            first.row = second.row;
            first.col = second.col;
            second.row = temp1;
            second.col = temp2;
        }
    }

    bool CheckThreeMatch(CellSc first, CellSc second)
    {
        bool b=false;
        int i, j, count=0, firIdx=-1;

        cells[selectedCell.col][selectedCell.row] = second;
        cells[selectedCell.col + Y][selectedCell.row + X] = first;
        int temp1 = first.row, temp2 = first.col;
        first.row = second.row;
        first.col = second.col;
        second.row = temp1;
        second.col = temp2;

        for (i=0; i<rowOrColCount; ++i)  //first 가로
        {
            if(cells[first.col][i].id==first.id)
            {
                count++;
                if (firIdx == -1) firIdx = i;

                if (i == rowMaxIdx && count>=3)
                {
                    b = true;
                    for (j = firIdx; j < firIdx + count; j++)
                    {
                        cells[first.col][j].reserveScl0 = true;
                    }
                    /*MatchEvent += () =>  
                    {
                        for (j = firIdx; j < firIdx + count; j++)
                        {
                            cells[first.col][j].transform.DOScale(Vector3.zero, 0.3f);
                            
                        }
                    };*/

                    break;
                }
            }
            else
            {
                if(count>=3)
                {
                    b = true;
                    for (j = firIdx; j < firIdx + count; j++)
                    {
                        cells[first.col][j].reserveScl0 = true;
                    }
                    /*MatchEvent += () =>
                    {
                        for(j=firIdx; j<firIdx+count; j++)
                        {
                            cells[first.col][j].transform.DOScale(Vector3.zero, 0.3f);
                            Debug.Log(firIdx.ToString() + " " + count.ToString());
                        }
                    };*/

                    break;
                }
                count = 0;
                firIdx = -1;
            }
        }
        
        count = 0; firIdx = -1;
        for (i = 0; i < rowOrColCount; ++i)  //first 세로
        {
            if (cells[i][first.row].id == first.id)
            {
                count++;
                if (firIdx == -1) firIdx = i;

                if (i == rowMaxIdx && count >= 3)
                {
                    b = true;
                    for (j = firIdx; j < firIdx + count; j++)
                    {
                        cells[j][first.row].reserveScl0 = true;
                    }
                    /* MatchEvent += () =>
                     {
                         for (j = firIdx; j < firIdx + count; j++)
                         {
                             cells[j][first.row].transform.DOScale(Vector3.zero, 0.3f);
                         }
                     };*/

                    break;
                }
            }
            else
            {
                if (count >= 3)
                {
                    b = true;
                    for (j = firIdx; j < firIdx + count; j++)
                    {
                        cells[j][first.row].reserveScl0 = true;
                    }
                    /*MatchEvent += () =>
                    {
                        for (j = firIdx; j < firIdx + count; j++)
                        {
                            cells[j][first.row].transform.DOScale(Vector3.zero, 0.3f);
                        }
                    };*/

                    break;
                }
                count = 0;
                firIdx = -1;
            }
        }
        
        count = 0; firIdx = -1;
        for (i = 0; i < rowOrColCount; ++i)  //second 가로
        {
            if (cells[second.col][i].id == second.id)
            {
                count++;
                if (firIdx == -1) firIdx = i;

                if (i == rowMaxIdx && count >= 3)
                {
                    b = true;
                    for (j = firIdx; j < firIdx + count; j++)
                    {
                        cells[second.col][j].reserveScl0 = true;
                    }
                    /* MatchEvent += () =>
                     {
                         for (j = firIdx; j < firIdx + count; j++)
                         {
                             cells[second.col][j].transform.DOScale(Vector3.zero, 0.3f);
                         }
                     };*/
                }
            }
            else
            {
                if (count >= 3)
                {
                    b = true;
                    for (j = firIdx; j < firIdx + count; j++)
                    {
                        cells[second.col][j].reserveScl0 = true;
                    }
                    /*MatchEvent += () =>
                    {
                        for (j = firIdx; j < firIdx + count; j++)
                        {
                            cells[second.col][j].transform.DOScale(Vector3.zero, 0.3f);
                        }
                    };*/

                    break;
                }
                count = 0;
                firIdx = -1;
            }
        }
        
        count = 0; firIdx = -1;
        for (i = 0; i < rowOrColCount; ++i)  //second 세로
        {
            if (cells[i][second.row].id == second.id)
            {
                count++;
                if (firIdx == -1) firIdx = i;

                if (i == rowMaxIdx && count >= 3)
                {
                    b = true;
                    for (j = firIdx; j < firIdx + count; j++)
                    {
                        cells[j][second.row].reserveScl0 = true;
                    }
                    /* MatchEvent += () =>
                     {
                         for (j = firIdx; j < firIdx + count; j++)
                         {
                             cells[j][second.row].transform.DOScale(Vector3.zero, 0.3f);
                         }
                     };*/
                    break;
                }
            }
            else
            {
                if (count >= 3)
                {
                    b = true;
                    for (j = firIdx; j < firIdx + count; j++)
                    {
                        cells[j][second.row].reserveScl0 = true;
                    }
                    /* MatchEvent += () =>          //이런식으로 이벤트에 밑의 함수를 넣어서 실행하면 4개의 for문 중 마지막꺼만 실행되는 문제 있음(아마 지역변수가 달라져서 그럴 수도(firIdx,count))
                     {
                         for (j = firIdx; j < firIdx + count; j++)
                         {
                             cells[j][second.row].transform.DOScale(Vector3.zero, 0.3f);

                         }
                     };*/

                    break;
                }
                count = 0;
                firIdx = -1;
            }
        }
        
        cells[selectedCell.col - Y][selectedCell.row - X] = first;
        cells[selectedCell.col][selectedCell.row] = second;
        temp1 = first.row; temp2 = first.col;
        first.row = second.row;
        first.col = second.col;
        second.row = temp1;
        second.col = temp2;

        return b;
    }
    

    void EventStart()
    {
        cells.ForEach(x =>
        {
            x.ForEach(y =>
            {
                if(y.reserveScl0) y.transform.DOScale(Vector3.zero, 0.3f);
            });
        });

        isChanging = false;

        /*if (MatchEvent == null) return;
        MatchEvent();

        foreach(Action ac in MatchEvent.GetInvocationList())
        {
            MatchEvent -= ac;
        }

        isChanging = false;*/
    }

    
    public void CreateBlock(int row, int col, Transform tr)
    {
        CellSc cell = PoolManager.instance.GetQueue().GetComponent<CellSc>();
        int r = UnityEngine.Random.Range(0, cellSprs.Length);
        cell.SetData(r, cellSprs[r], row, col);

        cell.spr.color = new Color(0, 0, 0, 0);
        cell.transform.position = tr.position;
        cell.SetPRS();

        cells[col][row] = cell;

        cell.spr.DOColor(Color.white, 1);
    }
}
