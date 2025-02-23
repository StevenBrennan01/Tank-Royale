using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Inspector Header and Spacing
    [Header("                                                     -= Tank Controller =-")]
    [Space(15)]
    #endregion

    [SerializeField] private GameObject tankHull;
    [SerializeField] public GameObject tankTower;

    //TankData
    [SerializeField] private TankAttributesSO tankData;

    private Rigidbody2D rb;
    private Camera mainCam;

    private Vector3 mousePos;
    [HideInInspector] public Vector2 moveDir;

    #region Inspector Comments and Spacing
    [Header("Movement Values")]
    [Space(15)]
    #endregion

    private float zRotation;

    private void Awake()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();

        if (tankData == null)
        {
            Debug.LogError("No tank data has been attached!");
        }
    }

    private void OnEnable()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveTank()
    {
        rb.AddForce(transform.up * moveDir.y * tankData.tankMoveSpeed * Time.deltaTime, ForceMode2D.Force);
        RotateHull();
    }

    #region Tank Rotations
    private void RotateHull()
    {
        if (moveDir.x != 0)
        {
            zRotation += moveDir.x * tankData.hullRotateSpeed * Time.deltaTime;
            rb.rotation = -zRotation;
        }
    }

    public void RotateTower()
    {
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 towerRot = (mousePos - tankTower.transform.position);
        float targetPoint = Mathf.Atan2(towerRot.y, towerRot.x) * Mathf.Rad2Deg - 90f;

        Quaternion towerRotation = Quaternion.Euler(0, 0, targetPoint);
        tankTower.transform.rotation = Quaternion.Slerp(tankTower.transform.rotation, towerRotation, tankData.towerRotateSpeed);
    }
    #endregion
}