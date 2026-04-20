using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] GameInput gameInput;
    [SerializeField] Rig combatRig;

    public float moveSpeed = 2f;
    [SerializeField] Transform attackPoint; 
    public float attackRange = 0.5f;
    public float attackDamage = 30f;
    [SerializeField] LayerMask enemyLayers;

    public float currentHealth = 100f;
    public float maxHealth = 100f;

    private void Update()
    {
        Vector3 camForward = Camera.main.transform.forward.normalized;
        Vector3 camRight = Camera.main.transform.right.normalized;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movDir = (camForward * inputVector.y + camRight * inputVector.x).normalized;

        if (movDir.magnitude >= 0.1f)
        {
            //transform.forward = Vector3.Slerp(transform.forward, movDir, Time.deltaTime * 10f);
            transform.forward = Vector3.Slerp(transform.forward, movDir, Time.deltaTime * 10f);
            controller.Move(movDir * moveSpeed * Time.deltaTime);

            // Tell the Animator we are walking
            animator.SetFloat("VelX", inputVector.x);
            animator.SetFloat("VelZ", inputVector.y);
        }
        else
        {
            // Tell the Animator we have stopped
            animator.SetFloat("VelX", 0);
            animator.SetFloat("VelZ", 0);
        }
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            Attack();
        }

        combatRig.weight = animator.GetFloat("IK_Weight");

        //if (Input.GetKeyDown(KeyCode.Space))
        //{

        //}
    }

    private void Attack()
    {
        // This fires the trigger you just set as a condition
        animator.SetTrigger("Attack");
    }

    public void HitTarget()
    {
        Debug.Log("HitTarget function was CALLED by the animation.");

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemyCollider in hitEnemies)
        {
            enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Hit: " + enemy.name);
                // For now, wasParried is false because this is a player attack
                enemy.TakeDamage(attackDamage);
                Debug.Log("Dealt damage to enemy!");
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("GetHit"); // Make sure you have this trigger in the Animator!
    
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            // Handle Player Death (e.g., Reload Scene)
            Debug.Log("Player Died!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        // This draws a red sphere in the Scene View so you can see the reach
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
