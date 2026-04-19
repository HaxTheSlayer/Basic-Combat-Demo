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
        Vector3 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 movDir = new Vector3(inputVector.x, 0, inputVector.y);

        if (movDir.magnitude >= 0.1f)
        {
            //transform.forward = Vector3.Slerp(transform.forward, movDir, Time.deltaTime * 10f);
            transform.forward = movDir;
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
    }
}
