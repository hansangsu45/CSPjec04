using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{
    private MeshSystem meshSystem;
    private int quadIndex; //몇 번 인덱스
    private int totalIndex; //전체 인덱스
    private int currentIndex;  //현재 인덱스
    private float frameSec;  //프레임당 시간

    private float currentTime; //현재 시간
    private float rotation;  //현재 회전

    private Vector3 position;  //현재 위치
    private Vector3 quadSize;  //현재 크기

    private bool skew;  // 뒤틀림 허용 여부
    private bool isComplete; //완료 여부
    public bool IsComplete => isComplete;

    private int matIndex;  //메테리얼에서 몇 번째

    public Particle(MeshSystem meshSystem, Vector3 position, float rotation, Vector3 quadSize, bool skew, int matIndex, int total, float time)
    {
        this.meshSystem = meshSystem;
        this.position = position;   
        this.rotation = rotation;
        this.quadSize = quadSize;
        this.skew = skew;
        this.matIndex = matIndex;
        this.totalIndex = total;
        this.frameSec = time/totalIndex;
        this.currentTime = 0f;
        this.currentIndex = 0;
        isComplete = false;

        quadIndex = meshSystem.AddQuad(position, rotation, quadSize, skew, matIndex); 

    }

    public void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime > frameSec)
        {
            currentTime = 0;
            currentIndex++;
            if (currentIndex >= totalIndex)
            {
                isComplete = true;
                meshSystem.DestoryQuad(quadIndex);
                return;
            }
        }
        meshSystem.UpdateQuad(quadIndex, position, rotation, quadSize, skew, matIndex, currentIndex);
    }
}
