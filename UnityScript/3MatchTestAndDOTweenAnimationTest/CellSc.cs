
using UnityEngine;

public class CellSc : MonoBehaviour
{
    public int id;
    public int row, col;

    public bool reserveScl0 = false;

    public PRS curPRS;

    [HideInInspector] public SpriteRenderer spr;

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (transform.localScale == Vector3.zero)
        {
            transform.localScale = curPRS.scale;
            reserveScl0 = false;
            GameManager.instance.CreateBlock(row, col, transform);  //임시 코드
            PoolManager.instance.InsertQueue(gameObject);
        }
    }

    public void SetData(int id, Sprite spr, int row, int col)
    {
        this.id = id;
        this.spr.sprite = spr;
        this.row = row;
        this.col = col;
    }

    public void SetPRS(bool create=true)
    {
        if(create)
           curPRS = new PRS(transform.localPosition, transform.localRotation, transform.localScale);
        else
        {
            curPRS.position = transform.localPosition;
            curPRS.rotation = transform.localRotation;
            curPRS.scale = transform.localScale;
        }
    }
    
}
