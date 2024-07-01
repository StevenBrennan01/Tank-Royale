using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform enemyTarget;

    #region Inspector Header and Spacing
    [Header("-= Enemy Attributes =-")]
    [Space(5)]
    #endregion

    [SerializeField] private GameObject enemyTower;
    [SerializeField] private GameObject enemyHull;

    [SerializeField] private float hullRotateSpeed;
    [SerializeField] private float towerRotateSpeed;

    private bool playerInRange;

    private Rigidbody2D rb;
    private NavMeshAgent enemyAgent;

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
            yield return null;
        }

    private void EnemyAttack()
    {
        if (playerInRange)
        {
            enemyTower.transform.rotation = Quaternion.RotateTowards(enemyTower.transform.rotation, enemyTarget.transform.position, towerRotateSpeed * Time.deltaTime);
            //rotate tower to player position
            //shoot at player
        }
    }
}
