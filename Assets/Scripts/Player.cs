using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] GameInput gameInput;
    [SerializeField] Rig combatRig;

    public float moveSpeed = 2f;
    [SerializeField] Transform attackPoint; 
    public float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers; 

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
        Debug.Log("Animator IK Value: " + animator.GetFloat("IK_Weight"));
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
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Hit: " + enemy.name);
            // logic for durability loss and enemy damage goes here
        }
    }
}
