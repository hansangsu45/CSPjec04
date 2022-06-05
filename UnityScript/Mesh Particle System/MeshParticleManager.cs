using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshParticleManager : MonoBehaviour
{
    public static MeshParticleManager Instance;

    private MeshSystem meshSystem;
    private List<Particle> particleList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("�ټ��� ��ƼŬ �Ŵ��� ������");
            return;
        }

        Instance = this;
        meshSystem = GetComponent<MeshSystem>();
        particleList = new List<Particle>();
    }

    public void SpawnParticle(Vector3 pos, int idx, float time)
    {
        int totalSheet = meshSystem.GetTotalFrame(idx);
        Vector3 size = new Vector3(0.8f, 0.8f);
        float rot = Random.Range(0f, 360f);
        particleList.Add(new Particle(meshSystem, pos, rot, size, true, idx, totalSheet, time));
    }

    private void Update()
    {
        for(int i=0; i<particleList.Count; i++)
        {
            Particle p = particleList[i];
            p.Update();
            if (p.IsComplete)
            {
                particleList.RemoveAt(i);
                i--;
            }
        }
    }
}
