using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState {  Idle, Trace, Attack, RunAway}
    public EnemyState state = EnemyState.Idle;

    public float moveSpeed = 2f;
    public float traceRange = 15f;
    public float attackRange = 6f;
    public float attackCooldown = 1.5f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    private float currentHP;
    public float maxHP = 5f;
    private Transform player;
    private float lastAttackTime;
    public Slider hpSlider;
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHP = maxHP;
        lastAttackTime = -attackCooldown;
        hpSlider.value = 1f;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        switch (state)
        {
            case EnemyState.Idle:
                if (dist < traceRange)
                    state = EnemyState.Trace;
                break;

            case EnemyState.Trace:
                if (dist < attackRange)
                    state = EnemyState.Attack;
                else if (dist > traceRange)
                    state = EnemyState.Idle;
                else
                    TracePlayer();
                break;

            case EnemyState.Attack:
                if (dist > attackRange)
                    state = EnemyState.Trace;
                else
                    AttackPlayer();
                break;

            case EnemyState.RunAway:
                    Runaway();
                if (dist > traceRange)
                    state = EnemyState.Idle;
                break;
        }
    }

    void TracePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.LookAt(player.position);
    }

    void AttackPlayer()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            transform.LookAt(player.position);
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
            if (ep != null)
            {
                Vector3 dir = (player.position - firePoint.position).normalized;
                ep.SetDirection(dir);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        hpSlider.value = (float)currentHP / maxHP;
        if (currentHP <= maxHP * 0.2)
        {
            state = EnemyState.RunAway;
        }
        currentHP = Mathf.Max(currentHP, 0);

        if (currentHP == 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

    void Runaway()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

    }

    

}
