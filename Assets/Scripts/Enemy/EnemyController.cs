using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private EnemyProjectileHandler enemyProjectileHandler_SCR;
    private PlayerController playerController_SCR;

    #region Inspector Header and Spacing
    [Header("                                                    -= Enemy Controller =-")]
    [Space(15)]
    #endregion

    [SerializeField] private Transform enemyTarget;

    #region Inspector Header and Spacing
    [Header("Attributes")]
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
        enemyProjectileHandler_SCR = GetComponent<EnemyProjectileHandler>();  
        enemyAgent = GetComponent<NavMeshAgent>();
        playerController_SCR = GetComponent<PlayerController>();

        enemyAgent.updateRotation = false;
        enemyAgent.updateUpAxis = false;
    }

    private void OnEnable()
    {
        enemyTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;

            EnemyEngage();
            enemyProjectileHandler_SCR.EnemyTankFired();
        }
    }

    //ATTEMPTING ROAMING HERE, RANDOM OR WAYPOINTS

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    private void EnemyEngage()
    {
        if (playerInRange)
        {
            //STARTS COROUTINE TO MOVE TANK
            enemyMoving = StartCoroutine(EnemyMoving_CR());

            //ROTATES TANK TOWER TO PLAYER POSITION
            Vector3 towerRot = (enemyTarget.transform.position - enemyTower.transform.position);
            float targetPointForTower = Mathf.Atan2(towerRot.y, towerRot.x) * Mathf.Rad2Deg - 90f;

            Quaternion towerRotation = Quaternion.Euler(0, 0, targetPointForTower);
            enemyTower.transform.rotation = Quaternion.Slerp(enemyTower.transform.rotation, towerRotation, towerRotateSpeed);

            //ROTATING ENEMY HULL TO PLAYER POSITION
            Vector3 hullRot = (enemyTarget.transform.position - enemyHull.transform.position);
            float targetPointForHull = Mathf.Atan2(towerRot.y, towerRot.x) * Mathf.Rad2Deg - 90f;

            Quaternion hullRotation = Quaternion.Euler(0, 0, targetPointForHull);
            enemyHull.transform.rotation = Quaternion.Slerp(enemyTower.transform.rotation, hullRotation, hullRotateSpeed);
        }
    }

    private IEnumerator EnemyMoving_CR()
        {
            enemyAgent.SetDestination(enemyTarget.transform.position);
            yield return null;
        }
}