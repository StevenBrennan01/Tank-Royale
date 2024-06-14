using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    private TankController tankController_SCR;

    #region Inspector Header and Spacing
    [Header("                                                  -= Projectile Attributes =-")]
    [Space(15)]
    #endregion

    [SerializeField] private Transform[] projectileSpawnPositions;
    public int index;

    [SerializeField] private GameObject projectilePrefab;
    //ADD MORE LATER IF SWITCHING PROJECTILES IS ADDED

    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireDelay;

    [HideInInspector] public bool canFire = true;

    private Vector3 mousePos;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    public void TankFired()
    {
        if (canFire)
        {
            //FINDING MOUSE POSITION IN WORLD SPACE TO SEND PROJECTILE
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 projectileFireDirection = (mousePos - projectileSpawnPositions[0].position); projectileFireDirection.Normalize();

            GameObject projectileSpawn = Instantiate(projectilePrefab, projectileSpawnPositions[0].position, Quaternion.identity);

            if (projectilePrefab.GetComponent<Rigidbody2D>() != null)
            {
                float projectileRotation = Mathf.Atan2(projectileFireDirection.y, projectileFireDirection.x) * Mathf.Rad2Deg;

                projectileSpawn.transform.rotation = Quaternion.Euler(0f, 0f, projectileRotation - 90f);
                projectileSpawn.GetComponent<Rigidbody2D>().AddForce(projectileFireDirection * projectileSpeed, ForceMode2D.Impulse);
            }
            CinemachineShake.Instance.CameraShake(5f, .2f);

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