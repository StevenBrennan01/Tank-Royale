using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTower : MonoBehaviour
{
    private Camera mainCam;
    private Vector3 mousePos;

    [SerializeField] private float towerLerpSpeed;

    private void Awake()
    {
        mainCam = Camera.main;     
    }

    private void Update()
    {
        //GIVES THE MOUSE POSITION A VALUE IN WORLD SPACE
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        //FINDING THE ANGLE BETWEEN THE MOUSE AND TANK TOWER
        Vector3 gunRot = (mousePos - transform.position).normalized;

        float targetPoint = Mathf.Atan2(gunRot.y, gunRot.x) * Mathf.Rad2Deg - 90f;

        //QUATERNION STUFF THAT SLERPS THE TOWER POINT TO THE MOUSE DIRECTION
        Quaternion towerRotation = Quaternion.Euler(0, 0, targetPoint);
        transform.rotation = Quaternion.Slerp(transform.rotation, towerRotation, towerLerpSpeed * Time.deltaTime);
    }
}