using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    public float movespeed = 2f;
    private float currentHealth;
    public float maxHealth = 5f;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * movespeed * Time.deltaTime;
        transform.LookAt(player.position);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth == 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

    

}
