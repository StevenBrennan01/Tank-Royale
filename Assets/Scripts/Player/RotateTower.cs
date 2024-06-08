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
        //GIVES THE MOUSE LOCATION A VALUE IN WORLD SPACE
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        //FINDING THE ANGLE BETWEEN THE MOUSE AND TANK TOWER
        Vector3 gunRot = (mousePos - transform.position).normalized;

        float targetPoint = Mathf.Atan2(gunRot.y, gunRot.x) * Mathf.Rad2Deg - 90f;

        //QUATERNION STUFF THAT IS TRICKY BUT LERPS THE TOWER TO FEEL BETTER
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetPoint);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, towerLerpSpeed * Time.deltaTime);
    }
}