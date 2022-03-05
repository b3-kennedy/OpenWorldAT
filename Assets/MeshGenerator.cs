using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MeshGenerator : MonoBehaviour
{
    GameObject chunkObj;
    Mesh mesh;
    public Material mat;

    Vector3[] vertices;
    int[] triangles;
    public int xSize;
    public int zSize;

    public float scale;
    public float offset;

    MeshCollider meshCol;

    int chunkX;
    int chunkZ;

    // Start is called before the first frame update
    void Start()
    {
        //CreateChunk();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateChunk(new Vector2(0, 0));
            CreateChunk(new Vector2(1, 0));
        }
    }

    public GameObject CreateChunk(Vector2 chunkCoord)
    {

        chunkX = (int)chunkCoord.x;
        chunkZ = (int)chunkCoord.y;

        string name = chunkX.ToString() + chunkZ.ToString();


        chunkObj = new GameObject();
        chunkObj.AddComponent<MeshFilter>();
        chunkObj.AddComponent<MeshRenderer>();
        meshCol = chunkObj.AddComponent<MeshCollider>();

        mesh = new Mesh();
        

        chunkObj.GetComponent<MeshFilter>().mesh = mesh;
        chunkObj.GetComponent<MeshRenderer>().material = mat;
        chunkObj.name = name;

        CreateShape(name);
        UpdateMesh();


        return chunkObj;
    }


    Vector3 vertexToWorld(Vector3 vert)
    {
        Matrix4x4 localToWorld = transform.localToWorldMatrix;
        Vector3 worldPos = localToWorld.MultiplyPoint3x4(vert);
        return worldPos;
    }

    void CreateShape(string fileName)
    {

        string path = "Assets/Resources/" + fileName + ".txt";
        StreamReader reader = new StreamReader(path);
        string[] lines = System.IO.File.ReadAllLines(path);
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = float.Parse(lines[i]);
                //print("normal: " + y + " | " + "abnormal: " + testy);

                vertices[i] = new Vector3(x, y/3, z);
                

                i++;
            }
        }

        reader.Close();


        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {

                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }



    }

    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        mesh.RecalculateNormals();
        meshCol.sharedMesh = chunkObj.GetComponent<MeshFilter>().mesh;

    }
}
