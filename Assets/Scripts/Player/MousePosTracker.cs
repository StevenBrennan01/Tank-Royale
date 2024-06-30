using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosTracker : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;

    void Update()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        Vector3 mouseWorldPosition = m_Camera.ScreenToWorldPoint(mouseScreenPosition);

        transform.position = mouseWorldPosition;
    }
}