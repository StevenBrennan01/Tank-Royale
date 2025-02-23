using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "TankData")]
public class TankAttributesSO : ScriptableObject
{
    [Header("Main Attributes")]
    [Space (10)]
    public float tankMoveSpeed;
    public float tankHealth;

    [Header("Secondary Attributes")]
    [Space(10)]
    public float hullRotateSpeed;
    public float towerRotateSpeed;
}