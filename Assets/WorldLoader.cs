using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.AI.Navigation;

//[ExecuteInEditMode]
public class WorldLoader : MonoBehaviour
{
    public Transform worldPos;
    public Material mat;
    public GameObject cube;
    public GameObject persistentChunksObj;
    GameObject[] meshes = new GameObject[100];
    public List<GameObject> chunksInScene;
    public List<Vector2> activeChunks;
    public List<Vector2> deactivatedChunks;
    public List<Vector2> persistentChunks;
    public List<GameObject> trees;
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
        for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
        {
            meshes[i] = gameObject.transform.GetChild(0).GetChild(i).gameObject;
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
                    newMesh.transform.SetParent(worldPos.GetChild(0));
                    newMesh.transform.position = new Vector3(meshes[i].transform.localPosition.x, 0, meshes[i].transform.localPosition.z);
                    chunksInScene.Add(newMesh);
                    activeChunks.Add(new Vector2(meshes[i].transform.localPosition.x / 200, meshes[i].transform.localPosition.z / 200));

                }

            }
        }
        yield return null;
    }
    // Update is called once per frame

    public void AddPersistentChunks()
    {
        for (int j = 0; j < persistentChunks.Count; j++)
        {
            if (!activeChunks.Contains(persistentChunks[j]))
            {
                GameObject persistentChunk = meshGen.CreateChunk(persistentChunks[j]);
                persistentChunk.transform.SetParent(worldPos.GetChild(2));
                persistentChunk.transform.position = new Vector3(persistentChunks[j].x * 200, 0, persistentChunks[j].y * 200);
                activeChunks.Add(persistentChunks[j]);
            }
            else
            {
                GetChunkAt(persistentChunks[j]).transform.parent = worldPos.GetChild(2);
            }
            //chunksInScene.Add(GetChunkAt(persistentChunks[j]));
        }
        worldPos.GetChild(2).gameObject.AddComponent<NavMeshSurface>();
        worldPos.GetChild(2).gameObject.GetComponent<NavMeshSurface>().collectObjects = CollectObjects.Children;
        worldPos.GetChild(2).gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public void ClearPersistentChunks()
    {
        for (int i = 0; i < transform.GetChild(2).childCount; i++)
        {
            if(Vector3.Distance(transform.GetChild(2).GetChild(i).position, player.transform.position) > chunkViewDistance * 200)
            {
                Destroy(transform.GetChild(2).GetChild(i).gameObject);

            }


        }
        persistentChunks.Clear();
    }

    void Update()
    {
        chunksInScene.RemoveAll(GameObject => GameObject == null);
        trees.RemoveAll(GameObject => GameObject == null);

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
            if(Vector3.Distance(player.transform.position, new Vector3(chunk.x * 200, 0, chunk.y * 200)) > chunkViewDistance * 200 && !persistentChunks.Contains(chunk))
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
                if (trees.Count > 0)
                {
                    foreach (var tree in trees)
                    {
                        if (chunk == CalculateChunkPosition(tree))
                        {
                            Destroy(tree);
                        }
                    }
                }

            }
        }


        //if(trees.Count > 0)
        //{
        //    foreach (var tree in trees)
        //    {
        //        if (deactivatedChunks.Contains(CalculateChunkPosition(tree)))
        //        {
        //            Destroy(tree);
        //        }
        //    }
        //}


    }

    Vector2 CalculatePlayerPosition()
    {
        Vector2 playerPos = new Vector2(Mathf.Round(player.transform.position.x / 200), Mathf.Round(player.transform.position.z / 200));
        return playerPos;
    }

    public Vector2 CalculateChunkPosition(GameObject chunk)
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
