using UnityEngine;

[System.Serializable]
public class PRS
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public PRS(Vector3 pos, Quaternion rot, Vector3 scl)
    {
        position = pos;
        rotation = rot;
        scale = scl;
    }
}
