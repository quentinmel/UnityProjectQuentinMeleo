using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAIController : MonoBehaviour
{
    public NavMeshAgent navAgent;

    public Transform[] targetPoints;
    public Transform playerGameObject;
    public float detectionRadius = 15f;
    public float attackRadius = 3f;
    public float attackCooldown = 4f;

    private int currentTargetIndex = 0;
    public Animator animator;
    private bool canAttack = true;

    void Start()
    {
        if (targetPoints.Length > 0)
        {
            navAgent.SetDestination(targetPoints[currentTargetIndex].position);
        }
    }

    private void Update()
    {
        if (targetPoints.Length == 0)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerGameObject.position);

        if (distanceToPlayer <= attackRadius && canAttack)
        {
            animator.SetBool("attack", true);
            navAgent.isStopped = true;
            StartCoroutine(ApplyDamageAfterDelay());
        }
        else if (distanceToPlayer <= detectionRadius)
        {
            navAgent.isStopped = false;
            navAgent.SetDestination(playerGameObject.position);
        }
        else
        {
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                currentTargetIndex = (currentTargetIndex + 1) % targetPoints.Length;
                navAgent.SetDestination(targetPoints[currentTargetIndex].position);
            }
        }

        if (distanceToPlayer > attackRadius)
        {
            animator.SetBool("attack", false);
        }
    }

    private IEnumerator ApplyDamageAfterDelay()
    {
        canAttack = false;
        PlayerController player = playerGameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(1);
        }

        navAgent.isStopped = false;
        yield return new WaitForSeconds(attackCooldown);
        animator.SetBool("attack", false);
        canAttack = true;
    }
}
