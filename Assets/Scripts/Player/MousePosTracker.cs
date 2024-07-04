using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosTracker : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private Transform playerPos;

    [Space(10)]

    [SerializeField] private float maxCamDistance = 15f;

    private void Update()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 direction = (mousePosition - playerPos.position).normalized;
        float distance = Vector3.Distance(mousePosition, playerPos.position);

        if (distance > maxCamDistance)
        {
            mousePosition = playerPos.position + direction * maxCamDistance;
        }

        transform.position = mousePosition;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.nearClipPlane;

        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }
}