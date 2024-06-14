using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputActions inputActions_SCR;
    private TankController tankController_SCR;

    private Coroutine movingCR;
    private Coroutine shootingCR;
    private Coroutine tankLookingCR;

    private Animator TankAnimator;

    private bool isMoving;
    private bool canShoot;
    private bool tankCanLook;

    private void Awake()
    {
        inputActions_SCR = new InputActions();
        tankController_SCR = GetComponent<TankController>();

        TankAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        inputActions_SCR.Player.Move.Enable();
        inputActions_SCR.Player.Move.performed += MovePerformed;
        inputActions_SCR.Player.Move.canceled += MoveCancelled;

        inputActions_SCR.Player.Shoot.Enable();
        inputActions_SCR.Player.Shoot.performed += ShootPerformed;

        inputActions_SCR.Player.MouseLook.Enable();
        inputActions_SCR.Player.MouseLook.performed += MouseLooking;
    }

    private void OnDisable()
    {
        inputActions_SCR.Player.Move.Disable();
        inputActions_SCR.Player.Move.performed -= MovePerformed;
        inputActions_SCR.Player.Move.canceled -= MoveCancelled;

        inputActions_SCR.Player.Shoot.Disable();
        inputActions_SCR.Player.Shoot.performed -= ShootPerformed;
    }

    private void MovePerformed(InputAction.CallbackContext value)
    {
        tankController_SCR.moveDir = value.ReadValue<Vector2>();
        isMoving = true;

        if (movingCR == null)
        {
            TankAnimator.SetBool("TracksMoving", true);
            movingCR = StartCoroutine(tankMovingCR());
        }
    }

    private void MoveCancelled(InputAction.CallbackContext value)
    {
        tankController_SCR.moveDir = value.ReadValue<Vector2>();
        isMoving = false;

        if (movingCR != null)
        {
            TankAnimator.SetBool("TracksMoving", false);
            movingCR = null;
        }
    }

    private void MouseLooking(InputAction.CallbackContext passThrough)
    {
        tankCanLook = true;

        if (tankLookingCR == null)
        {
            tankLookingCR = StartCoroutine(tankLooking());
        }
    }

    private void ShootPerformed(InputAction.CallbackContext button)
    {
        Debug.Log("TANK IS SHOOTING");
        TankAnimator.SetTrigger("GunFiring");
        shootingCR = StartCoroutine(tankShootingCR());
    }

    private IEnumerator tankMovingCR()
    {
        while (isMoving)
        {
            tankController_SCR.MoveTank();
            yield return null;
        }
    }

    private IEnumerator tankLooking()
    {
        while (tankCanLook)
        {
            tankController_SCR.RotateTower();
            yield return null;
        }
    }

    private IEnumerator tankShootingCR()
    {
        while (canShoot)
        {
            tankController_SCR.TankShoot();

            canShoot = false;
            yield return new WaitForSeconds(0.3f);
            canShoot = true;
        }
    }
}