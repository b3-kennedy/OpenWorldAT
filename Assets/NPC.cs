using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class NPC : MonoBehaviour
{

    public int status;
    // Start is called before the first frame update
    void Start()
    {
        ReadFile();
        if(status == 1 || status == 2)
        {
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(status == 1)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
        }
        if(status == 2)
        {
            transform.GetChild(1).gameObject.GetComponent<TextMeshPro>().text = "QUEST COMPLETE";
        }
    }

    void ReadFile()
    {
        string path = "Assets/Resources/ObjectData/" + "NPCData" + ".txt";

        string[] lines = System.IO.File.ReadAllLines(path);
        foreach (var line in lines)
        {
            string[] split = line.Split(':');
            status = int.Parse(split[1]);
            
        }
        Debug.Log(status);
    }

    public void UpdateFile()
    {
        string path = "Assets/Resources/ObjectData/" + "NPCData" + ".txt";
        
        string[] lines = System.IO.File.ReadAllLines(path);
        foreach (var line in lines)
        {
            string[] split = line.Split(':');
            int newStatus = int.Parse(split[1]) + 1;
            string newString = split[0] + ":" + newStatus.ToString() + ":" + split[2].ToString() + ":" + split[3].ToString();
            Debug.Log(newString);
            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(newString);
            writer.Close();
        }

        ReadFile();

    }
}
