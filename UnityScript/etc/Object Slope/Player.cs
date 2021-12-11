using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 8f;
    public float jumpPower = 10f;
    public float gravity = -3f;  //중력 너무 크면 오르막길 못 올라감
    private Rigidbody rigid;

    public Transform forwardRayTrm;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Move();
        Rotate();
        Jump();
        CheckSlope();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {


        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 movDir = new Vector3(x, 0, z).normalized;
        movDir.y = gravity;
        movDir *= speed;

        // -rigid.velocity.x (z)를 하지 않으면 미끄러짐이 심함. 오르막길 못 올라감 + 내리막길에서 가속받음
        rigid.AddForce(new Vector3(movDir.x-rigid.velocity.x,movDir.y,movDir.z-rigid.velocity.z), ForceMode.VelocityChange);
    }

    private void Rotate()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            transform.Rotate(new Vector3(0, -90, 0) * Time.deltaTime * 2);
        }
        if(Input.GetKey(KeyCode.Z))
        {
            transform.Rotate(new Vector3(0, 90, 0) * Time.deltaTime * 2);
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigid.velocity = Vector3.up * jumpPower;
        }
    }

    private void CheckSlope()
    {
        RaycastHit hit;

        Debug.DrawRay(forwardRayTrm.position, transform.forward * 1.3f, Color.blue);
        if(Physics.Raycast(forwardRayTrm.position, transform.forward, out hit, 1.3f, LayerMask.GetMask("Ground")))
        {
            if(hit.transform)
            {
                Vector3 normal = hit.normal;

                float dot = Vector3.Dot(transform.forward, normal);

                float deg = Mathf.Acos(dot / transform.forward.magnitude / normal.magnitude) * Mathf.Rad2Deg - 90f;

                Debug.Log(deg);  //오브젝트의 경사 (오브젝트가 몇 도 기울어져 있는지 구한다)
            }
        }
    }
}
