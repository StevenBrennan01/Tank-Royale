using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TankController : MonoBehaviour
{
    #region Inspector Header and Spacing
    [Header("                                                     -= Tank Controller =-")]
    [Space(15)]
    #endregion

    [SerializeField] private GameObject tankHull;
    [SerializeField] public GameObject tankTower;

    private Rigidbody2D rb;
    private Camera mainCam;

    private Vector3 mousePos;
    [HideInInspector] public Vector2 moveDir;

    #region Inspector Comments and Spacing
    [Header("Movement Values")]
    [Space(15)]
    #endregion

    [SerializeField] private float tankMoveSpeed;
    [SerializeField] private float hullRotateSpeed;
    [SerializeField] private float towerRotateSpeed;

    private void Awake()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveTank()
    {
        rb.AddForce(moveDir * tankMoveSpeed, ForceMode2D.Force);
        RotateHull();
    }

    private void RotateHull()
    {
        if (moveDir != Vector2.zero)
        {
            float angleTarget = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg - 90f;

            Quaternion targetRotation = Quaternion.Euler(0, 0, angleTarget);
            tankHull.transform.rotation = Quaternion.RotateTowards(tankHull.transform.rotation, targetRotation, hullRotateSpeed);
        }
    }

    public void RotateTower()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 gunRot = (mousePos - tankTower.transform.position);
        float targetPoint = Mathf.Atan2(gunRot.y, gunRot.x) * Mathf.Rad2Deg - 90f;

        Quaternion towerRotation = Quaternion.Euler(0, 0, targetPoint);
        tankTower.transform.rotation = Quaternion.Slerp(tankTower.transform.rotation, towerRotation, towerRotateSpeed);
    }
}