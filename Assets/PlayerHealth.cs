using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject healthBar;
    public float maxHealth;
    public float health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.GetComponent<RectTransform>().localScale = new Vector3(health / maxHealth, healthBar.GetComponent<RectTransform>().localScale.y, healthBar.GetComponent<RectTransform>().localScale.z);
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
    }

}
