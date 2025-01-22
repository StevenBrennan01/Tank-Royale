using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "TankData")]
public class TankMovementSettingsSO : ScriptableObject
{
    public float tankMoveSpeed;
    public float hullRotateSpeed;
    public float towerRotateSpeed;
}