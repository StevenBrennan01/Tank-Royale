using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [SerializeField] private GameObject tankBullet;

    private Rigidbody2D rb;

    //REMOVE THIS WHEN DONE
    [HideInInspector] public Vector2 moveDir;

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