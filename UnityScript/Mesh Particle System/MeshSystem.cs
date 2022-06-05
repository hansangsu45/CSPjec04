using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MeshSystem : MonoBehaviour
{
    private const int MAX_QUAD_AMOUNT = 1000;

    private MeshFilter meshFilter;
    private Mesh mesh;
    private MeshRenderer meshRenderer;

    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    #region 메테리얼 정보 입력
    [SerializeField] private int[] sheetCount;
    private int maxSheetCount;
    #endregion

    #region 쿼드 생성 및 업데이트 변수들
    private int quadIndex = 0;
    private bool updateVertices = false;
    private bool updateUv = false;
    private bool updateTriangles = false;
    #endregion

    public struct UVCoords
    {
        public Vector2 uv00;
        public Vector2 uv11;
    }

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        vertices = new Vector3[4*MAX_QUAD_AMOUNT];
        uv = new Vector2[4 * MAX_QUAD_AMOUNT];
        triangles = new int[6*MAX_QUAD_AMOUNT];

        mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;

        meshRenderer.sortingLayerName = "Default";
        meshRenderer.sortingOrder = 10;
        maxSheetCount = sheetCount.Max();
    }

    public int GetTotalFrame(int idx)
    {
        return sheetCount[idx];
    }

    public int AddQuad(Vector3 position, float rotation, Vector3 quadSize, bool skewed, int idx)
    {
        UpdateQuad(quadIndex, position, rotation, quadSize, skewed, idx, 0);
        int spawnedQuadIndex = quadIndex;
        quadIndex = (quadIndex + 1) % MAX_QUAD_AMOUNT;

        return spawnedQuadIndex;
    }

    public void UpdateQuad(int quadIndex, Vector3 pos, float rot, Vector3 quadSize,
        bool skewed, int idx, int animIdx)
    {
        int vIndex0 = quadIndex * 4;
        int vIndex1 = vIndex0 + 1;
        int vIndex2 = vIndex0 + 2;
        int vIndex3 = vIndex0 + 3;

        if (skewed)
        {
            vertices[vIndex0] = pos + Quaternion.Euler(0, 0, rot) * new Vector3(-quadSize.x, -quadSize.y);
            vertices[vIndex1] = pos + Quaternion.Euler(0, 0, rot) * new Vector3(-quadSize.x, quadSize.y);
            vertices[vIndex2] = pos + Quaternion.Euler(0, 0, rot) * new Vector3(quadSize.x, quadSize.y);
            vertices[vIndex3] = pos + Quaternion.Euler(0, 0, rot) * new Vector3(+quadSize.x, -quadSize.y);
        }
        else
        {
            vertices[vIndex0] = pos + Quaternion.Euler(0, 0, rot-180) * quadSize;
            vertices[vIndex1] = pos + Quaternion.Euler(0, 0, rot-270) * quadSize;
            vertices[vIndex2] = pos + Quaternion.Euler(0, 0, rot-0) * quadSize;
            vertices[vIndex3] = pos + Quaternion.Euler(0, 0, rot-90) * quadSize;
        }

        UVCoords uvc = GetUVCoord(idx, animIdx);
        uv[vIndex0] = uvc.uv00;
        uv[vIndex1] = new Vector2(uvc.uv00.x, uvc.uv11.y);
        uv[vIndex2] = uvc.uv11;
        uv[vIndex3] = new Vector2(uvc.uv11.x, uvc.uv00.y);

        int tIndex = quadIndex * 6;
        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex1;
        triangles[tIndex + 2] = vIndex2;
        triangles[tIndex + 3] = vIndex0;
        triangles[tIndex + 4] = vIndex2;
        triangles[tIndex + 5] = vIndex3;

        updateTriangles = true;
        updateUv = true;
        updateVertices = true;
    }

    private UVCoords GetUVCoord(int idx, int animIdx)
    {
        float startX = (float)animIdx / maxSheetCount;
        float startY = (float)idx / sheetCount.Length;

        float width = 1f / maxSheetCount;
        float height = 1f / sheetCount.Length;

        UVCoords uvc = new UVCoords()
        {
            uv00 = new Vector2(startX, startY),
            uv11 = new Vector2(startX + width, startY + height)
        };
        return uvc;
    }

    private void LateUpdate()
    {
        if (updateVertices)
        {
            mesh.vertices = vertices;
            updateVertices = false;
        }
        if (updateUv)
        {
            mesh.uv = uv;
            updateUv = false;
        }
        if (updateTriangles)
        {
            mesh.triangles = triangles;
            updateTriangles = false;
        }
    }

    public void DestoryQuad(int index)
    {
        int vIndex0 = 4 * index;
        int vIndex1 = vIndex0 + 1;
        int vIndex2 = vIndex0 + 2;
        int vIndex3 = vIndex0 + 3;

        vertices[vIndex0] = Vector3.zero;
        vertices[vIndex1] = Vector3.zero;
        vertices[vIndex2] = Vector3.zero;
        vertices[vIndex3] = Vector3.zero;

        updateVertices = true;
    }

    public void DestroyAllQuad()
    {
        Array.Clear(vertices, 0, vertices.Length);
        quadIndex = 0;
        updateVertices = true;
    }


}
