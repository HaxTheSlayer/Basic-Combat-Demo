using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Enemy : MonoBehaviour
{
    [SerializeField] Rig combatRig;

    public float health = 100f;
    public Transform player;


    public NavMeshAgent agent;
    public float attackRange = 2f;
    public float chaseRange = 10f;

    public float enemyDamage = 10f;
    public Transform enemyAttackPoint; 
    public float enemyAttackRange = 0.5f;
    public LayerMask playerLayer;

    private Animator anim;


    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
        combatRig.weight = anim.GetFloat("IK_Weight");
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0) Die();
    }

    void Idle() { agent.isStopped = true; anim.SetFloat("Speed", 0); }
    void ChasePlayer() { agent.isStopped = false; agent.SetDestination(player.position); anim.SetFloat("Speed", 1); }
    void AttackPlayer() { agent.isStopped = true; anim.SetTrigger("Attack"); }
    public void EnemyHitTarget()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(enemyAttackPoint.position, enemyAttackRange, playerLayer);

        foreach (Collider p in hitPlayer)
        {
            Player playerScript = p.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(enemyDamage);
            }
        }
    }

    void Die() { Destroy(this); }
}
