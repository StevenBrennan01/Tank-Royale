using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TankController : MonoBehaviour
{
    #region Inspector Header and Spacing
    [Header("                                                    -= Tank Controller =-")]
    [Space(15)]
    #endregion

    [SerializeField] private GameObject tankProjectile;
    [SerializeField] private GameObject tankHull;
    [SerializeField] private GameObject tankTower;

    private Rigidbody2D rb;
    private Camera mainCam;

    private Vector3 mousePos;
    [HideInInspector] public Vector3 moveDir;

    #region Inspector Header and Spacing
    [Header("                                                -= Movement Attributes =-")]
    [Space(15)]
    #endregion

    [SerializeField] private float tankMoveSpeed;
    [SerializeField] private float hullRotateSpeed;
    [SerializeField] private float towerRotateSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        mainCam = Camera.main;
    }

    public void MoveTank()
    {
        rb.AddForce(moveDir * tankMoveSpeed, ForceMode2D.Force);
        RotateHull();
    }

    private void RotateHull()
    {
        if (moveDir != Vector3.zero)
        {
            float angleTarget = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angleTarget);
            tankHull.transform.rotation = Quaternion.RotateTowards(tankHull.transform.rotation, targetRotation, hullRotateSpeed * Time.deltaTime);
        }
    }

    public void TowerRotate()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 gunRot = (mousePos - tankTower.transform.position);
        float targetPoint = Mathf.Atan2(gunRot.y, gunRot.x) * Mathf.Rad2Deg - 90f;

        Quaternion towerRotation = Quaternion.Euler(0, 0, targetPoint);
        tankTower.transform.rotation = Quaternion.Slerp(tankTower.transform.rotation, towerRotation, towerRotateSpeed * Time.deltaTime);
    }

    public void TankShoot()
    {

    }
}