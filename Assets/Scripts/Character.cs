using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    private float attackDamage = 30f;
    public bool hasDealtDamageThisSwing = false;
    public Slider healthBar;

    public Weapon weapon;
    public Animator animator;
    public Rig combatRig;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask targetLayers;

    public GameObject parryVFXPrefab;
    public Transform parryVFXSpawnPoint;

    public bool isBlocking = false;
    public float blockStartTime;
    public float parryWindow = 0.25f; // You have 0.25 seconds to get a perfect parry
    [Range(0, 1)] public float blockDamageMitigation = 0.2f; // Taking only 20% damage on a normal block

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
            victim.TakeDamage(attackDamage, this);
            hasDealtDamageThisSwing = true;
            //Debug.Log($"{gameObject.name} landed a DEEP hit on {victim.name}!");
        }
    }

    public virtual void TakeDamage(float damage, Character attacker)
    {
        if (currentHealth <= 0) return;

        if (isBlocking)
        {
            // Check if the block was timed perfectly
            if (Time.time - blockStartTime <= parryWindow)
            {
                SuccessfulParry(attacker);
                return; // Take 0 damage, exit function early
            }
            else
            {
                // Normal Block
                damage *= blockDamageMitigation;
                Debug.Log($"{gameObject.name} BLOCKED! Reduced damage to {damage}");
                // (Optional) Trigger a small 'block recoil' animation here
            }
        }

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

        if (healthBar != null) healthBar.value = currentHealth / maxHealth;

        //if (currentHealth <= 0) Die();
    }

    private void SuccessfulParry(Character attacker)
    {
        Debug.Log($"{gameObject.name} executed a PERFECT PARRY!");

        // (Polish Phase) Play a loud clang sound and spawn sparks here!
        if (parryVFXPrefab != null && parryVFXSpawnPoint != null)
        {
            GameObject vfx = Instantiate(parryVFXPrefab, parryVFXSpawnPoint.position, Quaternion.identity);
            Destroy(vfx, 1f);
        }

        if (attacker != null)
        {
            attacker.GetStunned();
        }
    }

    public virtual void GetStunned()
    {
        // Tell the attacker to play the StunnedLoop animation you have in your folder
        animator.SetTrigger("Stunned");
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
                other.TakeDamage(attackDamage, other);
            }
        }
    }
}
