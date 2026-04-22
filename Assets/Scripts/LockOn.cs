using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LockOn : MonoBehaviour
{
    public CinemachineVirtualCamera lockOnCamera;

    public float lockOnRadius = 15f;
    public LayerMask targetLayers;
    private GameObject activeLockOnIcon;

    public Transform CurrentTarget { get; private set; }
    public bool IsLockedOn { get; private set; }

    private void Update()
    {
        // You can eventually move this input check to your GameInput script!
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (IsLockedOn)
            {
                ClearLockOn();
            }
            else
            {
                FindTarget();
            }
        }
    }

    private void FindTarget()
    {
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, lockOnRadius, targetLayers);
        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider target in hitTargets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = target.transform;
            }
        }

        if (closestEnemy != null)
        {
            CurrentTarget = closestEnemy;
            IsLockedOn = true;
            Enemy enemyScript = CurrentTarget.GetComponentInParent<Enemy>();
            if (enemyScript != null && enemyScript.lockOnIcon != null)
            {
                activeLockOnIcon = enemyScript.lockOnIcon;
                activeLockOnIcon.SetActive(true);
            }

            lockOnCamera.LookAt = CurrentTarget;

            lockOnCamera.Priority = 20;
        }
    }

    public void ClearLockOn()
    {
        IsLockedOn = false;
        CurrentTarget = null;

        if (activeLockOnIcon != null)
        {
            activeLockOnIcon.SetActive(false);
            activeLockOnIcon = null;
        }

        lockOnCamera.LookAt = null;
        lockOnCamera.Priority = 0;
    }
}

