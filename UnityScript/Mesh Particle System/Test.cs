using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
   
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mPos.z = 0;
            
            for(int i=0; i<100; i++)
            {
                Vector3 pos = Random.insideUnitCircle * 2f;
                int idx = Random.Range(0, 3);
                float time = Random.Range(0.4f, 0.8f);
                MeshParticleManager.Instance.SpawnParticle(mPos + pos, idx, time);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            MeshParticleManager.Instance.SpawnParticle(Vector3.zero, 0, 0.8f);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MeshParticleManager.Instance.SpawnParticle(Vector3.one, 1, 0.8f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MeshParticleManager.Instance.SpawnParticle(Vector3.zero * 2f, 2, 0.8f);
        }
    }
}
