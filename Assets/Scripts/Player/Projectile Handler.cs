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

    [Space(15)]
    [SerializeField] private float projectileSpeed;

    [HideInInspector] public bool canFire = true;

    public int ammoCount;

    #region Inspector Header and Spacing
    [Header("                                                    -= Projectile Values =-")]
    [Space(15)]
    #endregion

    public int maxAmmo = 5;
    public int minAmmo = 0;

    private Coroutine fireDelay_CR;
    [SerializeField] private float fireDelay;

    private Coroutine reloadDelay_CR;
    [SerializeField] private float reloadRoundDelay = .5f;

    private Vector3 mousePos;

    private void Awake()
    {
        TankAnimator = GetComponent<Animator>();

        ammoCount = maxAmmo;
    }

    public void TankFired()
    {

        if (canFire && ammoCount > minAmmo)
        {
            Vector2 projectileFireDirection = projectileSpawnPositions[0].up; projectileFireDirection.Normalize();

            /// INSTANTIATES THE OBJECT FROM THE POOL
            GameObject projectileSpawn = ObjectPoolManager.spawnObject(projectilePrefabs[0], projectileSpawnPositions[0].position, Quaternion.identity);

            if (projectilePrefabs[0].GetComponent<Rigidbody2D>() != null)
            {
                float projectileRotation = Mathf.Atan2(projectileFireDirection.y, projectileFireDirection.x) * Mathf.Rad2Deg;

                //GIVES THE OBJECT ITS ROTATION AND FORCE HERE
                projectileSpawn.transform.rotation = Quaternion.Euler(0f, 0f, projectileRotation - 90f);
                projectileSpawn.GetComponent<Rigidbody2D>().AddForce(projectileFireDirection * projectileSpeed, ForceMode2D.Impulse);

                TankAnimator.SetTrigger("GunFiring");

                if (shootVFX != null) { shootVFX.Play(); }
            }
            canFire = false;

            //                            (INTENSITY, FOR TIME)
            CinemachineShake.Instance.CameraShake(1.5f, .25f);

            ammoCount--; //MAKE UI WORK WITH THIS FOR DEPLETING AMMO

            fireDelay_CR = StartCoroutine(FireDelay());
        }

        else
        {
            reloadDelay_CR = StartCoroutine(ReloadDelay());
            //return;
        }

        //else return;
    }

    private IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(fireDelay);
        canFire = true;
    }

    private IEnumerator ReloadDelay()
    {
        canFire = false;

        for (int i = 0; i < maxAmmo; i++)
        {
            ammoCount++;
            yield return new WaitForSeconds(reloadRoundDelay);
            // ADD SFX HERE TO AUDIOLISE RELOADING
        }

        //ammoCount = maxAmmo;
        canFire = true;
    }
}