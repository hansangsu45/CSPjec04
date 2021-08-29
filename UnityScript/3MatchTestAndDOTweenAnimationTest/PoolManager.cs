using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    private Queue<GameObject> queue = new Queue<GameObject>();
    public GameObject _prefab;
    public Transform _parent;

    [SerializeField] int queueCnt;

    private void Awake()
    {
        instance = this;
       

        for(int i=0; i<40; i++)
        {
            InsertQueue( Instantiate(_prefab, Vector2.zero, Quaternion.identity, _parent) );

        }
    }

    public void InsertQueue(GameObject o)
    {
        queue.Enqueue(o);
        o.SetActive(false);
    }

    public GameObject GetQueue()
    {
        GameObject o = queue.Dequeue();
        o.SetActive(true);
        return o;
    }

    private void Update()
    {
        queueCnt = queue.Count;
    }
}
