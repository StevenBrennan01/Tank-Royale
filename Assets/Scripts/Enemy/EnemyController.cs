using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private ProjectileHandler projectileHandler_SCR;

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

    private void Awake()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        projectileHandler_SCR = GetComponent<ProjectileHandler>();

        enemyAgent.updateRotation = false;
        enemyAgent.updateUpAxis = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;

            EnemyEngage();
            projectileHandler_SCR.TankFired();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    private void EnemyEngage()
    {
        if (playerInRange) //&& Vector2.Distance(transform.position, player.position) >= stoppingDistance)
        {
            //Starts coroutine to move tank
            enemyMoving = StartCoroutine(EnemyMoving_CR());


            //Rotates tank tower to player position
            Vector3 towerRot = (enemyTarget.transform.position - enemyTower.transform.position);
            float targetPoint = Mathf.Atan2(towerRot.y, towerRot.x) * Mathf.Rad2Deg - 90f;

            Quaternion towerRotation = Quaternion.Euler(0, 0, targetPoint);
            enemyTower.transform.rotation = Quaternion.Slerp(enemyTower.transform.rotation, towerRotation, towerRotateSpeed);
        }
    }

    //COROUTINES FOR MOVING AND ATTACKING
    private IEnumerator EnemyMoving_CR()
    {
        enemyAgent.SetDestination(enemyTarget.transform.position);
        yield return null;
    }
}