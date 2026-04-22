using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public event Action<Character> OnSignificantHit;

    [SerializeField] private Character owner;
    [SerializeField] private float depthThreshold = 0.5f; // How "deep" into the cylinder we must be (0 to 1)

    private Collider weaponCollider;

    private void Awake()
    {
        weaponCollider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        // 1. Are we in the attack animation state?
        if (owner.animator.GetCurrentAnimatorStateInfo(1).IsName("MeleeAttack_OneHanded"))
        {
            Character victim = other.GetComponent<Character>();

            // 2. Only hit if it's a Character and not the person holding the sword
            if (victim != null && victim != owner)
            {
                if (CheckOverlapDepth(other))
                {
                    OnSignificantHit?.Invoke(victim);
                }
            }
        }
    }

    private bool CheckOverlapDepth(Collider victimCollider)
    {
        Vector3 victimCenter = victimCollider.bounds.center;

        Vector3 closestPointOnSword = weaponCollider.ClosestPoint(victimCenter);

        victimCenter.y = 0;
        closestPointOnSword.y = 0;

        // Calculate distance from the edge of the sword to the center of the enemy
        float distance = Vector3.Distance(closestPointOnSword, victimCenter);
        float victimRadius = 0.5f; // Standard CharacterController/Capsule radius

        // If distance is less than half the radius, it's a "Significant Hit"
        return distance <= (victimRadius * depthThreshold);
    }
}