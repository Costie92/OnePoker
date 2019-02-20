using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMesh : MonoBehaviour {

    public Material material;
    private Vector2[] CardUV;
    // Use this for initialization
    void Start()
    {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        vertices[0] = new Vector3(0, 1);
        vertices[1] = new Vector3(1, 1);
        vertices[2] = new Vector3(0, 0);
        vertices[3] = new Vector3(1, 0);

        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(1, 0);
        /*
        int CardX = 0;
        int CardY = 450;
        int CardWidth = 310;
        int CardHeight = 450;
        int textureWidth = 4000;
        int textureHeight = 1812;

        uv[0] = ConvertPixelsToUVCoordinates(CardX, CardY + CardHeight, textureWidth, textureHeight);
        uv[1] = ConvertPixelsToUVCoordinates(CardX + CardWidth, CardY + CardHeight, textureWidth, textureHeight);
        uv[2] = ConvertPixelsToUVCoordinates(CardX, CardY, textureWidth, textureHeight);
        uv[3] = ConvertPixelsToUVCoordinates(CardX + CardWidth, CardY, textureWidth, textureHeight);
        */
        // card x 310 y 450
        CardUV = GetUVRectangleFromPixels(310, 1350, 310, 450, 4000, 1812);
        ApplyUVToUVArray(CardUV, ref uv);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 3;

        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        GameObject gameObject = new GameObject("Mesh", typeof(MeshFilter), typeof(MeshRenderer));
        gameObject.transform.localScale = new Vector3(30, 30, 1);
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
        gameObject.GetComponent<MeshRenderer>().material = material;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Vector2 ConvertPixelsToUVCoordinates(int x, int y, int textureWidth, int textureHeight)
    {
        return new Vector2((float)x / textureWidth, (float)y / textureHeight);
    }
    public Vector2[] GetUVRectangleFromPixels(int x, int y, int width, int height, int textureWidth, int textureHeight)
    {
        return new Vector2[] {
            /* 0, 1
             * 1, 1
             * 0, 0
             * 1, 0
             * */
            ConvertPixelsToUVCoordinates(x, y + height, textureWidth, textureHeight),
            ConvertPixelsToUVCoordinates(x + width, y+height, textureWidth, textureHeight),
            ConvertPixelsToUVCoordinates(x, y, textureWidth, textureHeight),
            ConvertPixelsToUVCoordinates(x + width, y, textureWidth, textureHeight),
        };
    }
    public void ApplyUVToUVArray(Vector2[] uv, ref Vector2[] mainUV)
    {
        //if (uv == null || uv.Length < 4 || mainUV == null || mainUV.Length < 4) throw new System.Exception();
        mainUV[0] = uv[0];
        mainUV[1] = uv[1];
        mainUV[2] = uv[2];
        mainUV[3] = uv[3];
    }
}