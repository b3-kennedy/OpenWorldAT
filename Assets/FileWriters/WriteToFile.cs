using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;


public class WriteToFile : MonoBehaviour
{
    public string[] fileNames = new string[100];
    public float scale;
    public float offset;

    // Start is called before the first frame update
    void Start()
    {

        


        for (int i = 0; i < fileNames.Length; i++)
        {
            string path = "Assets/Resources/" + i.ToString() + ".txt";
            StreamWriter writer = new StreamWriter(path, true);
            for (int x = 0; x <= GetComponent<MeshGenerator>().xSize; x++)
            {
                for (int z = 0; z <= GetComponent<MeshGenerator>().zSize; z++)
                {

                    //float y = Mathf.PerlinNoise(vertexToWorld(new Vector3(fileNames[0][0] * 200 + x, 0, fileNames[0][1] * 200 + z)).x * 0.01f, vertexToWorld(new Vector3(fileNames[0][0] * 200 + x, 0, fileNames[0][1] * 200 + z)).z * 0.01f);
                    //writer.WriteLine(y.ToString());
                    //writer.Close();

                }
            }

        }

        //for (int i = 0; i < fileNames.Length; i++)
        //{

        //}

    }

    Vector3 vertexToWorld(Vector3 vert)
    {
        Matrix4x4 localToWorld = transform.localToWorldMatrix;
        Vector3 worldPos = localToWorld.MultiplyPoint3x4(vert);
        return worldPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
