using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent enemyAgent;

    [SerializeField] private Transform player;

    [SerializeField] private bool playerInRange;
    [SerializeField] private bool enemyIsMoving;

    [SerializeField] private float enemySpeed;
    [SerializeField] private float stoppingDistance;

    private Coroutine enemyMoving;
    private Coroutine enemyAttacking;

    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
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
        if (playerInRange && Vector2.Distance(transform.position, player.position) >= stoppingDistance)
        {
            StartCoroutine(TankMoving());
        }
    }

    private IEnumerator TankMoving()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemySpeed * Time.deltaTime);
        enemyIsMoving = true;
        yield return null;
    }

    private IEnumerator TankIdle()
    {
        transform.position = Vector2.zero;
        yield return null;
    }

    private void EnemyAttack()
    {
        if (playerInRange)
        {
            //do something
        }
    }
}
