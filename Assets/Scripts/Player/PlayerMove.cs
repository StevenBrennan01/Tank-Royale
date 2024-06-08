using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;

    public Vector2 moveDir;

    #region Inspector Header and Spacing
    [Header("                                                -= Movement Attributes =-")]
    [Space(15)]
    #endregion

    [SerializeField] private float tankMoveSpeed;
    [SerializeField] private float tankRotateSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveTank()
    {
        rb.AddForce(moveDir * tankMoveSpeed, ForceMode2D.Force);
        
    }

    public void TankShoot()
    {

    }
}