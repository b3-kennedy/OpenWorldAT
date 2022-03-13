using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    LineRenderer line;
    public Transform start;
    public Transform cam;
    RaycastHit hit;
    public float damage;
    public float damageCooldown;
    float dmgTimer;
    bool doDamage = true;
    bool startTimer;
    bool shoot = false;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void LateUpdate()
    {
        if (shoot)
        {
            if (Physics.Raycast(cam.position, cam.transform.TransformDirection(Vector3.forward), out hit, 50))
            {
                line.SetPosition(0, start.position);
                line.SetPosition(1, hit.point);
                if (hit.collider.tag == "enemy" && doDamage)
                {
                    Debug.Log(hit.collider);
                    hit.collider.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
                    doDamage = false;
                    startTimer = true;
                    dmgTimer = damageCooldown;
                }
            }
            else
            {
                line.SetPosition(0, start.position);
                line.SetPosition(1, start.transform.TransformDirection(Vector3.forward) * 50 + transform.position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {



        if (Input.GetButtonDown("Fire1"))
        {
            line.enabled = true;
        }


        if (startTimer)
        {
            dmgTimer -= Time.deltaTime;
            if (dmgTimer <= 0)
            {
                doDamage = true;
                startTimer = false;
            }
        }

        if (Input.GetButton("Fire1"))
        {
            shoot = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            line.enabled = false;
            shoot = false;
        }
    }
}
