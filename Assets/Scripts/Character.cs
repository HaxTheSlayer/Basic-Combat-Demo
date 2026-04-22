using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Character : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    private float attackDamage = 30f;
    public bool hasDealtDamageThisSwing = false;

    public Weapon weapon;
    public Animator animator;
    public Rig combatRig;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask targetLayers;

    public virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void Start()
    {
        if (weapon != null)
        {
            // Subscribe to the event: "When the weapon hits, run my DealDamage logic"
            weapon.OnSignificantHit += HandleWeaponHit;
        }
    }

    private void HandleWeaponHit(Character victim)
    {
        // This only runs when the Weapon script fires its event
        if (!hasDealtDamageThisSwing)
        {
            victim.TakeDamage(attackDamage);
            hasDealtDamageThisSwing = true;
            //Debug.Log($"{gameObject.name} landed a DEEP hit on {victim.name}!");
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (currentHealth <= 0) return;

        // 2. Apply damage and clamp to 0 so we don't get negative HP
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log($"{gameObject.name} took {damage} damage. HP: {currentHealth}");

        // 3. Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Only play the flinch animation if they survived the hit
            animator.SetTrigger("GetHit");
        }

        //if (currentHealth <= 0) Die();
    }
    public virtual void Die()
    {
        animator.SetTrigger("Die");
        Debug.Log($"{gameObject.name} has died.");
    }

    public virtual void HitTarget()
    {
        Collider[] hitTargets = Physics.OverlapSphere(attackPoint.position, attackRange, targetLayers);
        foreach (Collider target in hitTargets)
        {
            // Check if the thing we hit has a Character script (Player or Enemy)
            Character other = target.GetComponent<Character>();
            if (other != null)
            {
                other.TakeDamage(attackDamage);
            }
        }
    }
}
