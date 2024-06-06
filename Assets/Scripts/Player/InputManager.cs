using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    InputActions inputActions_SCR; 
    PlayerMove playerMove_SCR;

    private Coroutine movingCR;
    private Coroutine shootingCR;

    private bool isMoving;

    private void Awake()
    {
        inputActions_SCR = new InputActions();
        playerMove_SCR = GetComponent<PlayerMove>();
    }

    private void OnEnable()
    {
        inputActions_SCR.Player.Move.Enable();
        inputActions_SCR.Player.Move.performed += MovePerformed;
        inputActions_SCR.Player.Move.canceled += MoveCancelled;

        //inputActions_SCR.Player.Shoot.Enable();
        //inputActions_SCR.Player.Shoot.performed += TankShooting;
    }

    private void OnDisable()
    {
        inputActions_SCR.Player.Move.Disable();
        inputActions_SCR.Player.Move.performed -= MovePerformed;
        inputActions_SCR.Player.Move.canceled -= MoveCancelled;

        //inputActions_SCR.Player.Shoot.Disable();
        //inputActions_SCR.Player.Shoot.performed -= TankShooting;
    }

    private void MovePerformed(InputAction.CallbackContext value)
    {
        Debug.Log(value.ReadValue<Vector2>());
        playerMove_SCR.moveDir = value.ReadValue<Vector2>();
        isMoving = true;

        if (movingCR == null)
        {
            movingCR = StartCoroutine(tankMovingCR());
        }
    }

    private void MoveCancelled(InputAction.CallbackContext value)
    {
        playerMove_SCR.moveDir = value.ReadValue<Vector2>();
        isMoving = false;

        if (movingCR != null)
        {
            movingCR = null;
        }
    }

    private IEnumerator tankMovingCR()
    {
        while (isMoving)
        {
            playerMove_SCR.tempMovement();
            yield return null;
        }
    }
}