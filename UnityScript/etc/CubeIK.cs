using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeIK : MonoBehaviour
{
    [SerializeField] private float distance = 1f;
    private List<Transform> cubeList;
    private Camera cam;

    private void Awake()
    {
        cubeList = new List<Transform>(GetComponentsInChildren<Transform>());
        cubeList.RemoveAt(0);
        cam = Camera.main;
    }

    private Vector3 GetMousePos()
    {
        Vector3 v = Input.mousePosition;
        v.z = -10;
        Vector3 pos = cam.ScreenToWorldPoint(v);
        pos.z = 0;
        return pos;
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);  //ray.origin, ray.direction
            //Vector3 dir = (pos - cubeList[0].position).normalized;

            cubeList[0].position = ray.origin + ray.direction.normalized * 5f;

            for (int i = 1; i < cubeList.Count; i++)
            {
                cubeList[i].position = cubeList[i - 1].position + (cubeList[i].position - cubeList[i-1].position).normalized * distance;
            }
        }
    }
}
