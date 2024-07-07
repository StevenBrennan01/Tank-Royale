using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    private PlayerController tankController_SCR;

    private Animator TankAnimator;

    #region Inspector Header and Spacing
    [Header("                                                    -= Projectile Handler =-")]
    [Space(15)]
    #endregion

    [SerializeField] private GameObject[] projectilePrefabs;
    [SerializeField] private Transform[] projectileSpawnPositions;

    //WILL NEED TO USE THIS WHEN ENABLING NEW BULLETS ETC.
    //private int index;

    [SerializeField] private ParticleSystem shootVFX;

    #region Inspector Comments and Spacing
    [Header("Speed Values")]
    [Space(15)]
    #endregion

    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireDelay;

    [HideInInspector] public bool canFire = true;

    private Vector3 mousePos;

    private void Awake()
    {
        TankAnimator = GetComponent<Animator>();
    }

    public void TankFired()
    {
        if (canFire)
        {
            Vector2 projectileFireDirection = projectileSpawnPositions[0].up; projectileFireDirection.Normalize();

            //GameObject projectileSpawn = Instantiate(projectilePrefabs[0], projectileSpawnPositions[0].position, Quaternion.identity);

            GameObject projectileSpawn = ObjectPoolManager.spawnObject(projectilePrefabs[0], projectileSpawnPositions[0].position, Quaternion.identity);

            if (projectilePrefabs[0].GetComponent<Rigidbody2D>() != null)
            {
                float projectileRotation = Mathf.Atan2(projectileFireDirection.y, projectileFireDirection.x) * Mathf.Rad2Deg;

                projectileSpawn.transform.rotation = Quaternion.Euler(0f, 0f, projectileRotation - 90f);
                projectileSpawn.GetComponent<Rigidbody2D>().AddForce(projectileFireDirection * projectileSpeed, ForceMode2D.Impulse);

                TankAnimator.SetTrigger("GunFiring");

                if (shootVFX != null)
                {
                    shootVFX.Play();
                }
            }

            //                            (INTENSITY, FOR TIME)
            CinemachineShake.Instance.CameraShake(1.5f, .25f);

            canFire = false;
            StartCoroutine(FireDelayCR());
        }

        else return;
    }

    private IEnumerator FireDelayCR()
    {
        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }
}