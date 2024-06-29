using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent enemyAgent;

    [SerializeField] private Transform enemyTarget;

    private bool playerInRange;
    private bool enemyIsMoving;

    //[SerializeField] private float enemySpeed;
    //[SerializeField] private float stoppingDistance;

    private Coroutine enemyMoving;
    private Coroutine enemyAttacking;

    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();

        enemyAgent.updateRotation = false;
        enemyAgent.updateUpAxis = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
            EnemyMove();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    private void EnemyMove()
    {
        if (playerInRange) //&& Vector2.Distance(transform.position, player.position) >= stoppingDistance)
        {
            enemyMoving = StartCoroutine(EnemyMoving_CR());
        }
    }

        private IEnumerator EnemyMoving_CR()
        {
            enemyAgent.SetDestination(enemyTarget.position);
            enemyIsMoving = true;
            yield return null;
        }

    private void EnemyAttack()
    {
        if (playerInRange)
        {
            //do something, shoot etc.
        }
    }
}
