using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosTracker : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;

    void Update()
    {
        Vector2 mouseScreenPosition = Input.mousePosition;
        Vector2 mouseWorldPosition = m_Camera.ScreenToWorldPoint(mouseScreenPosition);

        transform.position = mouseWorldPosition;
    }
}