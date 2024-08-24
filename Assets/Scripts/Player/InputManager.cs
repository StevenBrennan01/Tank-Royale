using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {  get; private set; }

    private static InputActions inputActions_SCR;
    private static PlayerController tankController_SCR;
    private static ProjectileHandler projectileHandler_SCR;
    private static UIManager uiManager_SCR;

    private Coroutine movingCR;
    private Coroutine shootingCR;
    private Coroutine tankLookingCR;
    private Coroutine reloadingCR;

    private Animator TankAnimator;

    private bool isMoving;
    private bool tankCanLook;
    private bool gamePaused = false;

    private void Awake()
    {
        inputActions_SCR = new InputActions();

        tankController_SCR = GetComponent<PlayerController>();
        projectileHandler_SCR = GetComponent<ProjectileHandler>();
        uiManager_SCR = FindObjectOfType<UIManager>();

        TankAnimator = GetComponent<Animator>();
    }

    // In future, keep in mind that these can be subscribed and unsubscribed to
    // depending on if the scene actually needs to access them
    // For example only calling the player controls when in game, not in menu and vice versa

    private void OnEnable()
    {
        EnableAllTankActions();
    }

    private void OnDisable()
    {
        DisableAllTankActions();
    }

    // ==== METHODS ====

    #region Enable / Disable Tank Action Maps
    private void EnableAllTankActions()
    {
        inputActions_SCR.Player.Move.Enable();
        inputActions_SCR.Player.Move.performed += MovePerformed;
        inputActions_SCR.Player.Move.canceled += MoveCancelled;

        inputActions_SCR.Player.Shoot.Enable();
        inputActions_SCR.Player.Shoot.performed += ShootPerformed;
        inputActions_SCR.Player.Shoot.canceled += ShootCancelled;

        inputActions_SCR.Player.MouseLook.Enable();
        inputActions_SCR.Player.MouseLook.performed += MouseLooking;
        inputActions_SCR.Player.MouseLook.canceled += MouseCancelled;

        inputActions_SCR.Player.PauseGame.Enable();
        inputActions_SCR.Player.PauseGame.performed += PausePerformed;

        inputActions_SCR.Player.Reload.Enable();
        inputActions_SCR.Player.Reload.performed += ReloadPerformed;

        inputActions_SCR.Player.SpeedBoost.Enable();
        inputActions_SCR.Player.SpeedBoost.performed += SpeedBoostPerformed;
    }

    private void DisableAllTankActions()
    {
        inputActions_SCR.Player.Move.Disable();
        inputActions_SCR.Player.Move.performed -= MovePerformed;
        inputActions_SCR.Player.Move.canceled -= MoveCancelled;

        inputActions_SCR.Player.Shoot.Disable();
        inputActions_SCR.Player.Shoot.performed -= ShootPerformed;
        inputActions_SCR.Player.Shoot.canceled -= ShootCancelled;

        inputActions_SCR.Player.MouseLook.Disable();
        inputActions_SCR.Player.MouseLook.performed -= MouseLooking;
        inputActions_SCR.Player.MouseLook.canceled -= MouseCancelled;

        inputActions_SCR.Player.PauseGame.Disable();
        inputActions_SCR.Player.PauseGame.performed -= PausePerformed;

        inputActions_SCR.Player.Reload.Disable();
        inputActions_SCR.Player.Reload.performed -= ReloadCancelled;

        inputActions_SCR.Player.SpeedBoost.Disable();
        inputActions_SCR.Player.SpeedBoost.performed -= SpeedBoostCancelled;
    }

    #endregion

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

    private void MouseCancelled(InputAction.CallbackContext passThrough)
    {
        tankCanLook = false;

        if (tankLookingCR != null)
        {
            tankLookingCR = null;
        }
    }

    private void ShootPerformed(InputAction.CallbackContext button)
    {
        if (shootingCR == null)
        {
            shootingCR = StartCoroutine(tankShootingCR());
        }
    }

    private void ShootCancelled(InputAction.CallbackContext button)
    {
        shootingCR = null;
    }

    private void ReloadPerformed(InputAction.CallbackContext button)
    {
        if (!projectileHandler_SCR.isReloading)
        {
            reloadingCR = StartCoroutine(tankReloadingCR());
        }
    }

    private void ReloadCancelled(InputAction.CallbackContext button)
    {
        reloadingCR = null;
    }

    private void SpeedBoostPerformed(InputAction.CallbackContext button)
    {
        //coroutine / uni.task? in player controller that gives temporary speed
    }
    private void SpeedBoostCancelled(InputAction.CallbackContext button)
    {

    }

    private void PausePerformed(InputAction.CallbackContext button)
    {
        if (!gamePaused)
        {
            Time.timeScale = 0;
            uiManager_SCR.pauseMenuUI.SetActive(true);
            gamePaused = true;
        }
        else if (gamePaused)
        {
            Time.timeScale = 1;
            uiManager_SCR.pauseMenuUI.SetActive(false);
            gamePaused = false;
        }
    }

    // ==== COROUTINES ====

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
        while (projectileHandler_SCR.canFire)
        {
            projectileHandler_SCR.TankFired();
            yield return null;
        }
    }

    private IEnumerator tankReloadingCR()
    {
        StartCoroutine(projectileHandler_SCR.ReloadDelay());
        yield return null;
    }
}