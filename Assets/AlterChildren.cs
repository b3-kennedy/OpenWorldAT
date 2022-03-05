using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AlterChildren : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] children = new GameObject[transform.childCount];
        for (int i = 0; i < children.Length; i++)
        {
            transform.GetChild(i).position = new Vector3(transform.GetChild(i).position.x - 100, transform.position.y, transform.GetChild(i).position.z - 100);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
