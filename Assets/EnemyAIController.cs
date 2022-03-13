using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{

    NavMeshAgent agent;
    LineRenderer line;
    public Transform dest;
    public float range;
    public float dmgRange;
    public float damage;
    public float damageCooldown;
    float dmgTimer;
    bool doDamage = true;
    bool startTimer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
        dest = GameObject.Find("Player").transform;
        dmgTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, dest.position) <= range)
        {
            agent.destination = dest.position;
        }

        if(Vector3.Distance(transform.position, dest.position) <= dmgRange && doDamage)
        {
            line.enabled = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, dest.position);
            dest.GetComponent<PlayerHealth>().TakeDamage(damage);
            doDamage = false;
            dmgTimer = damageCooldown;
            startTimer = true;
        }
        else
        {
            line.enabled = false;
        }


        if (startTimer)
        {
            dmgTimer -= Time.deltaTime;
            if(dmgTimer <= 0)
            {
                doDamage = true;
                startTimer = false;
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Color newColor = new Color(255, 0, 0, 0.5f);
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, range);
    }
}
