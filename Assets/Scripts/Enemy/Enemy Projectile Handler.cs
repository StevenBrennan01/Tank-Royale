using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileHandler : MonoBehaviour
{
    private Animator TankAnimator;

    #region Inspector Header and Spacing
    [Header("                                                    -= Projectile Handler =-")]
    [Space(15)]
    #endregion

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform[] projectileSpawnPositions;

    //WILL NEED TO USE THIS WHEN ENABLING NEW BULLETS ETC.
    //private int index;

    [SerializeField] private ParticleSystem shootVFX;

    #region Inspector Header and Spacing
    [Space(15)]
    [Header("                                                    -= Projectile Values =-")]
    [Space(15)]
    #endregion

    [SerializeField] private float projectileSpeed;

    private bool enemyCanFire = true;

    private Coroutine enemyFireDelay_CR;
    [SerializeField] private float fireDelay = .7f;

    private void Awake()
    {
        TankAnimator = GetComponent<Animator>();
    }

    public void EnemyTankFired()
    {
        if (enemyCanFire)
        {
            // === GOING TO NEED TO MAKE THIS WORK WITH MULTIPLE PROJECTILE SPAWN POS
            Vector2 projectileFireDirection = projectileSpawnPositions[0].up;
            projectileFireDirection.Normalize();

            /// INSTANTIATES THE OBJECT FROM THE POOL
            GameObject projectileSpawn = ObjectPoolManager.spawnObject(projectilePrefab, projectileSpawnPositions[0].position, Quaternion.identity);

            if (projectilePrefab.GetComponent<Rigidbody2D>() != null)
            {
                float projectileRotation = Mathf.Atan2(projectileFireDirection.y, projectileFireDirection.x) * Mathf.Rad2Deg;

                //GIVES THE OBJECT ITS ROTATION AND FORCE HERE
                projectileSpawn.transform.rotation = Quaternion.Euler(0f, 0f, projectileRotation - 90f);
                projectileSpawn.GetComponent<Rigidbody2D>().AddForce(projectileFireDirection * projectileSpeed, ForceMode2D.Impulse);

                if (shootVFX != null) { shootVFX.Play(); }
            }

            enemyCanFire = false;

            //                            (INTENSITY, FOR TIME)
            CinemachineShake.Instance.CameraShake(1.5f, .25f);

            // Stop any currently running coroutine
            if (enemyFireDelay_CR != null)
            {
                StopCoroutine(enemyFireDelay_CR);
            }

            enemyFireDelay_CR = StartCoroutine(EnemyFireDelay());
        }
    }

    // ===== COROUTINES =====

    private IEnumerator EnemyFireDelay()
    {
        yield return new WaitForSeconds(fireDelay);
        enemyCanFire = true;
    }
}