using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{

    NavMeshAgent agent;
    public Transform dest;
    public float range;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        dest = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, dest.position) <= range)
        {
            agent.destination = dest.position;
        }
        
    }

    private void OnDrawGizmos()
    {
        Color newColor = new Color(255, 0, 0, 0.5f);
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, range);
    }
}
