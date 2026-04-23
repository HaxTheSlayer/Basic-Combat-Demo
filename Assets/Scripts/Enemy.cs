using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] NavMeshAgent navMeshAgent;
    public GameObject lockOnIcon;
    public Player player;
    public float chaseRange = 10f;

    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    public override void Awake()
    {
        base.Awake();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.stoppingDistance = attackRange;
    }

    private void Update()
    {
        if (isDead) return;

        if (navMeshAgent != null)
        {
            if (animator.GetCurrentAnimatorStateInfo(1).IsName("Death"))
            {
                // Only disable the agent once to avoid repeated calls
                if (navMeshAgent.isActiveAndEnabled)
                {
                    navMeshAgent.isStopped = true;
                    navMeshAgent.velocity = Vector3.zero;
                    navMeshAgent.enabled = false; // Disable safely instead of destroying
                }
                return; // Exit early: no chasing, no attacking
            }

            if (animator.GetCurrentAnimatorStateInfo(1).IsName("StunnedLoop"))
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.velocity = Vector3.zero; // Keep them planted
                return; // EXIT the Update loop early! No chasing, no attacking.
            }

            if (!navMeshAgent.isActiveAndEnabled) return;

            if (!animator.GetCurrentAnimatorStateInfo(1).IsName("MeleeAttack_OneHanded"))
            {
                hasDealtDamageThisSwing = false;
            }
            combatRig.weight = animator.GetFloat("IK_Weight");

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= attackRange)
            {
                AttackState();
            }
            else if (distanceToPlayer <= chaseRange)
            {
                ChasePlayer();
            }
            else
            {
                Idle();
            }
        }
    }

    void AttackState()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;
        animator.SetFloat("VelZ", 0);

        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.forward = Vector3.Slerp(transform.forward, new Vector3(direction.x, 0, direction.z), Time.deltaTime * 10f);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("Attack");
            lastAttackTime = Time.time;
            // HitTarget() will be called by your Animation Event now!
        }
    }

    void Idle() 
    {
        navMeshAgent.isStopped = true;
        animator.SetFloat("VelZ", 0);
    }

    void ChasePlayer() 
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.transform.position);
        animator.SetFloat("VelZ", 1);
    }
}
