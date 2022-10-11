using UnityEngine;

public class MeshTest : MonoBehaviour
{
    //동적 메쉬 생성

    public Texture tex;
    public Shader shader;

    private void Start()
    {
        Vector3[] vertex = new Vector3[4]
        {
            new Vector3(0,0,0),
            new Vector3(0, 1,0 ),
            new Vector3(1, 1,0 ),
            new Vector3(1, 0,0 )
        };

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0,0),
            new Vector2(0, 1 ),
            new Vector2(1, 1 ),
            new Vector2(1, 0 )
        };

        int[] tri = new int[6] { 0, 1, 2, 0, 2, 3 };

        Mesh mesh = new Mesh();

        mesh.vertices = vertex;
        mesh.uv = uv;
        mesh.triangles = tri;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

        Material mat = new Material(Shader.Find("Standard"));
        //Material mat = new Material(shader);
        mat.SetTexture("_MainTex", tex);
        GetComponent<MeshRenderer>().material = mat;
    }
}
