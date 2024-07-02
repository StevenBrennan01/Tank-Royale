using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ProjectileHandler : MonoBehaviour
{
    private PlayerController tankController_SCR;

    private Animator TankAnimator;

    #region Inspector Header and Spacing
    [Header("                                                  -= Projectile Attributes =-")]
    [Space(15)]
    #endregion

    [SerializeField] private GameObject[] projectilePrefabs;
    [SerializeField] private Transform[] projectileSpawnPositions;

    [SerializeField] private ParticleSystem shootVFX;

    //public int index;

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

            GameObject projectileSpawn = Instantiate(projectilePrefabs[0], projectileSpawnPositions[0].position, Quaternion.identity);

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
    }

    private IEnumerator FireDelayCR()
    {
        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }
}