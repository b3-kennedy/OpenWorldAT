using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class TriggerEvents : MonoBehaviour
{

    public GameObject world;

    bool inTrigger;
    public GameObject item;
    GameObject npc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(npc.GetComponent<NPC>().status == 0)
                {
                    world.GetComponent<WorldLoader>().AddPersistentChunks();
                    world.GetComponent<SpawnEnemies>().Spawn(new Vector3(908.8f, 20.05f, 721.3f));
                    world.GetComponent<SpawnEnemies>().Spawn(new Vector3(903.8f, 20.05f, 721.3f));
                    world.GetComponent<SpawnEnemies>().Spawn(new Vector3(901.8f, 20.05f, 721.3f));
                    world.GetComponent<SpawnEnemies>().Spawn(new Vector3(900.8f, 20.05f, 721.3f));
                    world.GetComponent<SpawnEnemies>().Spawn(new Vector3(910.8f, 20.05f, 721.3f));
                    npc.GetComponent<NPC>().UpdateFile();
                    Debug.Log("interacted");
                }
                if(npc.GetComponent<NPC>().status == 1 && item != null)
                {
                    world.GetComponent<WorldLoader>().ClearPersistentChunks();
                    npc.GetComponent<NPC>().UpdateFile();
                }
                
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            inTrigger = true;
            other.transform.GetChild(0).gameObject.SetActive(true);
            npc = other.gameObject;
        }

        if (other.CompareTag("item"))
        {
            item = other.gameObject;
            other.gameObject.SetActive(false);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            inTrigger = false;
            other.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
