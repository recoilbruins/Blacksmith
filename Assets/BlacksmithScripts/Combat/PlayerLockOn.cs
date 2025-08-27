using BlackSmithInput;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Cinemachine.Samples;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerLockOn : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private LookAtTarget playerLookAtTarget;
    [SerializeField] private CinemachineCamera normalPlayerCamera; // Reference to the Cinemachine camera for lock-on functionality
    [SerializeField] private CinemachineCamera lockOnCamera; // Reference to the Cinemachine camera for lock-on functionality
    [SerializeField] private CinemachineInputAxisController cinemachineInputAxisController; // Reference to the Cinemachine input axis controller for lock-on functionality
    [SerializeField] private PlayerMovement playerMovement; // Reference to the PlayerMovement script for player movement functionality

    //[SerializeField] private PerfectLookAt perfectLookAt; // Reference to the PerfectLookAt script for player look-at functionality

    [Space(5f)]
    [Header("Lock On References")]
    [SerializeField] private Transform player; // Reference to the player transform
    [SerializeField] private Transform target; // The target to lock onto, e.g., an enemy

    [SerializeField] private RaycastHit[] enemiesHit; // Array to store raycast hits
    [SerializeField] private LayerMask enemyLayer; // Layer mask to filter for enemies

    [Space(5f)]
    [Header("Test Values")]
    [SerializeField] private float sphereCastRadius = 25f; // Radius of the sphere cast
    [SerializeField] private float maxDistance = 25f; // Maximum distance for lock-on to stay enabled
    [SerializeField] private float switchCooldown = 0.5f; // Cooldown time for switching targets
    private float switchTimer = 0f; // Timer for switching targets

    private bool isLockedOn = false; // Flag to check if the player is locked onto a target

    private void Update()
    {
        LockOn(); // Check for lock-on input and update the target
    }


    private void LockOn()
    {
        if (InputManager.instance.isLockOnPressed)
        {
            InputManager.instance.isLockOnPressed = false; // Reset the lock-on input

            isLockedOn = !isLockedOn; // Toggle the lock-on state

            if (isLockedOn)
            {
                TryLockOn(); // If locking on, find the nearest target
            }
            else
            {
                UnlockOn(); // If unlocking, reset the target
            }
        }
        if (isLockedOn)
        {
            OnCameraMovement(); // Handle camera movement while locked on
            
            if (Vector3.Distance(player.position, target.position) > maxDistance)
            {
                UnlockOn(); // If the target is too far away, unlock
            }

        }
    }
    private void TryLockOn()
    {
        // Perform a sphere cast to find the nearest target within the specified radius
        target = FindNearestTarget(sphereCastRadius, 60f); // 60 degrees view angle
        if (target != null)
        {
            normalPlayerCamera.gameObject.SetActive(false); // Disable the normal player camera
            lockOnCamera.gameObject.SetActive(true); // Enable the lock-on camera
            lockOnCamera.LookAt = target; // Set the lock-on camera to look at the target
            playerLookAtTarget.Target = target; // Set the player look-at target to the locked-on target
            playerMovement.isLockedOn = isLockedOn; // Update the player movement lock-on state
            playerMovement.lockOnTarget = target; // Set the lock-on target for player movement
            //perfectLookAt.enabled = true; // Enable the PerfectLookAt script for player look-at functionality
            //perfectLookAt.m_TargetObject = target.gameObject; // Set the PerfectLookAt target to the locked-on target
            Debug.Log("Lock-on enabled: " + target.name);
        }
        else
        {
            Debug.Log("No targets in range for lock-on.");
            isLockedOn = false; // If no target found, reset the lock-on state
        }
    }

    private void UnlockOn()
    {
        // Reset the target and lock-on state
        normalPlayerCamera.gameObject.SetActive(true); // Enable the normal player camera
        lockOnCamera.gameObject.SetActive(false); // Disable the lock-on camera
        playerLookAtTarget.Target = null; // Reset the player look-at target
        playerMovement.isLockedOn = false; // Update the player movement lock-on state
        playerMovement.lockOnTarget = null; // Reset the lock-on target for player movement
        //perfectLookAt.m_TargetObject = null; // Set the PerfectLookAt target to the locked-on target
        //perfectLookAt.enabled = false; // Disable the PerfectLookAt script
        target = null;
        isLockedOn = false;
        Debug.Log("Lock-on disabled");
        //cinemachineCamera.Follow = transform; // Reset the Cinemachine camera to the player's position
    }

    private Transform FindNearestTarget(float maxDistance, float viewAngle)
    {
        Collider[] targetsInView = Physics.OverlapSphere(player.position, maxDistance, enemyLayer);

        Transform nearestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider col in targetsInView)
        {
            Vector3 directionToTarget = (col.transform.position - player.position).normalized;
            float angle = Vector3.Angle(normalPlayerCamera.transform.forward, directionToTarget);

            if (angle < viewAngle / 2f)
            {
                float distance = Vector3.Distance(player.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestTarget = col.transform;
                }
            }
        }

        return nearestTarget;
    }

    private void OnCameraMovement()
    {
        float horizontalInput = InputManager.instance.cameraTargetHorizontal; // Get horizontal camera input

        if (switchTimer <= 0)
        {
            if (horizontalInput > 0.5f)
            {
                SwitchTarget(switchToRight: true); // Switch to the right target
                switchTimer = switchCooldown; // Reset the switch timer
            }
            else if (horizontalInput < -0.5f)
            {
                SwitchTarget(switchToRight: false); // Switch to the left target
                switchTimer = switchCooldown; // Reset the switch timer 
            }
        }
        else
        {
            switchTimer -= Time.deltaTime; // Decrease the switch timer
        }
    }

    private void SwitchTarget(bool switchToRight)
    {
        if (target == null) return; // If no target is locked on, do nothing

        Collider[] targetsInArea = Physics.OverlapSphere(player.position, maxDistance, enemyLayer);
        Transform bestTarget = null;
        float bestScore = -Mathf.Infinity;

        Vector3 toCurrent = (target.position - player.position).normalized;
        Vector3 playerRight = Vector3.Cross(Vector3.up, toCurrent);

        foreach (Collider col in targetsInArea)
        {
            if (col.transform == target) continue;

            Vector3 toOther = (col.transform.position - player.position).normalized;
            float side = Vector3.Dot(toOther, playerRight); // + = right, - = left

            if ((switchToRight && side > 0.1f) || (!switchToRight && side < 0.1f))
            {
                float allignment = Vector3.Dot(toOther, toCurrent);
                if (allignment > bestScore)
                {
                    bestScore = allignment;
                    bestTarget = col.transform;
                }
            }
        }
        if (bestTarget != null)
        {
            target = bestTarget; // Switch to the new target
            playerLookAtTarget.Target = target; // Update the player look-at target
            lockOnCamera.LookAt = target; // Update the lock-on camera to look at the new target
            playerMovement.lockOnTarget = target; // Set the lock-on target for player movement
            Debug.Log("Switched target to: " + target.name);
        }
        else
        {
            Debug.Log("No suitable target found to switch to.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a sphere at the player's position to visualize the lock-on radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sphereCastRadius);
    }
}
