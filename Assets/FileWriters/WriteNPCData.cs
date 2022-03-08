using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WriteNPCData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets/Resources/ObjectData/" + "NPCData" + ".txt";
        StreamWriter writer = new StreamWriter(path);
        writer.WriteLine(gameObject.name + ":" + "0" + ":" + transform.position.x.ToString() + "," + transform.position.y.ToString() +
            "," + transform.position.z.ToString() + ":" + transform.rotation.x.ToString() + "," + transform.rotation.y.ToString() + "," + transform.rotation.z.ToString() + "," + transform.rotation.w.ToString());
        writer.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
