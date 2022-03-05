using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WorldLoader : MonoBehaviour
{
    public Transform worldPos;
    public Material mat;
    public GameObject cube;
    GameObject[] meshes = new GameObject[100];
    public List<GameObject> chunksInScene;
    public List<Vector2> activeChunks;
    public List<Vector2> deactivatedChunks;
    public GameObject player;
    public GameObject prevChunk;
    public int chunkViewDistance;
    GameObject mesh;
    MeshGenerator meshGen;
    public List<GameObject> view;


    // Start is called before the first frame update
    void Start()
    {

        meshGen = GetComponent<MeshGenerator>();
        if (!Directory.Exists(Application.dataPath + "/Terrain"))
        {
            Debug.Log("doesnt exist");
        }
        else
        {
            print("exists");
        }
        mesh = gameObject;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            meshes[i] = gameObject.transform.GetChild(i).gameObject;
        }

    }

    IEnumerator CreateChunk()
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            if (Vector3.Distance(player.transform.position, meshes[i].transform.localPosition) < chunkViewDistance * 200)
            {
                if (!activeChunks.Contains(new Vector2(meshes[i].transform.localPosition.x / 200, meshes[i].transform.localPosition.z / 200)))
                {
                    if (deactivatedChunks.Contains(new Vector2(meshes[i].transform.localPosition.x / 200, meshes[i].transform.localPosition.z / 200)))
                    {
                        deactivatedChunks.Remove(new Vector2(meshes[i].transform.localPosition.x / 200, meshes[i].transform.localPosition.z / 200));
                    }
                    GameObject newMesh = meshGen.CreateChunk(new Vector2((meshes[i].transform.localPosition.x / 200)-1, (meshes[i].transform.localPosition.z / 200)-1));
                    newMesh.transform.SetParent(worldPos);
                    newMesh.transform.position = new Vector3(meshes[i].transform.localPosition.x, 0, meshes[i].transform.localPosition.z);
                    chunksInScene.Add(newMesh);
                    activeChunks.Add(new Vector2(meshes[i].transform.localPosition.x / 200, meshes[i].transform.localPosition.z / 200));

                }

            }
        }
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        chunksInScene.RemoveAll(GameObject => GameObject == null);

        //StartCoroutine(CreateChunk());

        for (int i = 0; i < meshes.Length; i++)
        {
            if (Vector3.Distance(player.transform.position, meshes[i].transform.localPosition) < chunkViewDistance * 200)
            {
                if (!activeChunks.Contains(new Vector2(meshes[i].transform.localPosition.x / 200, meshes[i].transform.localPosition.z / 200)))
                {
                    if (deactivatedChunks.Contains(new Vector2(meshes[i].transform.localPosition.x / 200, meshes[i].transform.localPosition.z / 200)))
                    {
                        deactivatedChunks.Remove(new Vector2(meshes[i].transform.localPosition.x / 200, meshes[i].transform.localPosition.z / 200));
                    }
                    GameObject newMesh = meshGen.CreateChunk(new Vector2(meshes[i].transform.localPosition.x / 200, meshes[i].transform.localPosition.z / 200));
                    newMesh.transform.SetParent(worldPos);
                    newMesh.transform.position = new Vector3(meshes[i].transform.localPosition.x, 0, meshes[i].transform.localPosition.z);
                    chunksInScene.Add(newMesh);
                    activeChunks.Add(new Vector2(meshes[i].transform.localPosition.x / 200, meshes[i].transform.localPosition.z / 200));

                }

            }
        }



        foreach (var chunk in activeChunks)
        {
            if(Vector3.Distance(player.transform.position, new Vector3(chunk.x * 200, 0, chunk.y * 200)) > chunkViewDistance * 200)
            {
                deactivatedChunks.Add(chunk);
            }
        }

        if(deactivatedChunks.Count > 0)
        {
            foreach (var chunk in deactivatedChunks)
            {
                Destroy(GetChunkAt(chunk));
                activeChunks.Remove(chunk);
            }
        }

    }

    Vector2 CalculatePlayerPosition()
    {
        Vector2 playerPos = new Vector2(Mathf.Round(player.transform.position.x / 200), Mathf.Round(player.transform.position.z / 200));
        return playerPos;
    }

    Vector2 CalculateChunkPosition(GameObject chunk)
    {
        Vector2 chunkPos = new Vector2(Mathf.Round(chunk.transform.position.x / 200), Mathf.Round(chunk.transform.position.z / 200));
        return chunkPos;
    }

    GameObject GetChunkAt(Vector2 pos)
    {
        foreach (var chunk in chunksInScene)
        {
            if(CalculateChunkPosition(chunk) == pos)
            {
                return chunk;
            }
        }
        //print("NO CHUNK AT" + " (" + pos + ")");
        return null;
    }


    GameObject GetChunkInMesh(Vector2 pos)
    {
        foreach (var chunk in meshes)
        {
            if (CalculateChunkPosition(chunk) == pos)
            {
                return chunk;
            }
        }
        print("NO CHUNK AT" + " (" + pos + ")");
        return null;
    }


    List<GameObject> MeshInViewDistance()
    {
        int realViewDis = chunkViewDistance * 200;
        List<GameObject> chunks = new List<GameObject>();
        foreach (var chunk in meshes)
        {
            if (Vector3.Distance(new Vector3(player.transform.position.x, 0, player.transform.position.z), new Vector3(chunk.transform.position.x, 0, chunk.transform.position.z)) <= realViewDis)
            {
                chunks.Add(chunk);
            }
        }
        return chunks;
    }
}
