using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Player : Character
{
    [SerializeField] CharacterController controller;
    [SerializeField] GameInput gameInput;
    [SerializeField] LockOn lockOn;

    public float moveSpeed = 2f;
    private void Movement()
    {
        Vector3 camForward = Camera.main.transform.forward.normalized;
        Vector3 camRight = Camera.main.transform.right.normalized;
        camForward.y = 0;
        camRight.y = 0;

        Vector3 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movDir = (camForward * inputVector.y + camRight * inputVector.x).normalized;

        if (movDir.magnitude >= 0.1f)
        {
            transform.forward = Vector3.Slerp(transform.forward, movDir, Time.deltaTime * 10f);
            movDir.y = 0;
            controller.Move(movDir * moveSpeed * Time.deltaTime);

            animator.SetFloat("VelX", inputVector.x);
            animator.SetFloat("VelZ", inputVector.y);
        }
        else
        {
            animator.SetFloat("VelX", 0);
            animator.SetFloat("VelZ", 0);
        }

        if (lockOn != null && lockOn.IsLockedOn && lockOn.CurrentTarget != null)
        {
            // Combat Strafe: Stare at the enemy
            Vector3 dirToTarget = (lockOn.CurrentTarget.position - transform.position).normalized;
            dirToTarget.y = 0;
            transform.forward = Vector3.Slerp(transform.forward, dirToTarget, Time.deltaTime * 15f);
        }
        else if (movDir.magnitude >= 0.1f)
        {
            transform.forward = Vector3.Slerp(transform.forward, movDir, Time.deltaTime * 10f);
        }
    }
    public override void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(1).IsName("MeleeAttack_OneHanded"))
        {
            hasDealtDamageThisSwing = false;
        }

        Movement();
        if (Input.GetMouseButtonDown(0)) // Left Click
        {
            Attack();
        }

        if (Input.GetMouseButtonDown(1)) // Right Click Pressed
        {
            isBlocking = true;
            blockStartTime = Time.time; // Start the parry timer
            animator.SetBool("IsBlocking", true);
        }
        else if (Input.GetMouseButtonUp(1)) // Right Click Released
        {
            isBlocking = false;
            animator.SetBool("IsBlocking", false);
        }

        //Debug.Log("Animator IK Value: " + animator.GetFloat("IK_Weight"));
        combatRig.weight = animator.GetFloat("IK_Weight");

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Attack()
    {
        // This fires the trigger you just set as a condition
        animator.SetTrigger("Attack");
    }
}
