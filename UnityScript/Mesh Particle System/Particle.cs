using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{
    private MeshSystem meshSystem;
    private int quadIndex; //�� �� �ε���
    private int totalIndex; //��ü �ε���
    private int currentIndex;  //���� �ε���
    private float frameSec;  //�����Ӵ� �ð�

    private float currentTime; //���� �ð�
    private float rotation;  //���� ȸ��

    private Vector3 position;  //���� ��ġ
    private Vector3 quadSize;  //���� ũ��

    private bool skew;  // ��Ʋ�� ��� ����
    private bool isComplete; //�Ϸ� ����
    public bool IsComplete => isComplete;

    private int matIndex;  //���׸��󿡼� �� ��°

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
