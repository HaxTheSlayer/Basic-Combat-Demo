using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] Animator animator;
    [SerializeField] GameInput gameInput;

    public float moveSpeed = 2f;
    
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
        //if (Input.GetKeyDown(KeyCode.Space))
        //{

        //}
    }
}
